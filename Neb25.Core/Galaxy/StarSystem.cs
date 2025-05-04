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
		public Vector3 GalacticPosition { get; set; } 

		/// <summary>
		/// Gets the list of stars within this star system.
		/// </summary>
		public List<Star> Stars { get; set; }

		/// <summary>
		/// Gets sets the primary star of this star system.
		/// </summary>
		public Star? PrimaryStar { get; set; }

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
		public bool hasBinary { get; set; }
		public Star binaryStarElement { get; set; }
		public bool hasTrinary { get; set; }
		public Star trinaryStarElement { get; set; }
		public bool hasQuaternary { get; set; }
		public Star quaternaryStarElement { get; set; }
		/// <summary>
		/// int value 1 to 5 representing how abundant the system is.
		/// </summary>
		public int AbundanceInt { get; set; }
		/// <summary>
		/// Description for how abundant the system is.
		/// </summary>
		public string AbundanceDesc { get; set; }





		/// <summary>
		/// Initializes a new instance of the <see cref="StarSystem"/> class.
		/// </summary>
		/// <param name="name">The name of the star system.</param>
		public StarSystem(Galaxy galaxy)
		{
			Id = Guid.NewGuid();
			Name = "Star: " + Id.ToString().Substring(30);
			GalacticPosition = Vector3.Zero;
			Stars = new List<Star>();
			Planets = new List<Planet>();
			JumpSites = new List<JumpSite>();
			ConnectedStarSystems = new List<StarSystem>();
			ParentGalaxy = galaxy;
			galaxy.StarSystems.Add(this);


			hasBinary = false;
			hasTrinary = false;
			hasQuaternary = false;
			AbundanceInt = 0;
			AbundanceDesc = "There's nothing.";



		}

		// Override ToString for easier debugging or display
		public override string ToString()
		{
			return $"{Name} ({GalacticPosition.X:F1}, {GalacticPosition.Y:F1}, {GalacticPosition.Z:F1}) - {Planets.Count} Planets";
		}

		/// <summary>
		/// A friendly way to view the system's star details, like when moused over, returns the string 'M2-IV (+3)'.
		/// </summary>
		public string StarCode()
		{
			if (PrimaryStar == null)
			{
				return "No Primary Star";
			}
			string starCode = PrimaryStar.StarCode();
			if (hasQuaternary) { starCode += " (+3)"; } else if (hasTrinary) { starCode += " (+2)"; } else if (hasBinary) { starCode += " (+1)";  }
				return string.IsNullOrWhiteSpace(starCode) ? "Unknown Star Type" : starCode;
		}


		public string SystemFacts()
		{
			if (PrimaryStar == null)
			{
				return "No Primary Star";
			}
			string starFacts = PrimaryStar.StarFacts();
			

			if (AbundanceInt == 5) { starFacts += " - Exceptional abundance"; }
			
			if (hasQuaternary) { starFacts += " (+3)"; } else if (hasTrinary) { starFacts += " (+2)"; } else if (hasBinary) { starFacts += " (+1)"; }
			return string.IsNullOrWhiteSpace(starFacts) ? "Unknown Star Type" : starFacts;

		}

	}
}