using Neb25.Core.Galaxy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neb25.UI.Forms
{
	public partial class SystemViewForm : Form
	{
		private readonly StarSystem _system;

		// --- Visualization Fields ---
		private PointF _viewOffset = PointF.Empty; // Offset from center in world coordinates
		private float _zoomLevel = 1.0f;          // Zoom factor
		private Point _lastMousePosition = Point.Empty;
		private bool _isPanning = false;

		private const float MinZoom = 0.1f;
		private const float MaxZoom = 10.0f;
		private const float ZoomFactor = 1.2f;

		// --- Interaction State ---
		private object? _selectedObject = null; // Can be Planet or JumpSite

		// --- Drawing Resources ---
		private readonly Brush _starBrush = Brushes.Yellow;
		private readonly Brush _planetBrush = Brushes.CornflowerBlue;
		private readonly Brush _jumpSiteBrush = Brushes.LimeGreen;
		private readonly Brush _jumpSiteConnectedBrush = Brushes.DarkGreen; // Different color if connected
		private readonly Pen _jumpSiteLinkPen = new Pen(Color.FromArgb(100, 0, 200, 0), 1.5f); // Semi-transparent green
		private readonly Font _infoFont = new Font("Consolas", 8f);
		private readonly Brush _infoBrush = Brushes.White;


		public SystemViewForm(StarSystem system)
		{
			this.Text += system.Name;
			InitializeComponent();
			_system = system ?? throw new ArgumentNullException(nameof(system));

			// Setup PictureBox
			systemPictureBox.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?.SetValue(systemPictureBox, true, null);
			systemPictureBox.Paint += SystemPictureBox_Paint;
			systemPictureBox.MouseDown += SystemPictureBox_MouseDown;
			systemPictureBox.MouseMove += SystemPictureBox_MouseMove;
			systemPictureBox.MouseUp += SystemPictureBox_MouseUp;
			systemPictureBox.MouseWheel += SystemPictureBox_MouseWheel;
			systemPictureBox.Resize += (sender, e) => systemPictureBox.Invalidate(); // Redraw on resize

			// Initial Setup
			CalculateInitialZoom();

		}

		private void SystemViewForm_Load(object sender, EventArgs e)
		{
			lblSystemName.Text = _system.Name;
			UpdateInfoPanel(); // Show initial system info
			systemPictureBox.Invalidate(); // Initial draw
		}

		private void CalculateInitialZoom()
		{
			if (!systemPictureBox.ClientSize.IsEmpty)
			{
				float maxOrbit = 10f; // Default if no objects
				/*
				if (_system.Planets.Any())
					maxOrbit = Math.Max(maxOrbit, _system.Planets.Max(p => Math.Max(Math.Abs(p.Position.X), Math.Abs(p.Position.Y))));
				if (_system.JumpSites.Any())
					maxOrbit = Math.Max(maxOrbit, _system.JumpSites.Max(js => Math.Max(Math.Abs(js.Position.X), Math.Abs(js.Position.Y))));
				*/


				// Add buffer
				maxOrbit *= 1.2f;

				if (maxOrbit > 1f)
				{
					float zoomX = systemPictureBox.ClientSize.Width / (maxOrbit * 2f);
					float zoomY = systemPictureBox.ClientSize.Height / (maxOrbit * 2f);
					_zoomLevel = Math.Min(zoomX, zoomY);
					_zoomLevel = Math.Clamp(_zoomLevel, MinZoom, MaxZoom);
				}
				else
				{
					_zoomLevel = 1.0f; // Default zoom if system is very small or empty
				}
			}
			else
			{
				_zoomLevel = 1.0f; // Default if picturebox size is not yet known
			}

			// Center view initially
			_viewOffset = PointF.Empty;
		}


		// --- Event Handlers ---

		private void SystemPictureBox_Paint(object? sender, PaintEventArgs e)
		{
			if (sender is not PictureBox pb) return;
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.Clear(pb.BackColor);

			PointF viewCenter = new PointF(pb.Width / 2.0f, pb.Height / 2.0f);

			// --- Draw Star(s) ---
			// Assuming the first star is the primary at the center for now
			if (_system.Stars.Any())
			{
				var primaryStar = _system.Stars.First();
				PointF starScreenPos = WorldToScreen(Vector3.Zero, viewCenter); // Star at system origin
				float starDrawSize = Math.Max(5f, primaryStar.Size * 2f * _zoomLevel); // Size based on Star.Size and zoom
				g.FillEllipse(_starBrush, starScreenPos.X - starDrawSize / 2, starScreenPos.Y - starDrawSize / 2, starDrawSize, starDrawSize);

				// Draw Star Name near it
				g.DrawString(primaryStar.Name, _infoFont, _infoBrush, starScreenPos.X + starDrawSize / 2, starScreenPos.Y);
			}

			// --- Draw Planets ---
			foreach (var planet in _system.Planets)
			{
				// TODO: Calculate planet positions based on orbital mechanics later
				// For now, use a fixed position relative to the star for display
				// Let's assign a placeholder position if needed (Planet class lacks Position currently)
				// Assigning temporary orbital positions for visualization:
				int planetIndex = _system.Planets.IndexOf(planet);
				float orbitRadius = 30f + planetIndex * 25f; // Simple increasing radius
				float angle = (float)(planetIndex * Math.PI / 4); // Spread them out
				Vector3 planetPos = new Vector3(orbitRadius * (float)Math.Cos(angle), orbitRadius * (float)Math.Sin(angle), 0);


				PointF planetScreenPos = WorldToScreen(planetPos, viewCenter);
				float planetDrawSize = Math.Max(2f, planet.Size * 1.5f * _zoomLevel); // Size based on Planet.Size and zoom

				// Draw planet
				g.FillEllipse(_planetBrush, planetScreenPos.X - planetDrawSize / 2, planetScreenPos.Y - planetDrawSize / 2, planetDrawSize, planetDrawSize);

				// Highlight selected planet
				if (_selectedObject == planet)
				{
					g.DrawEllipse(Pens.Cyan, planetScreenPos.X - planetDrawSize / 2 - 2, planetScreenPos.Y - planetDrawSize / 2 - 2, planetDrawSize + 4, planetDrawSize + 4);
				}
				// Draw Planet Name near it
				g.DrawString(planet.Name, _infoFont, _infoBrush, planetScreenPos.X + planetDrawSize / 2, planetScreenPos.Y);
			}

			// --- Draw Jump Sites ---
			foreach (var jumpSite in _system.JumpSites)
			{
				PointF jsScreenPos = WorldToScreen(jumpSite.Position, viewCenter); // Use the generated position
				float jsDrawSize = 6f * _zoomLevel; // Fixed size for jump points, scaled by zoom
				jsDrawSize = Math.Max(2f, jsDrawSize); // Minimum size

				Brush currentJsBrush = jumpSite.HasPartner ? _jumpSiteConnectedBrush : _jumpSiteBrush;
				RectangleF jsRect = new RectangleF(jsScreenPos.X - jsDrawSize / 2, jsScreenPos.Y - jsDrawSize / 2, jsDrawSize, jsDrawSize);

				// Draw jump site (e.g., as a diamond/square)
				// Simple square for now:
				g.FillRectangle(currentJsBrush, jsRect);

				// Draw line to partner system (visual only, doesn't leave the view)
				if (jumpSite.HasPartner && jumpSite.Partner != null)
				{
					// Draw a line pointing outwards roughly in the direction of the partner
					// This is symbolic as the partner is in another system view
					Vector3 direction = Vector3.Normalize(jumpSite.Position); // Direction from center
					if (direction == Vector3.Zero) direction = Vector3.UnitX; // Avoid zero vector if at center
					PointF endPoint = new PointF(jsScreenPos.X + direction.X * 20 * _zoomLevel, jsScreenPos.Y + direction.Y * 20 * _zoomLevel);
					g.DrawLine(_jumpSiteLinkPen, jsScreenPos, endPoint);
				}


				// Highlight selected jump site
				if (_selectedObject == jumpSite)
				{
					g.DrawRectangle(Pens.Cyan, jsRect.X - 2, jsRect.Y - 2, jsRect.Width + 4, jsRect.Height + 4);
				}
				// Draw Jump Site number near it
				g.DrawString($"JP {jumpSite.Number}", _infoFont, _infoBrush, jsScreenPos.X + jsDrawSize / 2, jsScreenPos.Y);
			}
		}

		private void SystemPictureBox_MouseDown(object? sender, MouseEventArgs e)
		{
			_lastMousePosition = e.Location;
			if (e.Button == MouseButtons.Left)
			{
				// Check for object selection
				_selectedObject = FindObjectAtScreenPoint(e.Location, (sender as PictureBox)!);
				UpdateInfoPanel(); // Update display based on selection
				(sender as PictureBox)?.Invalidate(); // Redraw to show selection highlight
			}
			else if (e.Button == MouseButtons.Right) // Use right mouse for panning
			{
				_isPanning = true;
				(sender as Control)!.Cursor = Cursors.Hand; // Use Hand cursor for panning
				(sender as Control)!.Capture = true;
			}
		}

		private void SystemPictureBox_MouseMove(object? sender, MouseEventArgs e)
		{
			if (_isPanning)
			{
				Point currentMousePosition = e.Location;
				float dx = currentMousePosition.X - _lastMousePosition.X;
				float dy = currentMousePosition.Y - _lastMousePosition.Y;

				// Adjust offset based on mouse movement and zoom
				if (Math.Abs(_zoomLevel) > 1e-6) // Avoid division by zero
				{
					_viewOffset.X -= dx / _zoomLevel;
					_viewOffset.Y -= dy / _zoomLevel;
				}

				_lastMousePosition = currentMousePosition;
				(sender as PictureBox)?.Invalidate(); // Redraw needed
			}
		}

		private void SystemPictureBox_MouseUp(object? sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) // Stop panning on right mouse up
			{
				_isPanning = false;
				(sender as Control)!.Cursor = Cursors.Default;
				(sender as Control)!.Capture = false;
			}
		}

		private void SystemPictureBox_MouseWheel(object? sender, MouseEventArgs e)
		{
			if (sender is not PictureBox pb) return;
			pb.Focus(); // Ensure picture box has focus for wheel events

			float zoomMultiplier = (e.Delta > 0) ? ZoomFactor : 1.0f / ZoomFactor;
			float newZoom = _zoomLevel * zoomMultiplier;
			newZoom = Math.Clamp(newZoom, MinZoom, MaxZoom); // Clamp zoom level

			if (Math.Abs(newZoom - _zoomLevel) < float.Epsilon) return; // No change

			// --- Zoom centering ---
			PointF mousePos = e.Location; // Mouse position in control coordinates
			PointF viewCenter = new PointF(pb.Width / 2.0f, pb.Height / 2.0f);

			// Calculate world coordinates under the mouse BEFORE zoom
			PointF worldPosUnderMouse = ScreenToWorld(mousePos, viewCenter);

			// Apply the new zoom level
			_zoomLevel = newZoom;

			// Calculate screen coordinates of the same world point AFTER zoom
			PointF screenPosAfterZoom = WorldToScreen(new Vector3(worldPosUnderMouse.X, worldPosUnderMouse.Y, 0), viewCenter);

			// Adjust the view offset to keep the world point under the mouse
			if (Math.Abs(_zoomLevel) > float.Epsilon)
			{
				_viewOffset.X -= (mousePos.X - screenPosAfterZoom.X) / _zoomLevel;
				_viewOffset.Y -= (mousePos.Y - screenPosAfterZoom.Y) / _zoomLevel;
			}
			// --- End Zoom centering ---

			pb.Invalidate(); // Redraw with new zoom and offset
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close(); // Close the System View form
		}


		// --- Helper Methods ---

		private void UpdateInfoPanel()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"System: {_system.Name}");
			sb.AppendLine("--------------------");

			if (_selectedObject == null)
			{
				sb.AppendLine($"Stars: {_system.Stars.Count}");
				sb.AppendLine($"Planets: {_system.Planets.Count}");
				sb.AppendLine($"Jump Sites: {_system.JumpSites.Count}");
				sb.AppendLine();
				sb.AppendLine("Select an object for details.");
			}
			else if (_selectedObject is Planet planet)
			{
				sb.AppendLine($"Selected: Planet");
				sb.AppendLine($"Name: {planet.Name}");
				sb.AppendLine($"Type: {planet.Type ?? "N/A"}");
				sb.AppendLine($"Size: {planet.Size}");
				// Add more planet details here later (resources, moons, etc.)
			}
			else if (_selectedObject is JumpSite jumpSite)
			{
				sb.AppendLine($"Selected: Jump Site");
				sb.AppendLine($"Number: JP {jumpSite.Number}");
				sb.AppendLine($"Position: ({jumpSite.Position.X:F0}, {jumpSite.Position.Y:F0}, {jumpSite.Position.Z:F0})");
				sb.AppendLine($"Gate Built: {jumpSite.HasGateBuilt}");
				if (jumpSite.HasPartner && jumpSite.Partner != null)
				{
					sb.AppendLine($"Connected To: {jumpSite.Partner.ParentStarSystem.Name} (JP {jumpSite.Partner.Number})");
				}
				else
				{
					sb.AppendLine($"Connected To: None");
				}
			}
			else if (_selectedObject is Star star) // Added case for selecting the star itself
			{
				sb.AppendLine($"Selected: Star");
				sb.AppendLine($"Name: {star.Name}");
				sb.AppendLine($"Type: {star.Type ?? "N/A"}");
				sb.AppendLine($"Size: {star.Size}");
			}

			lblInfo.Text = sb.ToString();
		}

		private object? FindObjectAtScreenPoint(PointF screenPoint, PictureBox pb)
		{
			PointF viewCenter = new PointF(pb.Width / 2.0f, pb.Height / 2.0f);
			float clickRadius = 5f; // Base click radius in screen pixels
									// Note: We don't scale click radius by zoom here, selection is based on screen proximity

			// Check Jump Sites first (often smaller targets)
			foreach (var jumpSite in _system.JumpSites.Reverse<JumpSite>()) // Reverse to check topmost first if overlapping
			{
				PointF jsScreenPos = WorldToScreen(jumpSite.Position, viewCenter);
				float jsDrawSize = Math.Max(2f, 6f * _zoomLevel); // Match drawing size calculation
				RectangleF jsRect = new RectangleF(jsScreenPos.X - jsDrawSize / 2, jsScreenPos.Y - jsDrawSize / 2, jsDrawSize, jsDrawSize);
				// Increase clickable area slightly
				jsRect.Inflate(clickRadius, clickRadius);
				if (jsRect.Contains(screenPoint))
				{
					return jumpSite;
				}
			}

			// Check Planets
			foreach (var planet in _system.Planets.Reverse<Planet>())
			{
				// Recalculate temporary position used for drawing
				int planetIndex = _system.Planets.IndexOf(planet);
				float orbitRadius = 30f + planetIndex * 25f;
				float angle = (float)(planetIndex * Math.PI / 4);
				Vector3 planetPos = new Vector3(orbitRadius * (float)Math.Cos(angle), orbitRadius * (float)Math.Sin(angle), 0);

				PointF planetScreenPos = WorldToScreen(planetPos, viewCenter);
				float planetDrawSize = Math.Max(2f, planet.Size * 1.5f * _zoomLevel);
				RectangleF planetRect = new RectangleF(planetScreenPos.X - planetDrawSize / 2, planetScreenPos.Y - planetDrawSize / 2, planetDrawSize, planetDrawSize);
				planetRect.Inflate(clickRadius, clickRadius);
				if (planetRect.Contains(screenPoint))
				{
					return planet;
				}
			}

			// Check Star(s)
			if (_system.Stars.Any())
			{
				var primaryStar = _system.Stars.First();
				PointF starScreenPos = WorldToScreen(Vector3.Zero, viewCenter);
				float starDrawSize = Math.Max(5f, primaryStar.Size * 2f * _zoomLevel);
				RectangleF starRect = new RectangleF(starScreenPos.X - starDrawSize / 2, starScreenPos.Y - starDrawSize / 2, starDrawSize, starDrawSize);
				starRect.Inflate(clickRadius, clickRadius);
				if (starRect.Contains(screenPoint))
				{
					return primaryStar; // Return the Star object
				}
			}


			return null; // Nothing found at this point
		}


		// --- Coordinate Transformation Helpers (System View Specific) ---
		// Converts world coordinates (relative to system center 0,0,0) to screen coordinates
		private PointF WorldToScreen(Vector3 worldPos, PointF viewCenter)
		{
			// Use only X and Y for a top-down 2D view
			float worldX = worldPos.X;
			float worldY = worldPos.Y;

			// Apply view offset
			float offsetX = worldX - _viewOffset.X;
			float offsetY = worldY - _viewOffset.Y;

			// Apply zoom
			float zoomedX = offsetX * _zoomLevel;
			float zoomedY = offsetY * _zoomLevel;

			// Convert to screen coordinates (Y is inverted for screen space)
			float screenX = viewCenter.X + zoomedX;
			float screenY = viewCenter.Y + zoomedY; // In WinForms, +Y is down, matching typical top-down map view

			return new PointF(screenX, screenY);
		}

		// Converts screen coordinates to world coordinates (relative to system center 0,0,0)
		private PointF ScreenToWorld(PointF screenPos, PointF viewCenter)
		{
			if (Math.Abs(_zoomLevel) < float.Epsilon) return _viewOffset; // Avoid division by zero

			// Calculate position relative to view center
			float relativeX = screenPos.X - viewCenter.X;
			float relativeY = screenPos.Y - viewCenter.Y; // Y is inverted

			// Remove zoom
			float unzoomedX = relativeX / _zoomLevel;
			float unzoomedY = relativeY / _zoomLevel;

			// Add view offset back
			float worldX = unzoomedX + _viewOffset.X;
			float worldY = unzoomedY + _viewOffset.Y;

			return new PointF(worldX, worldY);
		}
	}
}
