using System;
using System.Numerics;

namespace Neb25.Core.Galaxy.Generators
{
	public class JumpSiteGenerator
	{
		public static JumpSite GenerateJumpSite(StarSystem parentStarSystem, Random seed)
		{
			int countOfParentStarSystemJps = parentStarSystem.JumpSites.Count;
			JumpSite newJumpsite = new JumpSite(parentStarSystem);
			newJumpsite.Number = countOfParentStarSystemJps;
			float randomX = 0f;
			float randomY = 0f;
			float max_au = 1f;
			if (parentStarSystem.PrimaryStar is not null)
			{
				if (parentStarSystem.PrimaryStar.Type == "Red Dwarf")
				{
					max_au = 3f;
				}
				else if (parentStarSystem.PrimaryStar.Type == "Yellow Dwarf")
				{
				    max_au = 5f;
				}
			}
			double angle = seed.NextDouble() * 2 * Math.PI;
			double distance = Math.Sqrt(seed.NextDouble()) * max_au;
			randomX = (float)(distance * Math.Cos(angle));
			randomY = (float)(distance * Math.Sin(angle));

			newJumpsite.Position = new Vector3 { X = randomX, Y = randomY, Z = 0 };
			return newJumpsite;
		}
	}
}
