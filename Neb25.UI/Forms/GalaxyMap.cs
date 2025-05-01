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
		private readonly Galaxy _galaxy;

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

		// --- UI Control References ---
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

		/// <summary>
		/// Constructor for the GalaxyMap form.
		/// </summary>
		/// <param name="galaxy">The Galaxy object which is this 'game save' containing the player and NPCs and everything, created initially by the NewGameOptions.</param>
		public GalaxyMap(Galaxy galaxy)
		{
			InitializeComponent();
			_galaxy = galaxy ?? throw new ArgumentNullException(nameof(galaxy));


			// When we click on a system, the info panel appears in the top left of the screen. When we click off it, the panel disappears
			_selectedSystemInfoPanel = this.Controls.Find("selectedSystemInfoPanel", true).FirstOrDefault() as Panel;
			if (_selectedSystemInfoPanel != null)
			{
				_selectedSystemInfoPanel.Visible = false; // Start hidden
				_systemNameLabel = _selectedSystemInfoPanel.Controls.Find("systemNameLabel", true).FirstOrDefault() as Label;
				_subwayMapPictureBox = _selectedSystemInfoPanel.Controls.Find("subwayMapPictureBox", true).FirstOrDefault() as PictureBox;
				_enterSystemButton = _selectedSystemInfoPanel.Controls.Find("enterSystemButton", true).FirstOrDefault() as Button;

				// Attach Paint event handler for the subway map PictureBox if found
				if (_subwayMapPictureBox != null)
				{
					_subwayMapPictureBox.Paint += SubwayMapPictureBox_Paint; // Use a separate paint method if needed, or draw directly onto its Image
				}
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
								if (screenPos1.HasValue) _systemScreenPositions[system] = screenPos1.Value;
								if (screenPos2.HasValue) _systemScreenPositions[partnerSystem] = screenPos2.Value; // show the jump lane even when origin and partner are offscreen, to a certain number of pixels
								int howFarAwayToStillShowJumpLanes = 1000; // pixels offscreen
								if (screenPos1.HasValue && IsValidScreenPoint(screenPos1.Value, pb.ClientSize, howFarAwayToStillShowJumpLanes) &&
									screenPos2.HasValue && IsValidScreenPoint(screenPos2.Value, pb.ClientSize, howFarAwayToStillShowJumpLanes))
								{
									bool isLinkHighlighted = selectedSet.Contains(system) || selectedSet.Contains(partnerSystem) || system == _selectedSystem;
									Pen currentLinkPen = isLinkHighlighted ? _jumpLinkHighlightPen : _jumpLinkPen;
									try
									{
										g.DrawLine(currentLinkPen, screenPos1.Value.X, screenPos1.Value.Y, screenPos2.Value.X, screenPos2.Value.Y);
									}
									catch (OverflowException oe)
									{
										// Catch potential overflow if coordinates are still too large for GDI+
										Console.WriteLine($"GDI Overflow drawing line between {system.Name} and {partnerSystem.Name}. Pos1: {screenPos1}, Pos2: {screenPos2}. Error: {oe.Message}");
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
						// Determine star size and highlight status
						float baseStarDrawSize = 3f;
						float hoverSizeIncrease = 1.5f;
						float starDrawSize = (system == _hoveredSystem) ? baseStarDrawSize + hoverSizeIncrease : baseStarDrawSize;
						float highlightSize = baseStarDrawSize + 4f; // Size of the selection circle
																	 // Determine which highlight pen to use (if any)
						Pen? currentHighlightPen = null;
						if (selectedSet.Contains(system)) { currentHighlightPen = _multiSelectionPen; } // Multi-select highlight
						else if (system == _selectedSystem) { currentHighlightPen = _selectionPen; }   // Single-select highlight
																									   // Draw highlight circle if selected
						if (currentHighlightPen != null)
						{
							g.DrawEllipse(currentHighlightPen, screenPosValue.X - highlightSize / 2, screenPosValue.Y - highlightSize / 2, highlightSize, highlightSize);
						}
						// Draw the star itself
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
		}

		/// <summary>
		/// Updates the information panel based on the currently selected system.
		/// </summary>
		private void UpdateInfoPanel()
		{
			if (_selectedSystemInfoPanel == null) return;
			// Hide panel if no system or multiple systems are selected
			if (_selectedSystem == null || _selectedSystemsList.Count > 1)
			{
				_selectedSystemInfoPanel.Visible = false;
				if (_enterSystemButton != null) _enterSystemButton.Enabled = false;
				// Clear the subway map image when panel is hidden
				if (_subwayMapPictureBox != null && _subwayMapPictureBox.Image != null)
				{
					var oldImage = _subwayMapPictureBox.Image;
					_subwayMapPictureBox.Image = null;
					oldImage?.Dispose();
				}
			}
			else
			{
				// when the lmb goes down on a star's location, it becomes the _selectedSystem 
				_selectedSystemInfoPanel.Visible = true;
				if (_systemNameLabel != null) _systemNameLabel.Text = _selectedSystem.Name; _systemNameLabel.Visible = true;
				if (_enterSystemButton != null) { _enterSystemButton.Enabled = true; _enterSystemButton.Visible = true; }
				try // subway map
				{
					int maxJumps = 2;
					var systemsByDistance = GameFunctions.BreadthFirstFromOriginSystem(_selectedSystem, maxJumps);
					DrawSubwayMap(systemsByDistance, maxJumps);
					if (_subwayMapPictureBox != null) _subwayMapPictureBox.Visible = true;
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error generating subway map: {ex.Message}");
					if (_subwayMapPictureBox != null)
					{
						var oldImage = _subwayMapPictureBox.Image;
						_subwayMapPictureBox.Image = null;
						oldImage?.Dispose();
					}
				}
			}
		}


		/// <summary>
		/// Draws the "subway map" of nearby systems onto the subwayMapPictureBox's Image property.
		/// </summary>
		/// <param name="systemsByDistance">Dictionary mapping reachable systems to their jump distance.</param>
		/// <param name="maxJumps">The maximum jump distance included in the dictionary.</param>
		private void DrawSubwayMap(Dictionary<StarSystem, int> systemsByDistance, int maxJumps)
		{
			if (_subwayMapPictureBox == null || _subwayMapPictureBox.Width <= 0 || _subwayMapPictureBox.Height <= 0 || _selectedSystem == null)
			{
				return;
			}
			Bitmap mapBitmap = new Bitmap(_subwayMapPictureBox.Width, _subwayMapPictureBox.Height);
			using (Graphics g = Graphics.FromImage(mapBitmap))
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.Clear(Color.FromArgb(20, 20, 20)); // Dark background for the map

				// --- Layout Parameters ---
				PointF center = new PointF(_subwayMapPictureBox.Width / 2f, _subwayMapPictureBox.Height / 2f);
				float maxRadius = Math.Min(_subwayMapPictureBox.Width, _subwayMapPictureBox.Height) / 2f * 0.85f; // 85% of half the smallest dimension
				float ringSpacing = (maxJumps > 0) ? maxRadius / maxJumps : maxRadius; // Distance between jump rings

				// Dictionary to store calculated screen positions for each system node
				Dictionary<StarSystem, PointF> nodePositions = new Dictionary<StarSystem, PointF>();

				// --- Position Nodes ---
				// Group systems by their jump distance
				var systemsGroupedByDistance = systemsByDistance
					.Where(kvp => kvp.Key != null) // Ensure system is not null
					.GroupBy(kvp => kvp.Value) // Group by distance (int)
					.OrderBy(g => g.Key);      // Order by distance (0, 1, 2...)

				foreach (var group in systemsGroupedByDistance)
				{
					int distance = group.Key;
					var systemsInRing = group.Select(kvp => kvp.Key).ToList();
					int countInRing = systemsInRing.Count;

					// Calculate radius for this ring
					float radius = distance * ringSpacing;

					// Position nodes on the ring
					for (int i = 0; i < countInRing; i++)
					{
						StarSystem system = systemsInRing[i];
						PointF position;
						if (distance == 0)
						{
							// Place the origin system at the center
							position = center;
						}
						else
						{
							// Distribute other systems evenly around the ring
							float angle = (countInRing > 1) ? (float)(i * 2 * Math.PI / countInRing) : 0; // Angle in radians
																										  // Offset angle slightly for visual appeal if desired (e.g., + MathF.PI / 4)
							float x = center.X + radius * MathF.Cos(angle);
							float y = center.Y + radius * MathF.Sin(angle);
							position = new PointF(x, y);
						}
						nodePositions[system] = position;
					}
				}

				// --- Draw Links ---
				HashSet<string> drawnSubwayLinks = new HashSet<string>(); // Prevent drawing links twice
				foreach (var kvp in nodePositions)
				{
					StarSystem system1 = kvp.Key;
					PointF pos1 = kvp.Value;

					// Find neighbors within the map's scope
					foreach (var jumpSite in system1.JumpSites)
					{
						if (jumpSite.HasPartner && jumpSite.Partner?.ParentStarSystem != null)
						{
							StarSystem system2 = jumpSite.Partner.ParentStarSystem;

							// Check if the neighbor is also on the map and get its position
							if (nodePositions.TryGetValue(system2, out PointF pos2))
							{
								// Create unique link key
								string linkKey = system1.Id.CompareTo(system2.Id) < 0 ? $"{system1.Id}_{system2.Id}" : $"{system2.Id}_{system1.Id}";
								if (drawnSubwayLinks.Add(linkKey))
								{
									// Draw the link
									g.DrawLine(_subwayMapLinkPen, pos1, pos2);
								}
							}
						}
					}
				}

				// --- Draw Nodes (and Labels) ---
				foreach (var kvp in nodePositions)
				{
					StarSystem system = kvp.Key;
					PointF pos = kvp.Value;

					// Choose brush based on whether it's the origin system
					Brush nodeBrush = (system == _selectedSystem) ? _subwayMapOriginBrush : _subwayMapNodeBrush;

					// Draw the node circle
					g.FillEllipse(nodeBrush, pos.X - _subwayMapNodeRadius, pos.Y - _subwayMapNodeRadius, _subwayMapNodeRadius * 2, _subwayMapNodeRadius * 2);

					// Draw system name label (optional, can get cluttered)
					// Adjust text position slightly below the node
					// SizeF textSize = g.MeasureString(system.Name, _subwayMapFont);
					// g.DrawString(system.Name, _subwayMapFont, _subwayMapTextBrush, pos.X - textSize.Width / 2, pos.Y + _subwayMapNodeRadius + 1);
				}
			} // Graphics object is disposed here

			// --- Assign the new Bitmap to the PictureBox ---
			// Dispose the old image first to free memory
			Image? oldImage = _subwayMapPictureBox.Image;
			_subwayMapPictureBox.Image = mapBitmap; // Assign the newly created bitmap
			oldImage?.Dispose(); // Dispose the previous bitmap


		}

		/// <summary>
		/// Update handler for the subwaymap picturebox.
		/// </summary>
		private void SubwayMapPictureBox_Paint(object? sender, PaintEventArgs e)
		{
			// Dynamics, animations
			// Example: Draw a border around the PictureBox contents
			// if (sender is PictureBox spb && spb.Image != null) {
			//     e.Graphics.DrawRectangle(Pens.DimGray, 0, 0, spb.ClientSize.Width - 1, spb.ClientSize.Height - 1);
			// }
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
					SystemView systemView = new SystemView(_selectedSystem);
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
			if (sender is not Control control) return;
			_lastMousePosition = e.Location;
			_mouseDownPosition = e.Location;
			_potentialClickTarget = null;
			_isRotating = false;
			_isPanning = false;
			_isBoxSelecting = false;

			if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.Shift)
			{ // box select
				_isBoxSelecting = true;
				_selectionRectStartPoint = e.Location;
				_selectionRectCurrent = new Rectangle(e.Location, Size.Empty);
				_selectedSystemsList.Clear();
				_selectedSystem = null;
				if (_hoveredSystem != null) { _hoveredSystem = null; }
				control.Capture = true;
				control.Cursor = Cursors.Cross;
				(sender as PictureBox)?.Invalidate();
			}
			else if (e.Button == MouseButtons.Left)
			{ // lmb click on a star
				_potentialClickTarget = FindStarSystemAtScreenPoint(e.Location);
				_isRotating = true;
				control.Capture = true;
				control.Cursor = Cursors.NoMove2D;
			}
			else if (e.Button == MouseButtons.Right)
			{ // rmb map panning
				_isPanning = true;
				control.Capture = true;
				control.Cursor = Cursors.SizeAll;
			}
		}

		private void GalaxyPictureBox_MouseMove(object? sender, MouseEventArgs e)
		{
			Point currentMousePosition = e.Location;
			float deltaX = currentMousePosition.X - _lastMousePosition.X;
			float deltaY = currentMousePosition.Y - _lastMousePosition.Y;
			bool needsRedraw = false;
			StarSystem? currentlyHovered = null;

			if (_isBoxSelecting)
			{
				_selectionRectCurrent = NormalizeRectangle(_selectionRectStartPoint, currentMousePosition);
				needsRedraw = true;
			}
			else if (_isRotating)
			{
				// --- Apply Rotation ---
				if (Math.Abs(deltaX) > 0 || Math.Abs(deltaY) > 0)
				{
					float rotationSpeed = 0.005f; // Adjust sensitivity

					// Calculate rotation angles (yaw around world Y, pitch around camera's right axis)
					float yawAngle = -deltaX * rotationSpeed;
					float pitchAngle = -deltaY * rotationSpeed;

					// Get current camera direction and right vector
					Vector3 direction = _cameraTarget - _cameraPosition;
					Vector3 right = Vector3.Normalize(Vector3.Cross(direction, _cameraUp));
					// Recalculate Up vector orthogonal to new Right and Direction
					Vector3 localUp = Vector3.Normalize(Vector3.Cross(right, direction));

					// Create rotation matrices
					Matrix4x4 yaw = Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, yawAngle); // Rotate around world Y
					Matrix4x4 pitch = Matrix4x4.CreateFromAxisAngle(right, pitchAngle); // Rotate around camera's right axis

					// Apply rotations to the camera position relative to the target
					direction = Vector3.Transform(direction, yaw * pitch);

					// Prevent camera flipping over the top/bottom
					// Calculate the angle between the new direction and the world Up vector
					float dot = Vector3.Dot(Vector3.Normalize(direction), Vector3.UnitY);
					// Allow rotation up to ~5 degrees from vertical poles
					if (Math.Abs(dot) < 0.995f) // cos(5 degrees) approx 0.996, use slightly less
					{
						_cameraPosition = _cameraTarget - direction;
						// Update the camera's Up vector to stay orthogonal
						_cameraUp = localUp;
					}
					else
					{
						// If limit is reached, only apply yaw
						direction = _cameraTarget - _cameraPosition; // Reset direction
						direction = Vector3.Transform(direction, yaw);
						_cameraPosition = _cameraTarget - direction;
						// Update the camera's Up vector based only on yaw
						right = Vector3.Normalize(Vector3.Cross(direction, Vector3.UnitY)); // Use world Y temporarily
						_cameraUp = Vector3.Normalize(Vector3.Cross(right, direction));
					}


					needsRedraw = true;
				}
			}
			else if (_isPanning)
			{
				// --- Apply Panning ---
				if (Math.Abs(deltaX) > 0 || Math.Abs(deltaY) > 0)
				{
					// Calculate pan speed based on distance (pan faster when zoomed out)
					float distance = (_cameraPosition - _cameraTarget).Length();
					float panSpeed = distance * 0.001f; // Adjust sensitivity

					// Get camera's right and up vectors
					Vector3 direction = Vector3.Normalize(_cameraTarget - _cameraPosition);
					Vector3 right = Vector3.Normalize(Vector3.Cross(direction, _cameraUp));
					// Ensure Up is orthogonal after potential rotations
					Vector3 localUp = Vector3.Normalize(Vector3.Cross(right, direction));


					// Calculate movement vector
					Vector3 moveX = right * -deltaX * panSpeed;
					Vector3 moveY = localUp * deltaY * panSpeed; // Positive Y is usually up in world space

					// Update camera position and target
					_cameraPosition += moveX + moveY;
					_cameraTarget += moveX + moveY;

					needsRedraw = true;
				}
			}
			else // Not dragging, just moving the mouse
			{
				// Find system under cursor for hover effect/tooltip
				currentlyHovered = FindStarSystemAtScreenPoint(currentMousePosition);
				if (currentlyHovered != _hoveredSystem)
				{
					_hoveredSystem = currentlyHovered;
					needsRedraw = true; // Need redraw to show/hide hover/tooltip
				}
			}
			_lastMousePosition = currentMousePosition; // Update last position for next frame

			// Trigger redraw if needed
			if (needsRedraw && sender is PictureBox pb)
			{
				pb.Invalidate();
			}
		}

		private void GalaxyPictureBox_MouseUp(object? sender, MouseEventArgs e)
		{
			bool wasRotating = _isRotating;
			bool wasPanning = _isPanning;
			bool wasBoxSelecting = _isBoxSelecting;
			_isRotating = false;
			_isPanning = false;
			_isBoxSelecting = false;
			bool needsRedraw = false;

			if (wasBoxSelecting && e.Button == MouseButtons.Left)
			{ // box selection finished
				_selectionRectCurrent = NormalizeRectangle(_selectionRectStartPoint, e.Location);
				_selectedSystemsList.Clear();
				foreach (var kvp in _systemScreenPositions)
				{
					Point systemPoint = new Point((int)kvp.Value.X, (int)kvp.Value.Y);
					if (_selectionRectCurrent.Contains(systemPoint)) _selectedSystemsList.Add(kvp.Key);
				}

				_selectedSystem = null;
				if (_selectedSystemsList.Count ==1) { _selectedSystem = _selectedSystemsList[0]; }
				_selectionRectCurrent = Rectangle.Empty;
				needsRedraw = true;
				UpdateInfoPanel(); // Update panel (will hide it for multi-select)
			}
			else if (wasRotating && e.Button == MouseButtons.Left)
			{
				// Finalize Rotation / Handle Click
				int deltaX = e.X - _mouseDownPosition.X;
				int deltaY = e.Y - _mouseDownPosition.Y;
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
						_selectedSystem = null;
						_selectedSystemsList.Clear();
						needsRedraw = true;
						UpdateInfoPanel(); // Hide panel
					}
				}
			}

			// --- Reset Cursor and Capture ---
			if ((wasRotating || wasPanning || wasBoxSelecting) && sender is Control control)
			{
				control.Capture = false; // Release mouse capture
				control.Cursor = Cursors.Default; // Reset cursor
			}

			// Clear the potential target identified on MouseDown
			_potentialClickTarget = null;

			// Redraw if necessary (selection changed, box select ended)
			if (needsRedraw && sender is PictureBox pb)
			{
				pb.Invalidate();
			}
		}

		private void GalaxyPictureBox_MouseWheel(object? sender, MouseEventArgs e)
		{
			if (sender is not PictureBox pb) return;
			pb.Focus();
			float zoomFactor = 1.15f;
			float delta = e.Delta > 0 ? 1.0f / zoomFactor : zoomFactor;
			Vector3 direction = _cameraTarget - _cameraPosition;
			float distance = direction.Length();
			float minDistance = 5.0f;
			float maxDistance = 15000.0f;
			float newDistance = Math.Clamp(distance * delta, minDistance, maxDistance);
			if (Math.Abs(newDistance - distance) > 0.01f)
			{
				_cameraPosition = _cameraTarget - (Vector3.Normalize(direction) * newDistance);
				pb.Invalidate();
			}
		}

		private void GalaxyPictureBox_MouseLeave(object? sender, EventArgs e)
		{
			if (_hoveredSystem != null) { _hoveredSystem = null; (sender as PictureBox)?.Invalidate(); }
		}

		// --- Coordinate Transformation Helpers ---
		/// <summary>
		/// Projects a 3D world position to 2D screen coordinates.
		/// </summary>
		/// <param name="worldPos">The 3D world position.</param>
		/// <param name="viewProjectionMatrix">The combined view and projection matrix.</param>
		/// <param name="clientSize">The size of the PictureBox client area.</param>
		/// <returns>A 2D screen coordinate Vector2, or null if behind the camera.</returns>
		private Vector2? WorldToScreen(Vector3 worldPos, Matrix4x4 viewProjectionMatrix, Size clientSize)
		{
			// Transform world position to clip space
			Vector4 clipPos = Vector4.Transform(worldPos, viewProjectionMatrix);

			// If W is zero or negative, the point is behind or on the near plane clipping boundary
			if (clipPos.W <= 0.0001f) return null; // Avoid division by zero/invalid projection

			// Perform perspective divide to get Normalized Device Coordinates (NDC) [-1, 1]
			Vector3 ndcPos = new Vector3(clipPos.X / clipPos.W, clipPos.Y / clipPos.W, clipPos.Z / clipPos.W);

			// Convert NDC to screen coordinates [0, Width], [0, Height]
			// NDC X: -1 (left) to +1 (right) -> Screen X: 0 to Width
			// NDC Y: -1 (bottom) to +1 (top) -> Screen Y: Height to 0 (Y is inverted)
			float screenX = (ndcPos.X + 1.0f) / 2.0f * clientSize.Width;
			float screenY = (1.0f - ndcPos.Y) / 2.0f * clientSize.Height; // Invert Y for screen space

			return new Vector2(screenX, screenY);
		}
		/// <summary>
		/// Checks if a 2D screen point is within the client bounds, optionally with a margin.
		/// Also checks for NaN or Infinity values.
		/// </summary>
		/// <param name="point">The 2D screen point.</param>
		/// <param name="clientSize">The size of the client area.</param>
		/// <param name="margin">Optional margin outside the client bounds to consider valid.</param>
		/// <returns>True if the point is valid and within bounds (plus margin), false otherwise.</returns>
		private bool IsValidScreenPoint(Vector2 point, Size clientSize, float margin = 0)
		{
			// Check for invalid float values first
			if (!float.IsFinite(point.X) || !float.IsFinite(point.Y)) return false;

			// Define boundaries including margin
			float minX = -margin;
			float minY = -margin;
			float maxX = clientSize.Width + margin;
			float maxY = clientSize.Height + margin;

			// Check if point is within the boundaries
			return point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY;
		}

		/// <summary>
		/// Finds the star system closest to a given screen point, within a maximum distance.
		/// Uses the pre-calculated screen positions from the last Paint event.
		/// </summary>
		/// <param name="screenPoint">The screen point (e.g., mouse cursor position).</param>
		/// <param name="maxDistance">The maximum screen distance (radius) to search within.</param>
		/// <returns>The closest StarSystem within the radius, or null if none found.</returns>
		private StarSystem? FindStarSystemAtScreenPoint(Point screenPoint, float maxDistance = 10f)
		{
			StarSystem? closestSystem = null;
			float minDistanceSq = maxDistance * maxDistance; // Use squared distance for efficiency

			// Iterate through the cached screen positions
			foreach (var kvp in _systemScreenPositions)
			{
				StarSystem system = kvp.Key;
				Vector2 systemScreenPos = kvp.Value;

				// Calculate squared distance from the click point to the system's screen position
				float dx = screenPoint.X - systemScreenPos.X;
				float dy = screenPoint.Y - systemScreenPos.Y;
				float distanceSq = dx * dx + dy * dy;

				// If this system is closer than the current closest, update
				if (distanceSq < minDistanceSq)
				{
					minDistanceSq = distanceSq;
					closestSystem = system;
				}
			}
			return closestSystem;
		}
		/// <summary>
		/// Normalizes a rectangle defined by two corner points.
		/// Ensures the X, Y coordinates are the top-left corner and Width, Height are positive.
		/// </summary>
		/// <param name="p1">The first corner point (e.g., mouse down position).</param>
		/// <param name="p2">The second corner point (e.g., current mouse position).</param>
		/// <returns>A normalized Rectangle.</returns>
		private Rectangle NormalizeRectangle(Point p1, Point p2)
		{
			int x = Math.Min(p1.X, p2.X);
			int y = Math.Min(p1.Y, p2.Y);
			int width = Math.Abs(p1.X - p2.X);
			int height = Math.Abs(p1.Y - p2.Y);
			return new Rectangle(x, y, width, height);
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Dispose managed resources (like Pens, Brushes, Fonts)
				_jumpLinkPen?.Dispose();
				_jumpLinkHighlightPen?.Dispose();
				_starBrush?.Dispose(); // Note: Standard brushes like Brushes.White don't need disposal
				_selectionPen?.Dispose();
				_multiSelectionPen?.Dispose();
				_hoverPen?.Dispose();
				_selectionBoxPen?.Dispose();
				_starNameFont?.Dispose();
				_starNameBrush?.Dispose(); // Note: Standard brushes like Brushes.LightGray don't need disposal
				_debugFont?.Dispose();
				_tooltipFont?.Dispose();
				_tooltipTextBrush?.Dispose(); // Note: Standard brushes like Brushes.White don't need disposal
				_tooltipBackgroundBrush?.Dispose();
				_tooltipBorderPen?.Dispose();
				_subwayMapFont?.Dispose();
				_subwayMapTextBrush?.Dispose(); // Note: Standard brushes like Brushes.White don't need disposal
				_subwayMapLinkPen?.Dispose();
				_subwayMapNodeBrush?.Dispose(); // Note: Standard brushes like Brushes.DodgerBlue don't need disposal
				_subwayMapOriginBrush?.Dispose(); // Note: Standard brushes like Brushes.Orange don't need disposal

				// Dispose the current subway map image if it exists
				if (_subwayMapPictureBox?.Image != null)
				{
					_subwayMapPictureBox.Image.Dispose();
					_subwayMapPictureBox.Image = null;
				}


				// Dispose components added by the designer
				components?.Dispose();
			}
			base.Dispose(disposing);
		}


	}
}