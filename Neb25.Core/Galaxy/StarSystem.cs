using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neb25.Core
{ 
	public class StarSystem
	{
		/// <summary>
		/// Unique identifier for the system (optional but recommended).
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Name of the star system (e.g., "Sol", "Alpha Centauri").
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Position of the star system in galaxy coordinates.
		/// Using PointF for simplicity (X, Y). Consider a custom Vector2/3 struct later.
		/// </summary>
		public PointF Position { get; set; }

		/// <summary>
		/// List of planets within this star system.
		/// </summary>
		public List<Planet> Planets { get; set; }

		/// <summary>
		/// Constructor for a StarSystem.
		/// </summary>
		/// <param name="name">Name of the system.</param>
		/// <param name="position">Position in the galaxy.</param>
		public StarSystem(string name, PointF position)
		{
			Id = Guid.NewGuid(); // Assign a unique ID
			Name = name;
			Position = position;
			Planets = new List<Planet>(); // Initialize the list
		}

		// Add methods later for adding planets, updating, etc.
		public void AddPlanet(Planet planet)
		{
			Planets.Add(planet);
		}


	}
}
