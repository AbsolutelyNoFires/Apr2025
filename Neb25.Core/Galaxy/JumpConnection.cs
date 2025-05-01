namespace Neb25.Core.Galaxy
{
	public class JumpConnection
	{
		public Guid Id { get; set; }

		/// <summary>
		/// List of two jump points in any order.
		/// </summary>
		public List<JumpSite> JumpSites {  get; set; }

		/// <summary>
		/// List of the two linked systems in any order.
		/// </summary>
		public List<StarSystem> StarSystems { get; set; }

		public JumpConnection(List<JumpSite> jumpSites) {
			Id = Guid.NewGuid();
			JumpSites = jumpSites;
			List<StarSystem> unpackedSystems = new();
			for (int i = 0; i < JumpSites.Count; i++) 
			{
				unpackedSystems.Add(jumpSites[i].ParentStarSystem);
			}
			StarSystems = unpackedSystems;
			jumpSites[0].ParentStarSystem.ParentGalaxy.JumpConnections.Add(this);
		}
	}
}
