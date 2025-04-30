namespace Neb25.Core.Galaxy.Generators
{
	public class JumpConnectionGenerator
	{
		public JumpConnection GenerateJumpConnection(List<JumpSite> jumpSitePair, Random seed) {
			if (jumpSitePair == null || jumpSitePair.Count != 2) throw new ArgumentException("Tried to connect more than two jump sites into a jump connections");

			JumpConnection newJumpConnection = new JumpConnection(jumpSitePair);

			JumpSite fromJP = jumpSitePair[0];
			JumpSite toJP = jumpSitePair[1];
			fromJP.Partner = toJP;
			toJP.Partner = fromJP;
			
			double chanceOfAncientJumpGate = seed.Next(1);
			bool hasAncientJumpGate = false;
			if (chanceOfAncientJumpGate > 0.97) { // 2% one in fifty gates
				hasAncientJumpGate = true;
			}
			
			fromJP.HasGateBuilt = hasAncientJumpGate;
			toJP.HasGateBuilt = hasAncientJumpGate;

			fromJP.ParentStarSystem.ConnectedStarSystems = toJP.ParentStarSystem.ConnectedStarSystems;
			toJP.ParentStarSystem.ConnectedStarSystems = fromJP.ParentStarSystem.ConnectedStarSystems;

			return newJumpConnection;


		}
	}
}
