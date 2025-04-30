using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neb25.Core.Galaxy.Generators
{
	public class JumpSiteGenerator
	{
		public JumpSite GenerateJumpsite(StarSystem parentStarSystem, Random seed)
		{
			int countOfParentStarSystemJps = parentStarSystem.JumpSites.Count;
			JumpSite newJumpsite = new JumpSite(parentStarSystem);
			newJumpsite.Number = countOfParentStarSystemJps;
			return newJumpsite;
		}
	}
}
