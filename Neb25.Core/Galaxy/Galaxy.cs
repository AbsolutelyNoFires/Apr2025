using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Neb25.Core.Galaxy
{
	public class Galaxy
	{
		/// <summary>
		/// Unique identifier for the galaxy.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Galaxy friendly name
		/// </summary>
		public string Name { get; set; } = new string("New Galaxy");

		/// <summary>
		/// List of stars in the galaxy.
		/// </summary>
		public List<StarSystem> StarSystems { get; set; }

		/// <summary>
		/// Constructor for a Galaxy.
		/// </summary>
		/// <param name="name">Name of the system.</param>
		public Galaxy()
		{
			Id = Guid.NewGuid(); // Assign a unique ID
			StarSystems = new List<StarSystem>(); // Initialize the list
		}

		/// <summary>
		/// Add star system to galaxy.
		/// </summary>
		public void AddStarSystem(StarSystem star)
		{
			StarSystems.Add(star);
		}
	}
}
