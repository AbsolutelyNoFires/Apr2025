using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Neb25.Core.Galaxy
{
	/// <summary>
	/// Generates the game's galaxy, including star systems and their positions.
	/// </summary>
	public static class GalaxyGenerator
	{

		/// <summary>
		/// Generates a new galaxy based on the specified parameters.
		/// </summary>
		/// <param name="numSystems">The number of star systems to generate.</param>
		/// <param name="galaxyRadius">The approximate radius of the galaxy.</param>
		/// <returns>A newly generated Galaxy object.</returns>
		public static Galaxy GenerateGalaxy(int numSystems, float galaxyRadius)
		{
			var galaxy = new Galaxy("My new galaxy");
			var random = new Random(); // Use a single Random instance

			// Basic spiral galaxy generation attempt
			// More sophisticated algorithms could create better shapes (e.g., density waves)
			int numArms = random.Next(2, 5); // Number of spiral arms
			float armSeparationDistance = (float)(2 * Math.PI / numArms);
			float armOffsetMax = 0.5f; // How much stars deviate from the arm centerline
			float coreRadius = galaxyRadius * 0.1f; // Radius of the central core/bulge

			for (int i = 0; i < numSystems; i++)
			{
				// Determine position using a simple spiral galaxy model
				float angle = (float)(random.NextDouble() * 2 * Math.PI); // Random angle
				float dist = (float)(random.NextDouble() * galaxyRadius); // Random distance from center

				// Introduce spiral arm structure
				float armIndex = (angle / armSeparationDistance);
				float armAngle = (float)Math.Floor(armIndex) * armSeparationDistance;
				float armOffset = (armIndex - (float)Math.Floor(armIndex) - 0.5f) * armSeparationDistance * armOffsetMax;

				// Adjust angle for spiral effect (tighter spiral near center)
				float spiralFactor = (float)Math.Pow(dist / galaxyRadius, 0.5); // Controls how much arms curve
				float rotatedAngle = armAngle + armOffset + spiralFactor * (float)Math.PI * 0.5f; // Add rotation based on distance

				// Calculate initial X, Y based on spiral logic
				float x = dist * (float)Math.Cos(rotatedAngle);
				float y = dist * (float)Math.Sin(rotatedAngle);

				// Add some thickness/height (Z-coordinate) - flatter towards the edges
				// Gaussian distribution for Z, centered around 0
				float zStdDev = galaxyRadius * 0.05f * (1.0f - (dist / galaxyRadius)); // Standard deviation decreases with distance
				float z = (float)(SampleGaussian(random, 0, zStdDev));

				// Create the star system
				var system = new StarSystem($"System {i + 1}")
				{
					Position = new Vector3(x, y, z) // Assign 3D position
				};

				// Generate basic details for the star system (like planets)
				GenerateStarSystemDetails(system, random);

				galaxy.StarSystems.Add(system);
			}

			return galaxy;
		}





		/// <summary>
		/// Populates a StarSystem with basic details, like planets.
		/// </summary>
		/// <param name="system">The star system to populate.</param>
		/// <param name="random">The random number generator to use.</param>
		private static void GenerateStarSystemDetails(StarSystem system, Random random)
		{
			// Simple planet generation: Add a random number of planets
			int numPlanets = random.Next(0, 9); // 0 to 8 planets

			for (int i = 0; i < numPlanets; i++)
			{
				// TODO: Add more varied planet generation (types, sizes, resources, moons etc.)
				var planet = new Planet($"Planet {i + 1}");
				system.Planets.Add(planet);
			}
			// TODO: Add star type, asteroids, etc.
		}

		/// <summary>
		/// Helper method to sample from a Gaussian (normal) distribution.
		/// Uses the Box-Muller transform.
		/// </summary>
		/// <param name="random">Random number generator.</param>
		/// <param name="mean">Mean of the distribution.</param>
		/// <param name="stdDev">Standard deviation of the distribution.</param>
		/// <returns>A random sample from the distribution.</returns>
		private static double SampleGaussian(Random random, double mean, double stdDev)
		{
			// Ensure stdDev is positive
			stdDev = Math.Max(0.001, stdDev); // Avoid division by zero or negative stddev

			double u1 = 1.0 - random.NextDouble(); // Uniform(0,1] random doubles
			double u2 = 1.0 - random.NextDouble();
			double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
								   Math.Sin(2.0 * Math.PI * u2); // Random normal(0,1)
			double randNormal = mean + stdDev * randStdNormal; // Random normal(mean,stdDev^2)
			return randNormal;
		}

	}
}