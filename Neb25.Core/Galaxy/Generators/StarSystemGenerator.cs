namespace Neb25.Core.Galaxy.Generators
{
	public class StarSystemGenerator
	{
		public static StarSystem GenerateStarSystem(Galaxy galaxy, Random seed)
		{
			StarSystem newStarSystem = new StarSystem(galaxy);
			int numStars = 1;
			for (int i = 0; i < numStars; i++)
			{
				Star newStar = StarGenerator.GenerateStar(newStarSystem, seed);
			}
			return newStarSystem;
		}
	}
}
