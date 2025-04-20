using System.Collections.Generic;
using System.Numerics; // Required for Vector3

namespace Neb25.Core.Galaxy
{
	/// <summary>
	/// Represents a single star system within the galaxy.
	/// </summary>
	public class StarSystem
	{
		/// <summary>
		/// Gets or sets the unique identifier for the star system.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the star system.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the 3D position of the star system in the galaxy.
		/// Coordinates are relative to the galactic center (0,0,0).
		/// </summary>
		public Vector3 Position { get; set; } // Added Position

		/// <summary>
		/// Gets the list of stars within this star system.
		/// </summary>
		public List<Star> Stars { get; } = new List<Star>(); // Initialize the list

		/// <summary>
		/// Gets the list of planets within this star system.
		/// </summary>
		public List<Planet> Planets { get; } = new List<Planet>(); // Initialize the list

		// TODO: Add properties for star type, resources, owner empire, etc.

		/// <summary>
		/// Initializes a new instance of the <see cref="StarSystem"/> class.
		/// </summary>
		/// <param name="name">The name of the star system.</param>
		public StarSystem(string name)
		{
			Id = Guid.NewGuid();
			Name = name;
			Position = Vector3.Zero; // Default position
									 // Planets list is initialized above
		}

		// Override ToString for easier debugging or display
		public override string ToString()
		{
			return $"{Name} ({Position.X:F1}, {Position.Y:F1}, {Position.Z:F1}) - {Planets.Count} Planets";
		}
	}
}