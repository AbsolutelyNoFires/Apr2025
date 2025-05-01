using System.Numerics;

namespace Neb25.Core.Galaxy
{
	public class JumpSite
	{

		/// <summary>
		/// Unique identifier for the jump point.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Number JP in the star system (e.g., "Sol-1", "Sol-2").
		/// </summary>
		public int Number { get; set; }

		/// <summary>
		/// Parent star system 
		/// </summary>
		public StarSystem ParentStarSystem { get; set; }

		/// <summary>
		/// Whether or not a jump gate has been built by anyone.
		/// </summary>
		public bool HasGateBuilt { get; set; }

		/// <summary>
		/// Whether or not a partner jump point has already been asigned
		/// </summary>
		public bool HasPartner { get; set; }

		/// <summary>
		/// The partner jump point.
		/// </summary>
		public JumpSite? Partner { get; set; } = null;

		/// <summary>
		/// Gets or sets the 3D position of the jump point within the parent star system.
		/// Coordinates are relative to the system's primary star (0,0,0).
		/// </summary>
		public Vector3 Position { get; set; }
		/// <summary>
		/// Name of the JP, ie 'Star: Sol - JP 1'
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Initializes a new instance of the <see cref="StarSystem"/> class.
		/// </summary>
		/// <param name="name">The name of the star system.</param>
		public JumpSite(StarSystem thisParentStarSystem)
		{
			Id = Guid.NewGuid();
			ParentStarSystem = thisParentStarSystem;
			Number = ParentStarSystem.JumpSites.Count + 1;
			HasGateBuilt = false;
			HasPartner = false;
			Partner = null;
			Position = Vector3.Zero;
			Name = thisParentStarSystem.Name + " - JP " + Number.ToString();
			thisParentStarSystem.JumpSites.Add(this);
		}
	}
}
