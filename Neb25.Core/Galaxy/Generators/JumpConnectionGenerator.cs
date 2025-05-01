namespace Neb25.Core.Galaxy.Generators
{
	public class JumpConnectionGenerator
	{
		public static JumpConnection GenerateJumpConnection(List<JumpSite> jumpSitePair, Random seed) {

			// --- Input Validation ---
			if (seed == null)
			{
				throw new ArgumentNullException(nameof(seed), "Random seed cannot be null.");
			}
			if (jumpSitePair == null || jumpSitePair.Count != 2)
			{
				throw new ArgumentException("Jump connection requires exactly two jump sites.", nameof(jumpSitePair));
			}
			if (jumpSitePair[0] == null || jumpSitePair[1] == null)
			{
				throw new ArgumentException("Jump sites in the pair cannot be null.", nameof(jumpSitePair));
			}
			if (jumpSitePair[0] == jumpSitePair[1])
			{
				throw new ArgumentException("Cannot connect a jump site to itself.", nameof(jumpSitePair));
			}
			if (jumpSitePair[0].ParentStarSystem == jumpSitePair[1].ParentStarSystem)
			{
				// Optional: Decide if intra-system links are allowed. Throwing exception for now.
				// If allowed, remove this check.
				throw new ArgumentException("Cannot create jump connection between two sites in the same system.", nameof(jumpSitePair));
			}
			// --- End Validation ---
			
			JumpConnection newJumpConnection = new JumpConnection(jumpSitePair);

			JumpSite fromJP = jumpSitePair[0];
			JumpSite toJP = jumpSitePair[1];
			fromJP.Partner = toJP;
			fromJP.HasPartner = true;
			toJP.Partner = fromJP;
			toJP.HasPartner = true;
			fromJP.ParentStarSystem.ConnectedStarSystems.Add(toJP.ParentStarSystem);
			toJP.ParentStarSystem.ConnectedStarSystems.Add(fromJP.ParentStarSystem);

			double chanceOfAncientJumpGate = seed.Next(1);
			bool hasAncientJumpGate = false;
			if (chanceOfAncientJumpGate < 0.02) { 
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
