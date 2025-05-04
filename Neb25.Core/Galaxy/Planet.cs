namespace Neb25.Core.Galaxy
{
	public class Planet
	{
		/// <summary>
		/// Unique identifier for the planet.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Name of the planet (e.g., "Earth", "Mars").
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Type of planet (e.g., "Terrestrial", "Gas Giant", "Ice Giant", "Barren").
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// Radius of the planet in kilometers.
		/// </summary>
		public float Radius { get; set; }

		/// <summary>
		/// Mass of the planet in kilograms.
		/// </summary>
		public float Mass { get; set; }

		/// <summary>
		/// Density of the planet in kg/m³.
		/// </summary>
		public float Density { get; set; }

		/// <summary>
		/// Semi-major axis of the planet's orbit in astronomical units (AU).
		/// </summary>
		public float SemiMajorAxis { get; set; }

		/// <summary>
		/// Eccentricity of the planet's orbit (0 = circular, 0.9 = highly elliptical).
		/// </summary>
		public float Eccentricity { get; set; }

		/// <summary>
		/// Argument of periapsis in degrees (where the closest approach happens).
		/// </summary>
		public float ArgumentOfPeriapsis { get; set; }

		/// <summary>
		/// Mean anomaly at epoch (current position along orbit) in degrees.
		/// </summary>
		public float MeanAnomaly { get; set; }

		/// <summary>
		/// Planet's rotation period in hours (length of a day).
		/// </summary>
		public float SpinPeriod { get; set; }

		/// <summary>
		/// Zero-based index of the planet's orbit around its parent star.
		/// </summary>
		public int OrbitalIndex { get; set; }

		/// <summary>
		/// Reference to the parent star.
		/// </summary>
		public Star ParentStar { get; set; }

		/// <summary>
		/// List of moons orbiting the planet.
		/// </summary>
		public List<Moon> Moons { get; set; }

		
		/// <summary>
		/// Constructs a new planet orbiting the specified parent star.
		/// </summary>
		/// <param name="parentStar">The star the planet orbits.</param>
		public Planet(Star parentStar)
		{
			Id = Guid.NewGuid();
			ParentStar = parentStar;
			OrbitalIndex = parentStar.Planets.Count;
			int friendlyOrbitalIndex = OrbitalIndex + 1;

			Name = $"{parentStar.Name} - Planet {friendlyOrbitalIndex}";
			Type = "Barren"; // Default type
			Radius = 6371f; // Default to Earth radius
			Mass = 5.972e24f; // Default to Earth mass
			Density = 5515f; // Default to Earth density (kg/m³)
			SemiMajorAxis = 1.0f; // 1 AU
			Eccentricity = 0.0167f; // Earth's eccentricity
			ArgumentOfPeriapsis = 0f;
			MeanAnomaly = 0f;
			SpinPeriod = 24f; // 24 hours

			Moons = new List<Moon>();

			// Add this planet to the parent's planet list
			parentStar.Planets.Add(this);
			parentStar.ParentStarSystem.Planets.Add(this);
		}
	}
}
