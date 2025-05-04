namespace Neb25.Core.Utils
{
	public class Constants
	{
		public const float GalaxyRadius = 1000f;
		// just some numbers we sometimes play with
		// related to how many jump points are in a system
		public const int MinSysJps = 1;
		public const int MaxSysJps = 64;
		public const int PrefMinSysJps = 2;
		public const int PrefMaxSysJps = 6;
		public const float BiasFactor = 0.95f; // in BiasFactor percent of cases, we'll be within the preferred range
		// related to jump linkage formation
		public const int KNearest = 15; // How many nearest neighbors to consider for potential links
		public const float MaxConnectionDistanceMultiplier = 2f; // Allow connections to stars N times as far as the closest neighbor
		
	}
}
