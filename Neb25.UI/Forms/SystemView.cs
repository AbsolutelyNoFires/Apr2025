using Neb25.Core.Galaxy;
using System.Drawing.Drawing2D; // For SmoothingMode
using System.Windows.Forms;
using System.Numerics; // For Vector2 (if Planets/JumpSites use it) - Add if needed

namespace Neb25.UI.Forms
{
	public partial class SystemView : Form
	{
		private StarSystem _system;
		private Point dragStartPoint;
		private bool isDragging = false;
		private PointF viewOffset = PointF.Empty; // Offset for panning
		private float zoomFactor = 1.0f; // Start zoom factor (Adjust as needed)

		// --- Increased Scale ---
		// How many pixels represent one unit of game distance (e.g., pixels per million km or AU)
		// Increased significantly from 0.5f to make orbits visible. Adjust further if needed.
		private const float PIXELS_PER_DISTANCE_UNIT = 100f; // Example: 100 pixels per AU

		// Store screen positions calculated during Paint event for hit detection (Mouse Click)
		private Dictionary<object, RectangleF> _celestialObjectScreenBounds = new Dictionary<object, RectangleF>();
		private object? _selectedObject = null; // Store the currently selected object (Star, Planet, JumpSite)


		public SystemView(StarSystem passedStarSystem)
		{
			InitializeComponent();
			_system = passedStarSystem ?? throw new ArgumentNullException(nameof(passedStarSystem)); // Ensure system is not null

			// --- Form Setup ---
			this.Text = $"System View: {_system.Name}"; // Set form title
			lblSystemName.Text = _system.Name; // Set label text

			// --- Enable Double Buffering for smoother drawing ---
			// Set styles directly for potentially better performance than this.DoubleBuffered = true;
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
						  ControlStyles.UserPaint |
						  ControlStyles.AllPaintingInWmPaint, true);
			this.UpdateStyles();


			// --- Event Handlers ---
			this.systemPictureBox.Paint += SystemView_Paint; // Use PictureBox Paint event
			this.systemPictureBox.Resize += SystemView_Resize;
			this.systemPictureBox.MouseDown += SystemView_MouseDown;
			this.systemPictureBox.MouseMove += SystemView_MouseMove;
			this.systemPictureBox.MouseUp += SystemView_MouseUp;
			this.systemPictureBox.MouseWheel += SystemView_MouseWheel; // Attach MouseWheel to PictureBox

			// Initial drawing setup
			CenterViewOnStar(); // Center the view initially
		}

		// --- Coordinate Transformation ---

		/// <summary>
		/// Converts world coordinates (relative to star system center 0,0) to screen coordinates.
		/// </summary>
		/// <param name="worldPoint">The world point (e.g., planet position relative to star).</param>
		/// <returns>The corresponding screen point within the PictureBox.</returns>
		private PointF WorldToScreen(PointF worldPoint)
		{
			// Apply scaling and zoom
			float scaledX = worldPoint.X * zoomFactor * PIXELS_PER_DISTANCE_UNIT;
			float scaledY = worldPoint.Y * zoomFactor * PIXELS_PER_DISTANCE_UNIT; // Y might need inversion depending on world coords

			// Apply panning offset and center in PictureBox
			float screenX = scaledX + systemPictureBox.ClientSize.Width / 2f + viewOffset.X;
			float screenY = scaledY + systemPictureBox.ClientSize.Height / 2f + viewOffset.Y; // Assuming Y increases downwards on screen

			return new PointF(screenX, screenY);
		}

		/// <summary>
		/// Converts screen coordinates (within the PictureBox) back to world coordinates.
		/// </summary>
		/// <param name="screenPoint">The screen point (e.g., mouse click position).</param>
		/// <returns>The corresponding world point.</returns>
		private PointF ScreenToWorld(Point screenPoint)
		{
			// Remove centering and panning offset
			float centeredX = screenPoint.X - systemPictureBox.ClientSize.Width / 2f - viewOffset.X;
			float centeredY = screenPoint.Y - systemPictureBox.ClientSize.Height / 2f - viewOffset.Y;

			// Remove scaling and zoom (avoid division by zero)
			float scaleDenom = zoomFactor * PIXELS_PER_DISTANCE_UNIT;
			if (Math.Abs(scaleDenom) < 1e-6) scaleDenom = 1e-6f; // Prevent division by zero

			float worldX = centeredX / scaleDenom;
			float worldY = centeredY / scaleDenom; // Y might need inversion depending on world coords

			return new PointF(worldX, worldY);
		}

		/// <summary>
		/// Recalculates the view offset to center the view on the primary star (0,0 world coords).
		/// </summary>
		private void CenterViewOnStar()
		{
			// The star is at world (0,0). We want this to map to the screen center.
			// ScreenCenter = WorldToScreen(0,0)
			// ScreenCenter.X = (0 * zoom * scale) + PicBoxWidth/2 + viewOffset.X  => viewOffset.X = 0
			// ScreenCenter.Y = (0 * zoom * scale) + PicBoxHeight/2 + viewOffset.Y => viewOffset.Y = 0
			// Actually, if we want to center *initially*, we just set the offset to zero.
			// Panning will adjust it later.
			viewOffset = PointF.Empty;
			systemPictureBox.Invalidate(); // Redraw after centering
		}


		// --- Drawing Logic ---

		private void SystemView_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias; // Make drawing smooth
			g.Clear(systemPictureBox.BackColor); // Use PictureBox's back color

			// Clear previous screen bounds before redrawing
			_celestialObjectScreenBounds.Clear();

			if (_system != null)
			{
				// --- Draw Orbits First (so they are behind planets) ---
				DrawOrbits(g);

				// --- Draw Celestial Bodies ---
				DrawStars(g);
				DrawPlanets(g);
				DrawJumpSites(g);
				// Draw Moons (relative to planets) - TODO
				// Draw Ships, Stations, etc. later - TODO

				DrawInfoPanel(g);
			}

			// --- Draw Selection Highlight ---
			DrawSelectionHighlight(g);

			// --- Draw Debug Info (Optional) ---
			// DrawDebugInfo(g);
		}

		private void DrawStars(Graphics g)
		{
			if (_system.Stars == null) return;

			foreach (Star star in _system.Stars)
			{
				// Assuming stars are positioned relative to system center (0,0)
				// For now, treat the first star as the primary at (0,0)
				// TODO: Handle binary/trinary systems with actual positions
				PointF starWorldPos = PointF.Empty; // Assume star is at center (0,0) for now
				if (star != _system.PrimaryStar && _system.Stars.Count > 1)
				{
					// Placeholder for positioning secondary stars if needed
					// starWorldPos = new PointF(some_offset_x, some_offset_y);
				}

				PointF starScreenPos = WorldToScreen(starWorldPos);

				// Basic size and color - TODO: Vary based on star type/size
				float size = Math.Max(10f, 10f * zoomFactor); // Make star size slightly dependent on zoom
				Brush starBrush = Brushes.Yellow; // TODO: Color based on type

				RectangleF bounds = new RectangleF(
					starScreenPos.X - size / 2,
					starScreenPos.Y - size / 2,
					size, size);

				g.FillEllipse(starBrush, bounds);

				// Store bounds for click detection
				_celestialObjectScreenBounds[star] = bounds;

				// Only draw label if zoomed in enough
				if (zoomFactor > 0.5f)
				{
					DrawLabel(g, star.Name, starScreenPos, size);
				}
			}
		}

		private void DrawPlanets(Graphics g)
		{
			if (_system.Planets == null) return;

			foreach (Planet planet in _system.Planets)
			{
				// --- Calculate Planet Position ---
				// Simple static positioning for now: Place planets evenly around orbit
				// TODO: Calculate actual position based on orbital mechanics (Mean Anomaly etc.)
				float angleStep = 360f / _system.Planets.Count;
				float angle = planet.OrbitalIndex * angleStep * (float)(Math.PI / 180.0); // Convert degrees to radians

				// Position relative to the star (world 0,0)
				float planetWorldX = planet.SemiMajorAxis * MathF.Cos(angle);
				float planetWorldY = planet.SemiMajorAxis * MathF.Sin(angle);
				PointF planetWorldPos = new PointF(planetWorldX, planetWorldY);

				// --- Convert to Screen Coordinates ---
				PointF planetScreenPos = WorldToScreen(planetWorldPos);

				// --- Draw Planet ---
				float size = Math.Max(5f, 5f * zoomFactor); // Basic size, slightly zoom-dependent
				Brush planetBrush = Brushes.LightBlue; // TODO: Color/Texture based on type

				RectangleF bounds = new RectangleF(
					planetScreenPos.X - size / 2,
					planetScreenPos.Y - size / 2,
					size, size);

				g.FillEllipse(planetBrush, bounds);

				// Store bounds for click detection
				_celestialObjectScreenBounds[planet] = bounds;

				// Only draw label if zoomed in enough
				if (zoomFactor > 0.3f) // Adjust threshold as needed
				{
					DrawLabel(g, planet.Name, planetScreenPos, size);
				}

				// TODO: Draw Moons orbiting this planet
				// DrawMoons(g, planet, planetScreenPos);
			}
		}

		private void DrawOrbits(Graphics g)
		{
			if (_system.Planets == null) return;

			// Assuming the primary star is at world (0,0)
			PointF centerWorldPos = PointF.Empty;

			foreach (Planet planet in _system.Planets)
			{
				// Use SemiMajorAxis for the orbit radius
				DrawOrbit(g, centerWorldPos, planet.SemiMajorAxis);
			}

			// TODO: Draw orbits for moons around planets
		}


		private void DrawJumpSites(Graphics g)
		{
			if (_system.JumpSites == null) return;

			foreach (JumpSite jumpSite in _system.JumpSites)
			{
				// Use the JumpSite's Position property (relative to system center)
				PointF jumpSiteWorldPos = new PointF(jumpSite.Position.X, jumpSite.Position.Y);
				PointF jumpSiteScreenPos = WorldToScreen(jumpSiteWorldPos);

				float size = Math.Max(6f, 6f * zoomFactor); // Slightly zoom-dependent size
				Brush jumpBrush = jumpSite.HasGateBuilt ? Brushes.Cyan : Brushes.Magenta; // Different color if gate exists

				// Draw as a square/rectangle
				RectangleF bounds = new RectangleF(
				   jumpSiteScreenPos.X - size / 2,
				   jumpSiteScreenPos.Y - size / 2,
				   size, size);

				g.FillRectangle(jumpBrush, bounds);

				// Store bounds for click detection
				_celestialObjectScreenBounds[jumpSite] = bounds;

				// Only draw label if zoomed in enough
				if (zoomFactor > 0.4f) // Adjust threshold
				{
					DrawLabel(g, jumpSite.Name, jumpSiteScreenPos, size);
				}
			}
		}

		private void DrawOrbit(Graphics g, PointF centerWorldPos, float orbitalRadius)
		{
			if (orbitalRadius <= 0) return; // Don't draw zero-radius orbits

			PointF centerScreenPos = WorldToScreen(centerWorldPos);
			// Calculate the screen radius based on the world radius
			// We need the difference between the screen position of (orbitalRadius, 0) and the center screen pos
			PointF edgePointScreen = WorldToScreen(new PointF(centerWorldPos.X + orbitalRadius, centerWorldPos.Y));
			float radiusPixels = Math.Abs(edgePointScreen.X - centerScreenPos.X); // Use the difference in X coords

			// Only draw if the radius is reasonably large on screen
			if (radiusPixels < 2.0f) return;

			RectangleF orbitRect = new RectangleF(
				centerScreenPos.X - radiusPixels,
				centerScreenPos.Y - radiusPixels,
				radiusPixels * 2,
				radiusPixels * 2
			);

			// Use a less obtrusive color and style for orbits
			using (Pen orbitPen = new Pen(Color.FromArgb(100, 100, 100), 1) { DashStyle = DashStyle.Dot }) // Gray, dotted
			{
				try
				{
					g.DrawEllipse(orbitPen, orbitRect);
				}
				catch (OverflowException)
				{
					// Ignore error if orbit is too large to draw
				}
			}
		}

		private void DrawLabel(Graphics g, string text, PointF screenPos, float objectSize)
		{
			if (string.IsNullOrEmpty(text)) return;

			// Simple font, consider making size dependent on zoom?
			using (Font labelFont = new Font("Segoe UI", 7f))
			using (Brush labelBrush = new SolidBrush(Color.FromArgb(200, 200, 200))) // Light gray text
			{
				SizeF textSize = g.MeasureString(text, labelFont);
				// Position below the object, centered horizontally
				PointF labelPos = new PointF(screenPos.X - textSize.Width / 2, screenPos.Y + objectSize / 2 + 2); // 2px padding below

				g.DrawString(text, labelFont, labelBrush, labelPos);
			}
		}

		private void DrawSelectionHighlight(Graphics g)
		{
			if (_selectedObject != null && _celestialObjectScreenBounds.TryGetValue(_selectedObject, out RectangleF bounds))
			{
				// Slightly larger rectangle for the highlight
				float highlightPadding = 3f;
				RectangleF highlightRect = new RectangleF(
					bounds.X - highlightPadding,
					bounds.Y - highlightPadding,
					bounds.Width + highlightPadding * 2,
					bounds.Height + highlightPadding * 2);

				using (Pen selectionPen = new Pen(Color.Cyan, 1.5f)) // Cyan selection highlight
				{
					// Draw ellipse for stars/planets, rectangle for jump points
					if (_selectedObject is Star || _selectedObject is Planet)
					{
						g.DrawEllipse(selectionPen, highlightRect);
					}
					else if (_selectedObject is JumpSite)
					{
						g.DrawRectangle(selectionPen, Rectangle.Round(highlightRect));
					}
					// Add other types if needed
				}
			}
		}

		private void DrawInfoPanel(Graphics g)
		{
			if (lblInfo != null)
			{
				UpdateInfoLabel();
			}
		}

		private void UpdateInfoLabel()
		{
			if (_selectedObject == null)
			{

				string info = $"Looking at: {_system.Name}\n";
				info += "--------------------\n";
				info += $"Primary Star: {_system.PrimaryStar.BasicSpectralType}\n";
				info += $"SolarMass: {Math.Round(_system.PrimaryStar.SolarMasses,2)}; SolarRadius: {Math.Round(_system.PrimaryStar.SolarRadius,2)}\n";
				info += $"TempKelvin: {Math.Round(_system.PrimaryStar.TempKelvin, 2)}; SolarLuminosity: {Math.Round(_system.PrimaryStar.SolarLuminosity, 2)}\n";
				info += $"{_system.StarCode()}\n";
				lblInfo.Text = info;
			}
			else
			{
				// Build info string based on selected object type
				string info = $"Selected: {_selectedObject.GetType().Name}\n";
				info += "--------------------\n";

				if (_selectedObject is Star star)
				{
					info += $"Name: {star.Name}\n";
					info += $"Type: {star.BasicSpectralType}\n"; // Add more properties as needed
					info += $"Planets: {star.Planets?.Count ?? 0}\n";
				}
				else if (_selectedObject is Planet planet)
				{
					info += $"Name: {planet.Name}\n";
					info += $"Type: {planet.Type}\n";
					info += $"Orbit: {planet.SemiMajorAxis:F2} AU\n"; // Example property
					info += $"Moons: {planet.Moons?.Count ?? 0}\n";
				}
				else if (_selectedObject is JumpSite jumpSite)
				{
					info += $"Name: {jumpSite.Name}\n";
					info += $"Position: ({jumpSite.Position.X:F1}, {jumpSite.Position.Y:F1})\n"; // Show X, Y for 2D view
					info += $"Has Gate: {jumpSite.HasGateBuilt}\n";
					info += $"Partner: {(jumpSite.Partner != null ? jumpSite.Partner.ParentStarSystem.Name + " - JP " + jumpSite.Partner.Number : "None")}\n";
				}
				// Add more types (Moon, Ship, etc.) here

				lblInfo.Text = info;
			}
		}


		// --- Event Handlers for Interaction ---

		private void SystemView_Resize(object sender, EventArgs e)
		{
			systemPictureBox.Invalidate(); // Redraw when PictureBox is resized
		}

		private void SystemView_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Left) // Use either Mouse Button for panning
			{
				isDragging = true;
				dragStartPoint = e.Location;
				systemPictureBox.Cursor = Cursors.SizeAll; // Panning cursor
			}
			else if (e.Button == MouseButtons.Left)
			{
				// Check if clicked on an object
				PointF clickWorldPos = ScreenToWorld(e.Location); // Convert click to world coords if needed for physics checks

				object? clickedObject = null;
				// Iterate through stored bounds to find clicked object
				// Iterate in reverse to prioritize objects drawn last (usually smaller ones on top)
				foreach (var kvp in _celestialObjectScreenBounds.Reverse())
				{
					if (kvp.Value.Contains(e.Location))
					{
						clickedObject = kvp.Key;
						break; // Found the topmost object
					}
				}

				_selectedObject = clickedObject; // Update selection
				UpdateInfoLabel(); // Update the info display
				systemPictureBox.Invalidate(); // Redraw to show selection highlight
			}
		}

		private void SystemView_MouseMove(object sender, MouseEventArgs e)
		{
			if (isDragging)
			{
				// Calculate delta movement
				var dx = e.X - dragStartPoint.X;
				var dy = e.Y - dragStartPoint.Y;

				// Update the view offset
				viewOffset.X += dx;
				viewOffset.Y += dy;

				// Update the drag start point for the next move event
				dragStartPoint = e.Location;

				systemPictureBox.Invalidate(); // Request redraw
			}
			// Optional: Add hover effect logic here if needed
			// Find object under cursor, update hover state, invalidate if changed
		}

		private void SystemView_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Left) // Stop panning on either Mouse Button release
			{
				isDragging = false;
				systemPictureBox.Cursor = Cursors.Default; // Reset cursor
			}
		}

		private void SystemView_MouseWheel(object sender, MouseEventArgs e)
		{
			// Determine zoom direction
			float zoomSpeed = 1.2f; // Multiplicative zoom factor
			float oldZoom = zoomFactor;

			// Get mouse position in world coordinates *before* zoom
			PointF mouseWorldPosBeforeZoom = ScreenToWorld(e.Location);


			if (e.Delta > 0)
				zoomFactor *= zoomSpeed;    // Zoom In
			else
				zoomFactor /= zoomSpeed;    // Zoom Out

			// Clamp zoom factor to reasonable limits
			zoomFactor = Math.Clamp(zoomFactor, 0.05f, 50f); // Adjust min/max as needed

			// If zoom actually changed
			if (Math.Abs(zoomFactor - oldZoom) > 1e-6)
			{
				// Get mouse position in world coordinates *after* zoom (if offset didn't change)
				// We want the world point under the mouse to *stay* under the mouse.
				// ScreenPoint = WorldToScreen(WorldPoint)
				// ScreenPoint = (WorldPoint * newZoom * scale) + Center + newOffset
				// We know ScreenPoint (e.Location), WorldPoint (mouseWorldPosBeforeZoom), newZoom, scale, Center. Solve for newOffset.
				// newOffset = ScreenPoint - Center - (WorldPoint * newZoom * scale)

				PointF targetScreenPos = e.Location;
				float centerScreenX = systemPictureBox.ClientSize.Width / 2f;
				float centerScreenY = systemPictureBox.ClientSize.Height / 2f;
				float scaleFactor = zoomFactor * PIXELS_PER_DISTANCE_UNIT;

				float newOffsetX = targetScreenPos.X - centerScreenX - (mouseWorldPosBeforeZoom.X * scaleFactor);
				float newOffsetY = targetScreenPos.Y - centerScreenY - (mouseWorldPosBeforeZoom.Y * scaleFactor);

				viewOffset = new PointF(newOffsetX, newOffsetY);


				systemPictureBox.Invalidate(); // Redraw with new zoom and offset
			}
		}



		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close(); // Close this specific SystemView form
		}

	}
}
