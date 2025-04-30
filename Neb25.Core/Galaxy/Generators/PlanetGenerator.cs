namespace Neb25.Core.Galaxy.Generators
{
	public class PlanetGenerator
	{
		public static Planet GeneratePlanet(Star parentStar, Random seed)
		{
			Planet newPlanet = new(parentStar);
			parentStar.Planets.Add(newPlanet);
			parentStar.ParentStarSystem.Planets.Add(newPlanet);
			int numMoons = 1;
			for (int i = 0; i < numMoons; i++) {
				Moon moon = MoonGenerator.GenerateMoon(newPlanet, seed);
			}
			return newPlanet;
		}
	}
}
