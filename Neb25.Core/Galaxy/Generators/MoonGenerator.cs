﻿namespace Neb25.Core.Galaxy.Generators
{
	public class MoonGenerator
	{
		public static Moon GenerateMoon(Planet parentPlanet, Random seed)
		{
			Moon newMoon = new(parentPlanet);
			
			return newMoon;
		}
	}
}
