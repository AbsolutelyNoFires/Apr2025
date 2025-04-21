using Neb25.Core.Galaxy;
using System;
using System.Drawing;
using System.Numerics; // For Vector3, Matrix4x4
using System.Windows.Forms;
using System.Collections.Generic; // For List
using System.Drawing.Drawing2D; // For SmoothingMode, Matrix
using System.Linq; // Required for FirstOrDefault, Any, Max, OrderByDescending

namespace Neb25.UI.Forms
{
	public partial class Game : Form
	{
		// Store the galaxy passed to the form
		private readonly Core.Galaxy.Galaxy _currentGalaxy;

		// --- Visualization Fields ---
		private PointF _viewOffset = PointF.Empty;
		private float _zoomLevel = 0.05f;
		private Point _lastMousePosition = Point.Empty;
		private bool _isPanning = false;
		private bool _isRotating = false;
		private float _rotationX = 0.0f;
		private float _rotationY = 0.0f;
		private Matrix4x4 _viewMatrix = Matrix4x4.Identity;

		private const float MinZoom = 0.005f;
		private const float MaxZoom = 10.0f;
		private const float ZoomFactor = 1.2f;
		private const float RotationSpeed = 0.005f;

		// --- Interaction State ---
		private StarSystem? _hoveredSystem = null;
		private StarSystem? _selectedSystem = null;
		private RectangleF _selectedSystemPanelBounds;
		private RectangleF _enterSystemButtonBounds;

		// --- Brushes, Pens, Fonts ---
		private readonly Brush _starBrush = Brushes.White;
		private readonly Brush _hoverStarBrush = Brushes.Yellow;
		private readonly Brush _selectedStarBrush = Brushes.Cyan;
		private readonly Pen _hyperlanePen = new Pen(Color.FromArgb(100, 100, 100, 255), 1.0f); // Semi-transparent blue-gray lines
		private readonly Brush _panelBackgroundBrush = new SolidBrush(Color.FromArgb(200, 40, 40, 60));
		private readonly Brush _panelTextBrush = Brushes.White;
		private readonly Pen _panelBorderPen = Pens.Cyan;
		private readonly Brush _buttonBrush = Brushes.DarkCyan;
		private readonly Brush _buttonTextBrush = Brushes.White;
		private readonly Font _panelFont = new Font("Consolas", 9f);
		private readonly Font _buttonFont = new Font("Consolas", 10f, FontStyle.Bold);
		private readonly StringFormat _centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };


		public Game(Core.Galaxy.Galaxy galaxy)
		{
			InitializeComponent();
			_currentGalaxy = galaxy ?? throw new ArgumentNullException(nameof(galaxy), "A valid galaxy must be provided.");
			SetupGalaxyView();
			UpdateViewMatrix();
		}

		private void UpdateViewMatrix()
		{
			Matrix4x4 rotX = Matrix4x4.CreateRotationX(_rotationX);
			Matrix4x4 rotY = Matrix4x4.CreateRotationY(_rotationY);
			_viewMatrix = rotY * rotX;
		}

		private void SetupGalaxyView()
		{
			if (this.Controls.Find("galaxyPictureBox", true).FirstOrDefault() is PictureBox pb)
			{
				pb.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?.SetValue(pb, true, null);
				pb.BackColor = Color.Black;
				pb.Paint += GalaxyPictureBox_Paint;
				pb.MouseDown += GalaxyPictureBox_MouseDown;
				pb.MouseMove += GalaxyPictureBox_MouseMove;
				pb.MouseUp += GalaxyPictureBox_MouseUp;
				pb.MouseWheel += GalaxyPictureBox_MouseWheel;
				pb.MouseLeave += GalaxyPictureBox_MouseLeave;
				pb.Resize += (sender, e) => pb.Invalidate();

				// Calculate initial zoom
				if (_currentGalaxy.StarSystems.Any())
				{
					float maxDist = _currentGalaxy.StarSystems.Max(s => Math.Max(Math.Max(Math.Abs(s.Position.X), Math.Abs(s.Position.Y)), Math.Abs(s.Position.Z)));
					if (maxDist > 1.0f && pb.Width > 0 && pb.Height > 0)
					{
						float zoomX = pb.Width / (maxDist * 2.5f);
						float zoomY = pb.Height / (maxDist * 2.5f);
						_zoomLevel = Math.Min(zoomX, zoomY);
						_zoomLevel = Math.Clamp(_zoomLevel, MinZoom, MaxZoom);
					}
				}
			}
			else
			{
				MessageBox.Show("Error: Galaxy PictureBox ('galaxyPictureBox') not found...", "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void Game_Load(object sender, EventArgs e)
		{
			if (this.Controls.Find("galaxyPictureBox", true).FirstOrDefault() is PictureBox pb)
			{
				pb.Invalidate();
			}
		}

		// --- Event Handlers ---

		private void GalaxyPictureBox_Paint(object? sender, PaintEventArgs e)
		{
			if (sender is not PictureBox pb) return;

			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.Clear(pb.BackColor);

			PointF viewCenter = new PointF(pb.Width / 2.0f, pb.Height / 2.0f);

			// Pre-calculate transformed positions for all systems
			var transformedSystems = _currentGalaxy.StarSystems
				.Select(s => new { System = s, TransformedPos = Vector3.Transform(s.Position, _viewMatrix) })
				.ToDictionary(item => item.System, item => item.TransformedPos);

			// --- Hyperlane Drawing (Draw first, so stars are on top) ---
			var drawnLanes = new HashSet<Tuple<StarSystem, StarSystem>>(); // Prevent drawing lanes twice (A->B and B->A)
			foreach (var startSystem in _currentGalaxy.StarSystems)
			{
				// Check if this system and its transformed position exist in our dictionary
				if (!transformedSystems.TryGetValue(startSystem, out Vector3 startTransformedPos)) continue;

				PointF startScreenPos = WorldToScreen(startTransformedPos, viewCenter, _viewOffset, _zoomLevel);

				foreach (var jumpSite in startSystem.JumpSites)
				{
					if (jumpSite.HasPartner && jumpSite.Partner != null)
					{
						var endSystem = jumpSite.Partner.ParentStarSystem;

						// Ensure we only draw lane A->B once, using system IDs for comparison
						var pair = (startSystem.Id.CompareTo(endSystem.Id) < 0)
							? Tuple.Create(startSystem, endSystem)
							: Tuple.Create(endSystem, startSystem);

						if (drawnLanes.Add(pair)) // If the pair was successfully added (i.e., not drawn yet)
						{
							// Check if the end system and its transformed position exist
							if (transformedSystems.TryGetValue(endSystem, out Vector3 endTransformedPos))
							{
								PointF endScreenPos = WorldToScreen(endTransformedPos, viewCenter, _viewOffset, _zoomLevel);

								// Optional: Cull lanes far off-screen (check if both points are way out)
								float cullMargin = Math.Max(pb.Width, pb.Height); // Generous margin
								bool startOffscreen = startScreenPos.X < -cullMargin || startScreenPos.X > pb.Width + cullMargin ||
													  startScreenPos.Y < -cullMargin || startScreenPos.Y > pb.Height + cullMargin;
								bool endOffscreen = endScreenPos.X < -cullMargin || endScreenPos.X > pb.Width + cullMargin ||
													endScreenPos.Y < -cullMargin || endScreenPos.Y > pb.Height + cullMargin;

								if (!(startOffscreen && endOffscreen)) // Draw if at least one point is potentially visible
								{
									g.DrawLine(_hyperlanePen, startScreenPos, endScreenPos);
								}
							}
						}
					}
				}
			}


			// --- Star Drawing ---
			// Sort stars by transformed Z for pseudo-3D effect (closer stars draw on top)
			var sortedSystems = transformedSystems
				.OrderBy(kvp => kvp.Value.Z) // Sort by Z (draw furthest first)
				.ToList();

			foreach (var kvp in sortedSystems)
			{
				StarSystem system = kvp.Key;
				Vector3 transformedPos = kvp.Value; // Use pre-calculated transformed position

				PointF screenPos = WorldToScreen(transformedPos, viewCenter, _viewOffset, _zoomLevel);

				float starDrawSize = Math.Max(1.0f, 2.0f * _zoomLevel);
				float cullMargin = starDrawSize * 2;
				if (screenPos.X < -cullMargin || screenPos.X > pb.Width + cullMargin ||
					screenPos.Y < -cullMargin || screenPos.Y > pb.Height + cullMargin)
				{
					continue; // Cull stars far off-screen
				}

				// Determine star color/brush
				Brush currentStarBrush = _starBrush;
				if (system == _selectedSystem) currentStarBrush = _selectedStarBrush;
				else if (system == _hoveredSystem) currentStarBrush = _hoverStarBrush;

				// Draw the star
				g.FillEllipse(currentStarBrush, screenPos.X - starDrawSize / 2, screenPos.Y - starDrawSize / 2, starDrawSize, starDrawSize);
			}

			// --- Hover Info Drawing ---
			if (_hoveredSystem != null && _hoveredSystem != _selectedSystem)
			{
				DrawHoverPanel(g, _hoveredSystem, _lastMousePosition);
			}

			// --- Selected Info Panel Drawing ---
			if (_selectedSystem != null)
			{
				DrawSelectedPanel(g, _selectedSystem, pb.ClientRectangle);
			}
		}

		private void GalaxyPictureBox_MouseDown(object? sender, MouseEventArgs e)
		{
			_lastMousePosition = e.Location;
			(sender as Control)?.Focus();

			if (e.Button == MouseButtons.Left)
			{
				if (_selectedSystem != null && _enterSystemButtonBounds.Contains(e.Location)) return;
				_isPanning = true;
				(sender as Control)!.Cursor = Cursors.Hand;
			}
			else if (e.Button == MouseButtons.Right)
			{
				_isRotating = true;
				(sender as Control)!.Cursor = Cursors.SizeAll;
			}
			 (sender as Control)!.Capture = true;
		}

		private void GalaxyPictureBox_MouseMove(object? sender, MouseEventArgs e)
		{
			Point currentMousePosition = e.Location;
			float dx = currentMousePosition.X - _lastMousePosition.X;
			float dy = currentMousePosition.Y - _lastMousePosition.Y;
			bool needsRedraw = false;

			if (_isPanning)
			{
				if (Math.Abs(_zoomLevel) > 1e-6)
				{
					_viewOffset.X -= dx / _zoomLevel;
					_viewOffset.Y -= dy / _zoomLevel;
					needsRedraw = true;
				}
			}
			else if (_isRotating)
			{
				_rotationY += dx * RotationSpeed;
				_rotationX += dy * RotationSpeed;
				_rotationX = Math.Clamp(_rotationX, -(float)Math.PI / 2.0f + 0.1f, (float)Math.PI / 2.0f - 0.1f);
				UpdateViewMatrix();
				needsRedraw = true;
			}
			else
			{
				StarSystem? previouslyHovered = _hoveredSystem;
				_hoveredSystem = FindSystemAtScreenPoint(currentMousePosition, (sender as PictureBox)!);
				if (_hoveredSystem != previouslyHovered) needsRedraw = true;

				bool overPanel = _selectedSystem != null && _selectedSystemPanelBounds.Contains(currentMousePosition);
				bool overButton = _selectedSystem != null && _enterSystemButtonBounds.Contains(currentMousePosition);
				(sender as Control)!.Cursor = (_hoveredSystem != null || overPanel || overButton) ? Cursors.Hand : Cursors.Default;
			}

			_lastMousePosition = currentMousePosition;
			if (needsRedraw)
			{
				(sender as PictureBox)?.Invalidate();
			}
		}

		private void GalaxyPictureBox_MouseUp(object? sender, MouseEventArgs e)
		{
			float dragDistanceSq = (e.X - _lastMousePosition.X) * (e.X - _lastMousePosition.X) +
								  (e.Y - _lastMousePosition.Y) * (e.Y - _lastMousePosition.Y);
			bool isClick = dragDistanceSq < 25.0f;

			bool wasPanning = _isPanning;
			bool wasRotating = _isRotating;

			_isPanning = false;
			_isRotating = false;
			(sender as Control)!.Capture = false;

			bool overPanel = _selectedSystem != null && _selectedSystemPanelBounds.Contains(e.Location);
			bool overButton = _selectedSystem != null && _enterSystemButtonBounds.Contains(e.Location);
			_hoveredSystem = FindSystemAtScreenPoint(e.Location, (sender as PictureBox)!);
			(sender as Control)!.Cursor = (_hoveredSystem != null || overPanel || overButton) ? Cursors.Hand : Cursors.Default;

			if (e.Button == MouseButtons.Left && isClick)
			{
				if (_selectedSystem != null && _enterSystemButtonBounds.Contains(e.Location))
				{
					EnterSystemView(_selectedSystem);
					(sender as PictureBox)?.Invalidate();
					return;
				}
				if (_selectedSystem != null && _selectedSystemPanelBounds.Contains(e.Location))
				{
					(sender as PictureBox)?.Invalidate();
					return;
				}

				StarSystem? clickedSystem = FindSystemAtScreenPoint(e.Location, (sender as PictureBox)!);
				if (_selectedSystem != clickedSystem)
				{
					_selectedSystem = clickedSystem;
					(sender as PictureBox)?.Invalidate();
				}
				else if (clickedSystem == null)
				{
					_selectedSystem = null;
					(sender as PictureBox)?.Invalidate();
				}
			}
			else if (!isClick && (wasPanning || wasRotating))
			{
				(sender as PictureBox)?.Invalidate();
			}
		}

		private void GalaxyPictureBox_MouseWheel(object? sender, MouseEventArgs e)
		{
			if (sender is not PictureBox pb) return;
			pb.Focus();

			float zoomMultiplier = (e.Delta > 0) ? ZoomFactor : 1.0f / ZoomFactor;
			float newZoom = _zoomLevel * zoomMultiplier;
			newZoom = Math.Clamp(newZoom, MinZoom, MaxZoom);

			if (Math.Abs(newZoom - _zoomLevel) < float.Epsilon) return;

			PointF mousePos = e.Location;
			PointF viewCenter = new PointF(pb.Width / 2.0f, pb.Height / 2.0f);
			PointF worldPosUnderMouse = ScreenToWorld(mousePos, viewCenter);
			_zoomLevel = newZoom;
			PointF screenPosAfterZoom = WorldToScreen(new Vector3(worldPosUnderMouse.X, worldPosUnderMouse.Y, 0), viewCenter, _viewOffset, _zoomLevel);

			if (Math.Abs(_zoomLevel) > float.Epsilon)
			{
				_viewOffset.X -= (mousePos.X - screenPosAfterZoom.X) / _zoomLevel;
				_viewOffset.Y -= (mousePos.Y - screenPosAfterZoom.Y) / _zoomLevel;
			}
			pb.Invalidate();
		}

		private void GalaxyPictureBox_MouseLeave(object? sender, EventArgs e)
		{
			if (_hoveredSystem != null)
			{
				_hoveredSystem = null;
				(sender as PictureBox)?.Invalidate();
			}
			if (_isPanning || _isRotating)
			{
				_isPanning = false;
				_isRotating = false;
				(sender as Control)!.Capture = false;
				(sender as Control)!.Cursor = Cursors.Default;
				(sender as PictureBox)!.Invalidate();
			}
		}

		// --- Drawing Helpers ---
		private void DrawHoverPanel(Graphics g, StarSystem system, Point mousePos)
		{
			string info = $"{system.Name}\nPlanets: {system.Planets.Count}";
			SizeF size = g.MeasureString(info, _panelFont);
			float padding = 5f;
			RectangleF panelRect = new RectangleF(
				mousePos.X + 15, mousePos.Y + 10,
				size.Width + 2 * padding, size.Height + 2 * padding);

			RectangleF clientRect = g.VisibleClipBounds;
			if (panelRect.Right > clientRect.Width) panelRect.X = mousePos.X - panelRect.Width - 15;
			if (panelRect.Bottom > clientRect.Height) panelRect.Y = mousePos.Y - panelRect.Height - 10;
			if (panelRect.Left < 0) panelRect.X = 0;
			if (panelRect.Top < 0) panelRect.Y = 0;

			g.FillRectangle(_panelBackgroundBrush, panelRect);
			g.DrawRectangle(Pens.Yellow, panelRect.X, panelRect.Y, panelRect.Width, panelRect.Height);
			g.DrawString(info, _panelFont, _panelTextBrush, panelRect.X + padding, panelRect.Y + padding);
		}

		private void DrawSelectedPanel(Graphics g, StarSystem system, Rectangle clientRect)
		{
			float panelWidth = 200;
			float panelHeight = 170; // Increased height slightly for neighbor count
			float padding = 10f;
			float buttonHeight = 25f;
			float buttonWidth = panelWidth - 2 * padding;

			_selectedSystemPanelBounds = new RectangleF(
				padding, clientRect.Height - panelHeight - padding, panelWidth, panelHeight);

			g.FillRectangle(_panelBackgroundBrush, _selectedSystemPanelBounds);
			g.DrawRectangle(_panelBorderPen, _selectedSystemPanelBounds.X, _selectedSystemPanelBounds.Y, _selectedSystemPanelBounds.Width, _selectedSystemPanelBounds.Height);

			float currentY = _selectedSystemPanelBounds.Y + padding;
			float contentWidth = panelWidth - 2 * padding;

			// System Name
			SizeF nameSize = g.MeasureString(system.Name, _buttonFont, (int)contentWidth);
			RectangleF nameRect = new RectangleF(_selectedSystemPanelBounds.X + padding, currentY, contentWidth, nameSize.Height);
			g.DrawString(system.Name, _buttonFont, _panelTextBrush, nameRect);
			currentY += nameSize.Height + padding / 2;

			// Planet info - we'll add info about jump points later
			g.DrawString($"Planets: {system.Planets.Count}", _panelFont, _panelTextBrush, _selectedSystemPanelBounds.X + padding, currentY);
			currentY += _panelFont.Height;


			// Position
			g.DrawString($"Pos: ({system.Position.X:F0}, {system.Position.Y:F0}, {system.Position.Z:F0})", _panelFont, _panelTextBrush, _selectedSystemPanelBounds.X + padding, currentY);
			currentY += _panelFont.Height;

			// ASCII Graphic
			string asciiArt = "   (*)   \n  / | \\ \n (.'.)\n  \\ | / \n   ---   "; // Slightly different art
			float requiredArtHeight = _panelFont.Height * 5;
			if (currentY + requiredArtHeight < _selectedSystemPanelBounds.Bottom - buttonHeight - padding * 2)
			{
				g.DrawString(asciiArt, _panelFont, Brushes.LightGreen, _selectedSystemPanelBounds.X + padding, currentY);
			}

			// Enter System Button
			_enterSystemButtonBounds = new RectangleF(
			   _selectedSystemPanelBounds.X + padding, _selectedSystemPanelBounds.Bottom - buttonHeight - padding,
			   buttonWidth, buttonHeight);
			g.FillRectangle(_buttonBrush, _enterSystemButtonBounds);
			g.DrawString("Enter System", _buttonFont, _buttonTextBrush, _enterSystemButtonBounds, _centerFormat);
		}

		// --- Interaction Logic ---
		private StarSystem? FindSystemAtScreenPoint(PointF screenPoint, PictureBox pb)
		{
			PointF viewCenter = new PointF(pb.Width / 2.0f, pb.Height / 2.0f);
			float clickRadius = Math.Max(5.0f, 3.0f * _zoomLevel);
			float clickRadiusSq = clickRadius * clickRadius;

			// Pre-calculate transformed positions if not already done (or pass them in)
			// For simplicity here, we recalculate, but could optimize by passing the dictionary
			var transformedSystems = _currentGalaxy.StarSystems
			   .Select(s => new { System = s, TransformedPos = Vector3.Transform(s.Position, _viewMatrix) })
			   .OrderByDescending(s => s.TransformedPos.Z) // Closest first
			   .ToList();

			foreach (var item in transformedSystems)
			{
				PointF starScreenPos = WorldToScreen(item.TransformedPos, viewCenter, _viewOffset, _zoomLevel);
				float dx = screenPoint.X - starScreenPos.X;
				float dy = screenPoint.Y - starScreenPos.Y;
				float distSq = dx * dx + dy * dy;

				if (distSq <= clickRadiusSq)
				{
					return item.System;
				}
			}
			return null;
		}

		private void EnterSystemView(StarSystem system)
		{
			
			// TODO: Implement SystemViewForm
			var systemForm = new SystemViewForm(system);
			systemForm.Show();
		}

		// --- Coordinate Transformation Helpers ---
		private PointF WorldToScreen(Vector3 rotatedWorldPos, PointF viewCenter, PointF offset, float zoom)
		{
			float worldX = rotatedWorldPos.X;
			float worldY = rotatedWorldPos.Y;
			float offsetX = worldX - offset.X;
			float offsetY = worldY - offset.Y;
			float zoomedX = offsetX * zoom;
			float zoomedY = offsetY * zoom;
			float screenX = viewCenter.X + zoomedX;
			float screenY = viewCenter.Y + zoomedY;
			return new PointF(screenX, screenY);
		}

		private PointF ScreenToWorld(PointF screenPos, PointF viewCenter)
		{
			if (Math.Abs(_zoomLevel) < float.Epsilon) return _viewOffset;
			float relativeX = screenPos.X - viewCenter.X;
			float relativeY = screenPos.Y - viewCenter.Y;
			float unzoomedX = relativeX / _zoomLevel;
			float unzoomedY = relativeY / _zoomLevel;
			float worldX = unzoomedX + _viewOffset.X;
			float worldY = unzoomedY + _viewOffset.Y;
			return new PointF(worldX, worldY);
		}

		// Dispose cached graphics objects
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_hyperlanePen?.Dispose(); // Dispose the new pen
				_panelBackgroundBrush?.Dispose();
				_buttonBrush?.Dispose();
				_panelFont?.Dispose();
				_buttonFont?.Dispose();
				_centerFormat?.Dispose();
				components?.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
