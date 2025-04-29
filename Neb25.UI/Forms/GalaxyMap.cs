using Neb25.Core.Galaxy; // Required for Galaxy, StarSystem etc.
using Neb25.Core.Utils;  // Required for GameFunctions
using System;               // Required for MathF, Guid etc.
using System.Collections.Generic; // Required for HashSet, List, Dictionary
using System.Drawing;           // Required for Graphics, Point, Color etc.
using System.Drawing.Drawing2D; // Required for SmoothingMode
using System.Linq;              // Required for FirstOrDefault, GroupBy etc.
using System.Numerics;          // Required for Vector3, Matrix4x4
using System.Windows.Forms;     // Required for Form, MouseEventArgs etc.


namespace Neb25.UI.Forms
{
	public partial class GalaxyMap : Form
	{
		private readonly Core.Galaxy.Galaxy _galaxy;

		// --- Camera State ---
		private Vector3 _cameraPosition = new Vector3(0, 0, 500);
		private Vector3 _cameraTarget = Vector3.Zero;
		private Vector3 _cameraUp = Vector3.UnitY;
		private float _cameraFov = MathF.PI / 4.0f;
		private float _nearPlane = 1.0f;
		private float _farPlane = 10000.0f;

		// --- Mouse Interaction State ---
		private Point _mouseDownPosition;
		private Point _lastMousePosition;
		private bool _isRotating = false;
		private bool _isPanning = false;
		private bool _isBoxSelecting = false;
		private Point _selectionRectStartPoint;
		private Rectangle _selectionRectCurrent;
		private const int MaxClickDragDistance = 5 * 5;

		// --- Selection & Hover State ---
		private StarSystem? _selectedSystem = null;
		private List<StarSystem> _selectedSystemsList = new List<StarSystem>();
		private StarSystem? _potentialClickTarget = null;
		private StarSystem? _hoveredSystem = null;

		// --- UI Control References (Set in Constructor) ---
		private Panel? _selectedSystemInfoPanel = null;
		private Label? _systemNameLabel = null;
		private PictureBox? _subwayMapPictureBox = null;
		private Button? _enterSystemButton = null;

		// --- Drawing Resources ---
		private Pen _jumpLinkPen = new Pen(Color.FromArgb(100, 60, 60, 200), 1);
		private Pen _jumpLinkHighlightPen = new Pen(Color.FromArgb(200, 100, 100, 255), 1.5f);
		private Brush _starBrush = Brushes.WhiteSmoke;
		private Pen _selectionPen = new Pen(Color.Cyan, 1.5f);
		private Pen _multiSelectionPen = new Pen(Color.LimeGreen, 1.5f);
		private Pen _hoverPen = new Pen(Color.Yellow, 1f);
		private Pen _selectionBoxPen = new Pen(Color.White, 1f) { DashStyle = DashStyle.Dash };
		private Font _starNameFont = new Font("Consolas", 7f);
		private Brush _starNameBrush = Brushes.LightGray;
		private Font _debugFont = new Font("Consolas", 8f);
		// Tooltip resources
		private Font _tooltipFont = new Font("Segoe UI", 9f, FontStyle.Regular);
		private Brush _tooltipTextBrush = Brushes.White;
		private Brush _tooltipBackgroundBrush = new SolidBrush(Color.FromArgb(200, 40, 40, 40));
		private Pen _tooltipBorderPen = new Pen(Color.FromArgb(220, 80, 80, 80), 1f);
		// Subway Map resources
		private Font _subwayMapFont = new Font("Segoe UI", 7f);
		private Brush _subwayMapTextBrush = Brushes.White;
		private Pen _subwayMapLinkPen = new Pen(Color.Gray, 1f);
		private Brush _subwayMapNodeBrush = Brushes.DodgerBlue;
		private Brush _subwayMapOriginBrush = Brushes.Orange; // Highlight origin system
		private float _subwayMapNodeRadius = 3f;

		// --- Pre-calculated Data (for performance) ---
		private Dictionary<StarSystem, Vector2> _systemScreenPositions = new Dictionary<StarSystem, Vector2>();

		// --- Optimization ---
		private HashSet<string> _drawnLinks = new HashSet<string>();


		public GalaxyMap(Core.Galaxy.Galaxy galaxy)
		{
			InitializeComponent();
			_galaxy = galaxy ?? throw new ArgumentNullException(nameof(galaxy));


			// When we click on a system, the info panel appears in the top left of the screen. When we click off it, the panel disappears
			_selectedSystemInfoPanel = this.Controls.Find("selectedSystemInfoPanel", true).FirstOrDefault() as Panel;
			if (_selectedSystemInfoPanel != null)
			{
				_selectedSystemInfoPanel.Visible = false;
				_systemNameLabel = _selectedSystemInfoPanel.Controls.Find("systemNameLabel", true).FirstOrDefault() as Label;
				_subwayMapPictureBox = _selectedSystemInfoPanel.Controls.Find("subwayMapPictureBox", true).FirstOrDefault() as PictureBox;
				_enterSystemButton = _selectedSystemInfoPanel.Controls.Find("enterSystemButton", true).FirstOrDefault() as Button;
			}



			// --- Setup galaxyPictureBox ---
			if (this.Controls.Find("galaxyPictureBox", true).FirstOrDefault() is PictureBox pb)
			{
				pb.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?.SetValue(pb, true, null);
				pb.Paint += GalaxyPictureBox_Paint;
				pb.MouseDown += GalaxyPictureBox_MouseDown;
				pb.MouseMove += GalaxyPictureBox_MouseMove;
				pb.MouseUp += GalaxyPictureBox_MouseUp;
				pb.MouseWheel += GalaxyPictureBox_MouseWheel;
				pb.MouseLeave += GalaxyPictureBox_MouseLeave;
				pb.Resize += (sender, e) => pb.Invalidate();
				pb.BackColor = Color.Black;
			}
			else
			{
				MessageBox.Show("Error: galaxyPictureBox not found on the Game form.", "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.Load += (s, e) => this.Close();
			}
		}

		private void GalaxyPictureBox_Paint(object? sender, PaintEventArgs e)
		{

			if (sender is not PictureBox pb) return;
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			g.Clear(pb.BackColor);

			if (pb.ClientSize.Height <= 0) return;
			float aspectRatio = (float)pb.ClientSize.Width / pb.ClientSize.Height;
			Matrix4x4 viewMatrix = Matrix4x4.CreateLookAt(_cameraPosition, _cameraTarget, _cameraUp);
			Matrix4x4 projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(_cameraFov, aspectRatio, _nearPlane, _farPlane);
			Matrix4x4 viewProjectionMatrix = viewMatrix * projectionMatrix;

			_drawnLinks.Clear();
			_systemScreenPositions.Clear();
			HashSet<StarSystem> selectedSet = new HashSet<StarSystem>(_selectedSystemsList);

			// --- Draw Jump Links ---
			if (_galaxy?.StarSystems != null)
			{
				foreach (var system in _galaxy.StarSystems)
				{
					foreach (var jumpSite in system.JumpSites)
					{
						if (jumpSite.HasPartner && jumpSite.Partner != null && jumpSite.Partner.ParentStarSystem != null && jumpSite.Partner.ParentStarSystem != system)
						{
							var partnerSystem = jumpSite.Partner.ParentStarSystem;
							string linkKey = system.Id.CompareTo(partnerSystem.Id) < 0 ? $"{system.Id}_{partnerSystem.Id}" : $"{partnerSystem.Id}_{system.Id}";
							if (_drawnLinks.Add(linkKey))
							{
								Vector2? screenPos1 = WorldToScreen(system.Position, viewProjectionMatrix, pb.ClientSize);
								Vector2? screenPos2 = WorldToScreen(partnerSystem.Position, viewProjectionMatrix, pb.ClientSize);
								if (screenPos1.HasValue && IsValidScreenPoint(screenPos1.Value, pb.ClientSize)) _systemScreenPositions[system] = screenPos1.Value;
								if (screenPos2.HasValue && IsValidScreenPoint(screenPos2.Value, pb.ClientSize)) _systemScreenPositions[partnerSystem] = screenPos2.Value;
								if (screenPos1.HasValue && IsValidScreenPoint(screenPos1.Value, pb.ClientSize, 0) && screenPos2.HasValue && IsValidScreenPoint(screenPos2.Value, pb.ClientSize, 0))
								{
									bool isLinkHighlighted = selectedSet.Contains(system) && selectedSet.Contains(partnerSystem);
									Pen currentLinkPen = isLinkHighlighted ? _jumpLinkHighlightPen : _jumpLinkPen;
									g.DrawLine(currentLinkPen, screenPos1.Value.X, screenPos1.Value.Y, screenPos2.Value.X, screenPos2.Value.Y);
								}
							}
						}
					}
				}
			}


			// --- Draw Star Systems ---
			if (_galaxy?.StarSystems != null)
			{
				foreach (var system in _galaxy.StarSystems)
				{
					if (!_systemScreenPositions.TryGetValue(system, out Vector2 screenPosValue))
					{
						Vector2? screenPos = WorldToScreen(system.Position, viewProjectionMatrix, pb.ClientSize);
						if (screenPos.HasValue && IsValidScreenPoint(screenPos.Value, pb.ClientSize, 50))
						{
							screenPosValue = screenPos.Value;
							_systemScreenPositions[system] = screenPosValue;
						}
						else continue;
					}
					float baseStarDrawSize = 3f;
					float hoverSizeIncrease = 1.5f;
					float starDrawSize = (system == _hoveredSystem) ? baseStarDrawSize + hoverSizeIncrease : baseStarDrawSize;
					float highlightSize = baseStarDrawSize + 4f;
					Pen? currentHighlightPen = null;
					if (selectedSet.Contains(system)) { currentHighlightPen = _multiSelectionPen; }
					else if (system == _selectedSystem) { currentHighlightPen = _selectionPen; }
					if (currentHighlightPen != null) { g.DrawEllipse(currentHighlightPen, screenPosValue.X - highlightSize / 2, screenPosValue.Y - highlightSize / 2, highlightSize, highlightSize); }
					g.FillEllipse(_starBrush, screenPosValue.X - starDrawSize / 2, screenPosValue.Y - starDrawSize / 2, starDrawSize, starDrawSize);
				}
			}


			// --- Box select systems, mouse over systems for their name ---
			if (_isBoxSelecting) { g.DrawRectangle(_selectionBoxPen, _selectionRectCurrent); }
			if (!_isBoxSelecting && _hoveredSystem != null && _systemScreenPositions.TryGetValue(_hoveredSystem, out Vector2 hoveredPos))
			{
				string text = _hoveredSystem.Name;
				SizeF textSize = g.MeasureString(text, _tooltipFont);
				float padding = 4f;
				float tooltipX = hoveredPos.X + 8f; float tooltipY = hoveredPos.Y - textSize.Height - 8f;
				if (tooltipX + textSize.Width + 2 * padding > pb.ClientSize.Width) tooltipX = pb.ClientSize.Width - textSize.Width - 2 * padding;
				if (tooltipY < 0) tooltipY = hoveredPos.Y + 8f; if (tooltipX < 0) tooltipX = 0;
				RectangleF bgRect = new RectangleF(tooltipX, tooltipY, textSize.Width + 2 * padding, textSize.Height + 2 * padding);
				PointF textPos = new PointF(tooltipX + padding, tooltipY + padding);
				g.FillRectangle(_tooltipBackgroundBrush, bgRect); g.DrawRectangle(_tooltipBorderPen, bgRect.X, bgRect.Y, bgRect.Width, bgRect.Height);
				g.DrawString(text, _tooltipFont, _tooltipTextBrush, textPos);
			}
		}

		/// <summary>
		/// Updates the information panel based on the currently selected system.
		/// </summary>
		private void UpdateInfoPanel()
		{
			if (_selectedSystemInfoPanel == null) return;
			if (_selectedSystem == null)
			{
				_selectedSystemInfoPanel.Visible = false;
				if (_enterSystemButton != null) _enterSystemButton.Enabled = false;
				if (_subwayMapPictureBox != null) _subwayMapPictureBox.Image = null;
			}
			else
			{
				// when the lmb goes down on a star's location, it becomes the _selectedSystem 
				_selectedSystemInfoPanel.Visible = true;
				if (_systemNameLabel != null) _systemNameLabel.Text = _selectedSystem.Name;
				if (_enterSystemButton != null) _enterSystemButton.Enabled = true; _enterSystemButton.Visible = true;
				try // subway map
				{
					int maxJumps = 3; 
					var systemsByDistance = GameFunctions.BreadthFirstFromOriginSystem(_selectedSystem, maxJumps);
					DrawSubwayMap(systemsByDistance, maxJumps);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error generating subway map: {ex.Message}");
					if (_subwayMapPictureBox != null) _subwayMapPictureBox.Image = null;
				}
			}
		}

		/// <summary>
		/// Draws the "subway map" of nearby systems onto the subwayMapPictureBox.
		/// </summary>
		private void DrawSubwayMap(Dictionary<StarSystem, int> systemsByDistance, int maxJumps)
		{
			if (_subwayMapPictureBox == null || _subwayMapPictureBox.Width <= 0 || _subwayMapPictureBox.Height <= 0 || _selectedSystem == null)
			{
				return;
			}
			Bitmap mapBitmap = new Bitmap(_subwayMapPictureBox.Width, _subwayMapPictureBox.Height);
			using (Graphics g = Graphics.FromImage(mapBitmap))
			{
				// tbd
			}
			Image? oldImage = _subwayMapPictureBox.Image;
			_subwayMapPictureBox.Image = mapBitmap;
			oldImage?.Dispose();
		}


		/// <summary>
		/// Handles clicking the "Enter System" button in the info panel.
		/// </summary>
		private void EnterSystemButton_Click(object? sender, EventArgs e)
		{
			if (_selectedSystem != null)
			{
				// tbd: Check if a view for this system is already open, and if so bring it to foreground
				// For now, just open a new one.
				try
				{
					SystemViewForm systemView = new SystemViewForm(_selectedSystem);
					systemView.Show(this);
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Error opening system view: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void GalaxyPictureBox_MouseDown(object? sender, MouseEventArgs e)
		{
			_lastMousePosition = e.Location; _mouseDownPosition = e.Location; _potentialClickTarget = null;
			_isRotating = false; _isPanning = false; _isBoxSelecting = false;
			if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.Shift)
			{ // box select
				_isBoxSelecting = true; _selectionRectStartPoint = e.Location; _selectionRectCurrent = new Rectangle(e.Location, Size.Empty);
				_selectedSystemsList.Clear(); _selectedSystem = null; if (_hoveredSystem != null) { _hoveredSystem = null; }
				(sender as Control)!.Capture = true; (sender as Control)!.Cursor = Cursors.Cross; (sender as PictureBox)?.Invalidate();
			}
			else if (e.Button == MouseButtons.Left)
			{ // lmb click on a star
				_potentialClickTarget = FindStarSystemAtScreenPoint(e.Location); _isRotating = true;
				(sender as Control)!.Capture = true; (sender as Control)!.Cursor = Cursors.NoMove2D;
			}
			else if (e.Button == MouseButtons.Right)
			{ // rmb map panning
				_isPanning = true; (sender as Control)!.Capture = true; (sender as Control)!.Cursor = Cursors.SizeAll;
			}
		}

		private void GalaxyPictureBox_MouseMove(object? sender, MouseEventArgs e)
		{
			Point currentMousePosition = e.Location; float deltaX = currentMousePosition.X - _lastMousePosition.X; float deltaY = currentMousePosition.Y - _lastMousePosition.Y;
			bool needsRedraw = false; StarSystem? currentlyHovered = null;
			if (_isBoxSelecting) { _selectionRectCurrent = NormalizeRectangle(_selectionRectStartPoint, currentMousePosition); needsRedraw = true; }
			else if (_isRotating) { needsRedraw = true; _lastMousePosition = currentMousePosition; }
			else if (_isPanning) { needsRedraw = true; _lastMousePosition = currentMousePosition; }
			else { currentlyHovered = FindStarSystemAtScreenPoint(currentMousePosition); if (currentlyHovered != _hoveredSystem) { _hoveredSystem = currentlyHovered; needsRedraw = true; } _lastMousePosition = currentMousePosition; }
			if (needsRedraw && sender is PictureBox pb) { pb.Invalidate(); }
		}

		private void GalaxyPictureBox_MouseUp(object? sender, MouseEventArgs e)
		{
			bool wasRotating = _isRotating; bool wasPanning = _isPanning; bool wasBoxSelecting = _isBoxSelecting;
			_isRotating = false; _isPanning = false; _isBoxSelecting = false;
			bool needsRedraw = false;

			if (wasBoxSelecting && e.Button == MouseButtons.Left)
			{ // box selection finished
				_selectionRectCurrent = NormalizeRectangle(_selectionRectStartPoint, e.Location);
				_selectedSystemsList.Clear();
				foreach (var kvp in _systemScreenPositions) { Point systemPoint = new Point((int)kvp.Value.X, (int)kvp.Value.Y); if (_selectionRectCurrent.Contains(systemPoint)) { _selectedSystemsList.Add(kvp.Key); } }
				_selectedSystem = null; _selectionRectCurrent = Rectangle.Empty;
				needsRedraw = true;
				UpdateInfoPanel(); // Update panel based on multi-selection (currently just hides it)
			}
			else if (wasRotating && e.Button == MouseButtons.Left)
			{
				// Finalize Rotation / Handle Click
				int deltaX = e.X - _mouseDownPosition.X; int deltaY = e.Y - _mouseDownPosition.Y;
				bool isClick = (deltaX * deltaX + deltaY * deltaY) < MaxClickDragDistance;
				if (isClick && _potentialClickTarget != null)
				{
					// Single click select
					_selectedSystem = _potentialClickTarget;
					_cameraTarget = _selectedSystem.Position;
					_selectedSystemsList.Clear();
					needsRedraw = true;
					UpdateInfoPanel(); // Show/Update panel for single selection
				}
				else if (isClick && _potentialClickTarget == null)
				{
					// Single click deselect
					if (_selectedSystem != null || _selectedSystemsList.Count > 0)
					{
						_selectedSystem = null; _selectedSystemsList.Clear();
						needsRedraw = true;
						UpdateInfoPanel(); // Hide panel
					}
				}
			}

			if ((wasRotating || wasPanning || wasBoxSelecting) && sender is Control control) { control.Capture = false; control.Cursor = Cursors.Default; }
			_potentialClickTarget = null;
			if (needsRedraw && sender is PictureBox pb) { pb.Invalidate(); }
		}

		private void GalaxyPictureBox_MouseWheel(object? sender, MouseEventArgs e)
		{
			if (sender is not PictureBox pb) return; pb.Focus(); float zoomFactor = 1.15f; float delta = e.Delta > 0 ? 1.0f / zoomFactor : zoomFactor;
			Vector3 direction = _cameraTarget - _cameraPosition; float distance = direction.Length(); float minDistance = 5.0f; float maxDistance = 15000.0f;
			float newDistance = Math.Clamp(distance * delta, minDistance, maxDistance);
			if (Math.Abs(newDistance - distance) > 0.01f) { _cameraPosition = _cameraTarget - (Vector3.Normalize(direction) * newDistance); pb.Invalidate(); }
		}

		private void GalaxyPictureBox_MouseLeave(object? sender, EventArgs e)
		{
			if (_hoveredSystem != null) { _hoveredSystem = null; (sender as PictureBox)?.Invalidate(); }
		}

		// --- Coordinate Transformation Helpers ---
		private Vector2? WorldToScreen(Vector3 worldPos, Matrix4x4 viewProjectionMatrix, Size clientSize)
		{
			Vector4 clipPos = Vector4.Transform(worldPos, viewProjectionMatrix); if (clipPos.W <= 0.0001f) return null; Vector3 ndcPos = new Vector3(clipPos.X / clipPos.W, clipPos.Y / clipPos.W, clipPos.Z / clipPos.W);
			float screenX = (ndcPos.X + 1.0f) / 2.0f * clientSize.Width; float screenY = (1.0f - ndcPos.Y) / 2.0f * clientSize.Height; return new Vector2(screenX, screenY);
		}
		private bool IsValidScreenPoint(Vector2 point, Size clientSize, float margin = 0)
		{
			if (!float.IsFinite(point.X) || !float.IsFinite(point.Y)) return false; float minX = -margin; float minY = -margin; float maxX = clientSize.Width + margin; float maxY = clientSize.Height + margin; return point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY;
		}
		private StarSystem? FindStarSystemAtScreenPoint(Point screenPoint, float maxDistance = 10f)
		{
			StarSystem? closestSystem = null; float minDistanceSq = maxDistance * maxDistance; foreach (var kvp in _systemScreenPositions) { StarSystem system = kvp.Key; Vector2 systemScreenPos = kvp.Value; float dx = screenPoint.X - systemScreenPos.X; float dy = screenPoint.Y - systemScreenPos.Y; float distanceSq = dx * dx + dy * dy; if (distanceSq < minDistanceSq) { minDistanceSq = distanceSq; closestSystem = system; } }
			return closestSystem;
		}
		private Rectangle NormalizeRectangle(Point p1, Point p2)
		{
			int x = Math.Min(p1.X, p2.X); int y = Math.Min(p1.Y, p2.Y); int width = Math.Abs(p1.X - p2.X); int height = Math.Abs(p1.Y - p2.Y); return new Rectangle(x, y, width, height);
		}

	}
}
