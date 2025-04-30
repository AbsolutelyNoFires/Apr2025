namespace Neb25.Core.Galaxy
{
	public class Star
	{

		/// <summary>
		/// Unique identifier for the star.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Name of the star (e.g., "Promixa Centauri - Alpha", "Proxima Centauri - Beta").
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Type of star (e.g., "Red Dwarf", "Brown Giant").
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// Size category or radius (could be int or double).
		/// </summary>
		public int Size { get; set; }

		/// <summary>
		/// Parent star system in which the star lives.
		/// </summary>
		public StarSystem ParentStarSystem { get; set; }

		/// <summary>
		/// List of planets in orbit of this star, if any.
		/// </summary>
		public List<Planet> Planets { get; set; }


		/// <summary>
		/// Constructor for a Star.
		/// </summary>
		public Star(StarSystem parentStarSystem)
		{
			Id = Guid.NewGuid();
			int parentSystemNumStars = parentStarSystem.Stars.Count;
			string newLetter = ((char)65+ parentSystemNumStars).ToString();
			Name = parentStarSystem.Name+' '+ newLetter;
			Type = "Red Dwarf";
			Size = 42069;
			ParentStarSystem = parentStarSystem;
			Planets = new List<Planet>();
		}
		public override string ToString()
		{
			return $"{Name} - {Planets.Count} Planets";
		}
	}
}
