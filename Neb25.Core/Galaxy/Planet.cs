using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

			// Add more properties later: Gravity, Resources, Moons, Atmosphere, etc.

			/// <summary>
			/// Constructor for a Planet.
			/// </summary>
			/// <param name="name">Name of the planet.</param>
			
			public Planet(string name)
			{
				Id = Guid.NewGuid();
				Name = name;
				// Initialize other properties/lists here if needed
			}
		}
	
}
