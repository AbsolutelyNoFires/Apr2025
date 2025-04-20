using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		// Add more properties later

		/// <summary>
		/// Constructor for a Star.
		/// </summary>
		/// <param name="name">Name of the Star.</param>
		/// <param name="type">Type of the star.</param>
		/// <param name="size">Size of the star.</param>
		public Star(string name, string type, int size)
		{
			Id = Guid.NewGuid();
			Name = name;
			Type = type;
			Size = size;
			// Initialize other properties/lists here if needed
		}
	}
}
