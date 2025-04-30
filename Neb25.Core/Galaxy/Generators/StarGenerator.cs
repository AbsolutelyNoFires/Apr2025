namespace Neb25.Core.Galaxy.Generators
{
	public class StarGenerator
	{
		public static Star GenerateStar(StarSystem parentStarSystem, Random seed)
		{
			Star newStar = new Star(parentStarSystem);
			int numWorlds = 3;
			List<Planet> newPlanets = new();
			for (int i = 0; i < numWorlds; i++) {
				Planet newPlanet = PlanetGenerator.GeneratePlanet(newStar, seed);
			}
			parentStarSystem.Stars.Add(newStar);
			return newStar;
		}
	}
}
