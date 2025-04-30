using System.Numerics;

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
		public Vector3 Position { get; set; } 

		/// <summary>
		/// Gets the list of stars within this star system.
		/// </summary>
		public List<Star> Stars { get; set; }

		/// <summary>
		/// Gets sets the primary star of this star system.
		/// </summary>
		public Star PrimaryStar { get; set; }

		/// <summary>
		/// Gets the list of planets within this star system.
		/// </summary>
		public List<Planet> Planets { get; set; }

		/// <summary>
		/// Gets the list of jump points within this star system.
		/// </summary>
		public List<JumpSite> JumpSites { get; set; }

		/// <summary>
		/// Gets the list connected star systems.
		/// </summary>
		public List<StarSystem> ConnectedStarSystems { get; set; } 

		/// <summary>
		/// The galaxy in which the star system lives.
		/// </summary>
		public Galaxy ParentGalaxy { get; set; }

	

		/// <summary>
		/// Initializes a new instance of the <see cref="StarSystem"/> class.
		/// </summary>
		/// <param name="name">The name of the star system.</param>
		public StarSystem(Galaxy galaxy)
		{
			Id = Guid.NewGuid();
			Name = "Star: " + Id.ToString().Substring(27);
			Position = Vector3.Zero;
			Stars = new List<Star>();
			PrimaryStar = new Star(this);
			Planets = new List<Planet>();
			JumpSites = new List<JumpSite>();
			ConnectedStarSystems = new List<StarSystem>();
			ParentGalaxy = galaxy;
		}

		// Override ToString for easier debugging or display
		public override string ToString()
		{
			return $"{Name} ({Position.X:F1}, {Position.Y:F1}, {Position.Z:F1}) - {Planets.Count} Planets";
		}
	}
}