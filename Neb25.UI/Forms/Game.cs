using Neb25.Core.Galaxy;
using System;
using System.Drawing;
using System.Numerics; // For Vector3 used by StarSystem
using System.Windows.Forms;
using System.Collections.Generic; // For List
using System.Drawing.Drawing2D; // For SmoothingMode
using System.Linq; // Required for FirstOrDefault

namespace Neb25.UI.Forms
{
	public partial class Game : Form
	{
		// Store the galaxy passed to the form
		private readonly Core.Galaxy.Galaxy _currentGalaxy; // Made readonly, initialized in constructor

		// --- Visualization Fields ---
		private PointF _viewOffset = PointF.Empty; // Pan offset (in world coordinates)
		private float _zoomLevel = 0.05f; // Zoom factor (pixels per world unit)
		private Point _lastMousePosition = Point.Empty; // For panning calculation
		private bool _isPanning = false;
		private const float MinZoom = 0.005f; // Minimum zoom level
		private const float MaxZoom = 10.0f;  // Maximum zoom level
		private const float ZoomFactor = 1.2f; // How much zoom changes per wheel tick

		// --- Brushes and Pens (cache for performance) ---
		private readonly Brush _starBrush = Brushes.White;
		private readonly Brush _backgroundBrush = Brushes.Black; // Although PictureBox BackColor is used, could be useful
		private readonly Pen _debugCenterPen = Pens.Red; // Optional: for debugging center

		/// <summary>
		/// Initializes a new instance of the Game form with a specific galaxy.
		/// </summary>
		/// <param name="galaxy">The galaxy data to display and interact with.</param>
		/// <exception cref="ArgumentNullException">Thrown if galaxy is null.</exception>
		public Game(Core.Galaxy.Galaxy galaxy) // Constructor now accepts a Galaxy object
		{
			InitializeComponent();

			// Store the provided galaxy, ensuring it's not null
			_currentGalaxy = galaxy ?? throw new ArgumentNullException(nameof(galaxy), "A valid galaxy must be provided.");

			SetupGalaxyView(); // Initialize visualization settings
		}

		/// <summary>
		/// Sets up the PictureBox for galaxy drawing.
		/// </summary>
		private void SetupGalaxyView()
		{
			// Find the PictureBox - ensure it's added in Game.Designer.cs
			// If you named it differently, change "galaxyPictureBox" here.
			if (this.Controls.Find("galaxyPictureBox", true).FirstOrDefault() is PictureBox pb)
			{
				// Double buffering reduces flicker
				// Use reflection as DoubleBuffered is protected
				pb.GetType().GetProperty("DoubleBuffered",
					System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
					?.SetValue(pb, true, null);

				pb.BackColor = Color.Black; // Set background color
				pb.Paint += GalaxyPictureBox_Paint; // Wire up the paint event
				pb.MouseDown += GalaxyPictureBox_MouseDown;
				pb.MouseMove += GalaxyPictureBox_MouseMove;
				pb.MouseUp += GalaxyPictureBox_MouseUp;
				pb.MouseWheel += GalaxyPictureBox_MouseWheel; // Wire up mouse wheel for zooming
				pb.Resize += (sender, e) => pb.Invalidate(); // Redraw on resize

				// Set initial zoom based on galaxy size (optional refinement)
				// Find max extent to set a reasonable starting zoom
				if (_currentGalaxy.StarSystems.Any())
				{
					float maxDist = _currentGalaxy.StarSystems.Max(s => Math.Max(Math.Abs(s.Position.X), Math.Abs(s.Position.Y)));
					if (maxDist > 1.0f && pb.Width > 0 && pb.Height > 0) // Avoid division by zero
					{
						// Aim to fit most of the galaxy initially
						float zoomX = pb.Width / (maxDist * 2.5f); // *2.5 to add some padding
						float zoomY = pb.Height / (maxDist * 2.5f);
						_zoomLevel = Math.Min(zoomX, zoomY); // Use the more constrained dimension
						_zoomLevel = Math.Clamp(_zoomLevel, MinZoom, MaxZoom); // Clamp to limits
					}
				}

			}
			else
			{
				// Handle case where PictureBox wasn't found
				MessageBox.Show("Error: Galaxy PictureBox ('galaxyPictureBox') not found on the Game form. Please add it in the designer.",
								"Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				// Consider closing the form or preventing further execution if the PB is essential
				// this.Load += (s, e) => this.Close(); // Example: Close form if PB missing
			}
		}


		/// <summary>
		/// Handles the Load event of the Game form.
		/// </summary>
		private void Game_Load(object sender, EventArgs e)
		{
			// Galaxy is now passed in via the constructor.
			// Remove the generation logic from here.

			// Trigger initial draw now that the form is loaded and sized
			if (this.Controls.Find("galaxyPictureBox", true).FirstOrDefault() is PictureBox pb)
			{
				pb.Invalidate();
			}
		}

		// --- Event Handlers for Visualization ---

		/// <summary>
		/// Handles the Paint event for the galaxy PictureBox. Draws the galaxy representation.
		/// </summary>
		private void GalaxyPictureBox_Paint(object? sender, PaintEventArgs e)
		{
			// _currentGalaxy is now guaranteed non-null by the constructor
			if (sender is not PictureBox pb) return;

			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias; // Make circles look smoother
			g.Clear(pb.BackColor); // Clear with background color

			// Calculate the center of the PictureBox (view center)
			PointF viewCenter = new PointF(pb.Width / 2.0f, pb.Height / 2.0f);

			// Draw each star system
			foreach (var system in _currentGalaxy.StarSystems)
			{
				// Convert 3D world position to 2D screen position
				PointF screenPos = WorldToScreen(system.Position, viewCenter, _viewOffset, _zoomLevel);

				// Simple culling: Don't draw if way off-screen (basic optimization)
				float starDrawSize = Math.Max(1.0f, 2.0f * _zoomLevel); // Use a variable for size calculation
				float cullMargin = starDrawSize * 2; // Margin based on star size
				if (screenPos.X < -cullMargin || screenPos.X > pb.Width + cullMargin ||
					screenPos.Y < -cullMargin || screenPos.Y > pb.Height + cullMargin)
				{
					continue;
				}

				// Draw the star (e.g., a small circle)
				g.FillEllipse(_starBrush, screenPos.X - starDrawSize / 2, screenPos.Y - starDrawSize / 2, starDrawSize, starDrawSize);

				// TODO: Draw system names when zoomed in enough
				// TODO: Draw connections (hyperlanes) if implemented
				// TODO: Color stars based on type or owner
			}

			// --- Optional: Display Debug Info ---
			// string debugText = $"Zoom: {_zoomLevel:F3}, Offset: ({_viewOffset.X:F1}, {_viewOffset.Y:F1}), Stars: {_currentGalaxy.StarSystems.Count}";
			// g.DrawString(debugText, this.Font, Brushes.Yellow, 10, 10);
			// --- End Optional ---
		}

		/// <summary>
		/// Handles mouse down event for panning.
		/// </summary>
		private void GalaxyPictureBox_MouseDown(object? sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle) // Use Left or Middle mouse for panning
			{
				_isPanning = true;
				_lastMousePosition = e.Location;
				// Capture mouse to handle dragging outside the control boundaries
				(sender as Control)!.Capture = true;
				(sender as Control)!.Cursor = Cursors.Hand; // Change cursor to indicate panning
			}
		}

		/// <summary>
		/// Handles mouse move event for panning.
		/// </summary>
		private void GalaxyPictureBox_MouseMove(object? sender, MouseEventArgs e)
		{
			if (_isPanning && sender is PictureBox pb)
			{
				// Prevent panning if zoom is too small (avoids large jumps / potential division issues)
				if (Math.Abs(_zoomLevel) < 1e-6) return;

				Point currentMousePosition = e.Location;
				// Calculate change in screen coordinates
				float dx = currentMousePosition.X - _lastMousePosition.X;
				float dy = currentMousePosition.Y - _lastMousePosition.Y;

				// Adjust view offset based on mouse movement scaled by inverse zoom level
				// Panning should move the world opposite to the mouse drag direction
				_viewOffset.X -= dx / _zoomLevel;
				_viewOffset.Y -= dy / _zoomLevel; // Screen Y is inverted relative to typical cartesian Y

				_lastMousePosition = currentMousePosition;
				pb.Invalidate(); // Request redraw
			}
			// TODO: Add hover effects (e.g., highlight nearest star) here if desired
		}

		/// <summary>
		/// Handles mouse up event to stop panning.
		/// </summary>
		private void GalaxyPictureBox_MouseUp(object? sender, MouseEventArgs e)
		{
			if (_isPanning)
			{
				_isPanning = false;
				(sender as Control)!.Capture = false; // Release mouse capture
				(sender as Control)!.Cursor = Cursors.Default; // Restore default cursor
			}
			// TODO: Add selection logic here (e.g., if click without dragging)
		}

		/// <summary>
		/// Handles mouse wheel event for zooming.
		/// </summary>
		private void GalaxyPictureBox_MouseWheel(object? sender, MouseEventArgs e)
		{
			if (sender is not PictureBox pb) return;

			// Calculate zoom factor (increase or decrease)
			float zoomMultiplier = (e.Delta > 0) ? ZoomFactor : 1.0f / ZoomFactor;
			float newZoom = _zoomLevel * zoomMultiplier;

			// Clamp zoom level within min/max bounds
			newZoom = Math.Clamp(newZoom, MinZoom, MaxZoom);

			// If zoom didn't change (due to clamping), exit
			if (Math.Abs(newZoom - _zoomLevel) < float.Epsilon) return;

			// --- Zoom towards mouse cursor ---
			// 1. Get mouse position relative to the control
			PointF mousePos = e.Location;

			// 2. Calculate world coordinates under the cursor *before* zoom
			PointF worldPosUnderMouse = ScreenToWorld(mousePos, new PointF(pb.Width / 2.0f, pb.Height / 2.0f), _viewOffset, _zoomLevel);

			// 3. Update zoom level
			_zoomLevel = newZoom;

			// Prevent division by zero in adjustment calculation if zoom becomes extremely small
			if (Math.Abs(_zoomLevel) < float.Epsilon) return;

			// 4. Calculate where the *same* world coordinates should be *after* zoom
			PointF screenPosAfterZoom = WorldToScreen(new Vector3(worldPosUnderMouse.X, worldPosUnderMouse.Y, 0), new PointF(pb.Width / 2.0f, pb.Height / 2.0f), _viewOffset, _zoomLevel);

			// 5. Adjust the offset so the world point stays under the mouse
			// We want the new screen position (screenPosAfterZoom) to be where the mouse cursor *is* (mousePos).
			// The difference needs to be compensated by adjusting the view offset (in world units).
			_viewOffset.X -= (mousePos.X - screenPosAfterZoom.X) / _zoomLevel;
			_viewOffset.Y -= (mousePos.Y - screenPosAfterZoom.Y) / _zoomLevel; // Screen Y is inverted
																			   // --- End Zoom towards mouse cursor ---

			pb.Invalidate(); // Request redraw after zoom
		}


		// --- Coordinate Transformation Helpers ---

		/// <summary>
		/// Converts 3D World Coordinates (using X, Y) to 2D Screen Coordinates.
		/// </summary>
		/// <param name="worldPos">The Vector3 world position (only X and Y are used).</param>
		/// <param name="viewCenter">The center point of the PictureBox.</param>
		/// <param name="offset">The current pan offset (in world coordinates).</param>
		/// <param name="zoom">The current zoom level (pixels per world unit).</param>
		/// <returns>The corresponding PointF on the screen.</returns>
		private PointF WorldToScreen(Vector3 worldPos, PointF viewCenter, PointF offset, float zoom)
		{
			// 1. Apply offset: Translate world based on the view offset.
			float offsetX = worldPos.X - offset.X;
			float offsetY = worldPos.Y - offset.Y; // Using Y for screen Y

			// 2. Apply zoom: Scale the offset coordinates.
			float zoomedX = offsetX * zoom;
			float zoomedY = offsetY * zoom;

			// 3. Translate to view center: Position relative to the PictureBox center.
			// Add zoomedX to center X.
			// Add zoomedY to center Y (Screen Y increases downwards, matching typical Cartesian Y after offset and zoom).
			float screenX = viewCenter.X + zoomedX;
			float screenY = viewCenter.Y + zoomedY;

			return new PointF(screenX, screenY);
		}

		/// <summary>
		/// Converts 2D Screen Coordinates back to 2D World Coordinates (X, Y plane).
		/// </summary>
		/// <param name="screenPos">The PointF screen position.</param>
		/// <param name="viewCenter">The center point of the PictureBox.</param>
		/// <param name="offset">The current pan offset (in world coordinates).</param>
		/// <param name="zoom">The current zoom level (pixels per world unit).</param>
		/// <returns>The corresponding PointF in the world (X, Y plane).</returns>
		private PointF ScreenToWorld(PointF screenPos, PointF viewCenter, PointF offset, float zoom)
		{
			// Prevent division by zero if zoom is extremely small
			if (Math.Abs(zoom) < float.Epsilon)
			{
				// Return the current center of the view in world coordinates as a fallback
				return offset;
			}

			// 1. Translate from view center: Get coordinates relative to the center.
			float relativeX = screenPos.X - viewCenter.X;
			float relativeY = screenPos.Y - viewCenter.Y; // Screen Y increases downwards

			// 2. Apply inverse zoom: Scale back to world units.
			float unzoomedX = relativeX / zoom;
			float unzoomedY = relativeY / zoom;

			// 3. Apply inverse offset: Translate back based on the view offset.
			float worldX = unzoomedX + offset.X;
			float worldY = unzoomedY + offset.Y; // Add offset Y

			return new PointF(worldX, worldY);
		}

		// Add other game logic methods here (e.g., handling clicks on stars, turn progression)...
	}
}
