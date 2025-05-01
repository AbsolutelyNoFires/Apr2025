using System.Numerics;

namespace Neb25.Core.Galaxy.Generators
{
	public class StarSystemGenerator
	{
		public static StarSystem GenerateStarSystem(Galaxy galaxy, Random seed)
		{
			StarSystem newStarSystem = new StarSystem(galaxy);

			// generation logic
			int numStars = 1; //binaries, trinaries
			int numJumpSites = 6; //per sys
			float galaxyRadius = 1000f;


			for (int i = 0; i < numStars; i++)
			{
				Star newStar = StarGenerator.GenerateStar(newStarSystem, seed);
			}



			int numArms = galaxy.GameSettings.GalaxyArms;
			float armSeparationDistance = (float)(2 * Math.PI / numArms);
			float armOffsetMax = 0.5f;
			
			float angle = (float)(seed.NextDouble() * 2 * Math.PI);
			float dist = (float)(seed.NextDouble() * galaxyRadius);
			float armIndex = angle / armSeparationDistance;
			float armAngle = (float)Math.Floor(armIndex) * armSeparationDistance;
			float armOffset = (armIndex - (float)Math.Floor(armIndex) - 0.5f) * armSeparationDistance * armOffsetMax;
			float spiralFactor = (float)Math.Pow(dist / galaxyRadius, 0.5);
			float rotatedAngle = armAngle + armOffset + spiralFactor * (float)Math.PI * 0.5f;
			float zStdDev = galaxyRadius * 0.05f * (1.0f - dist / galaxyRadius);
			
			newStarSystem.Position = new Vector3
			{
				X = dist * (float)Math.Cos(rotatedAngle),
				Y = dist * (float)Math.Sin(rotatedAngle),
				Z = (float)Core.Utils.GameFunctions.SampleGaussian(seed, 0, zStdDev)
			};

			// now generate jump sites
			for (int i = 0; i < numJumpSites; i++)
			{
				JumpSite newJumpSite = JumpSiteGenerator.GenerateJumpSite(newStarSystem, seed);
			}

			return newStarSystem;
		}
	}
}
