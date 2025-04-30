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
		/// Size category or radius (could be int or double).
		/// </summary>
		public int Size { get; set; }

		/// <summary>
		///	Which planet in the parent star's orbit is this, zero index.
		/// </summary>
		public int OrbitalIndex { get; set; }

		/// <summary>
		/// Which star is this in orbit of.
		/// </summary>
		public Star ParentStar { get; set; }
		/// <summary>
		/// List of moons in orbit of this planet.
		/// </summary>
		public List<Moon> Moons { get; set; }
			
		public Planet(Star parentStar)
		{
			Id = Guid.NewGuid();
			OrbitalIndex = parentStar.Planets.Count;
			int friendlyOrbitalIndex = (int)(1 + OrbitalIndex);
			Name = parentStar.Name + " - Planet " + friendlyOrbitalIndex;
			Type = "Barren";
			Size = 42069;

			ParentStar = parentStar;
			Moons = new List<Moon>();
		}
	}
	
}
