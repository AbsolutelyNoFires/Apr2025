﻿using Neb25.Core.Galaxy;

namespace Neb25.Core.Utils
{
	public class GameFunctions
	{

		/// <summary>
		/// Returns the N number of nearest systems in galactic space from an origin starsystem,
		/// that are also within a specified radius.
		/// Uses squared distance comparison initially for efficiency, then calculates Sqrt only when needed.
		/// </summary>
		/// <param name="originStarSystem">The origin for the search.</param>
		/// <param name="NToReturn">The maximum number of nearest systems to return.</param>
		/// <param name="withinRadius">The maximum search radius from the origin system.</param>
		/// <returns>A dictionary where keys are nearby StarSystems (up to NToReturn) and values are their actual distance from the originStarSystem, sorted by distance.</returns>
		public static Dictionary<StarSystem, double> NearbyStarSystems(StarSystem originStarSystem, int NToReturn, double withinRadius)
		{
			// Use squared distance for initial filtering to avoid unnecessary Sqrt calculations
			double withinRadiusSquared = withinRadius * withinRadius;
			var systemsWithinRadius = new List<KeyValuePair<StarSystem, double>>(); // Use a list to store potential candidates with squared distance

			// Ensure ParentGalaxy and StarSystems are not null before iterating
			if (originStarSystem?.ParentGalaxy?.StarSystems == null)
			{
				// Or throw an exception, depending on desired behavior
				return [];
			}

			foreach (StarSystem thisSys in originStarSystem.ParentGalaxy.StarSystems)
			{
				// Skip the origin system itself
				if (thisSys == originStarSystem)
				{
					continue;
				}

				float diffX = thisSys.GalacticPosition.X - originStarSystem.GalacticPosition.X;
				float diffY = thisSys.GalacticPosition.Y - originStarSystem.GalacticPosition.Y;
				float diffZ = thisSys.GalacticPosition.Z - originStarSystem.GalacticPosition.Z;

				// Calculate squared distance
				double distSq = (diffX * diffX) + (diffY * diffY) + (diffZ * diffZ);

				// Filter by squared radius
				if (distSq < withinRadiusSquared)
				{
					// Add the system and its *squared* distance for now
					systemsWithinRadius.Add(new KeyValuePair<StarSystem, double>(thisSys, distSq));
				}
			}

			// Now, sort the systems within the radius by their SQUARED distance (result is the same as sorting by actual distance)
			// Take the nearest NToReturn
			// Then, calculate the actual distance (Sqrt) only for the ones we are returning
			// And convert back to a Dictionary

			Dictionary<StarSystem, double> result = systemsWithinRadius
				.OrderBy(kvp => kvp.Value) // Sort by squared distance (ascending)
				.Take(NToReturn)           // Get the top N results
				.ToDictionary(              // Convert back to a dictionary
					kvp => kvp.Key,        // Key is the StarSystem
					kvp => Math.Sqrt(kvp.Value) // Value is the actual distance (now we calculate Sqrt)
				);

			return result;
		}


		/// <summary>
		/// Helper method to sample from a Gaussian (normal) distribution.
		/// Uses the Box-Muller transform.
		/// </summary>
		/// <param name="random">Random number generator.</param>
		/// <param name="mean">Mean of the distribution.</param>
		/// <param name="stdDev">Standard deviation of the distribution.</param>
		/// <returns>A random sample from the distribution.</returns>
		public static double SampleGaussian(Random random, double mean, double stdDev)
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

		/// <summary>
		/// Performs a breadth-first search from the origin star system.
		/// </summary>
		/// <param name="originStarSystem">The origin star system.</param>
		/// <param name="maxJumps">Maximum number of jumps from origin</param>
		/// <returns>A dictionary where keys are reachable StarSystems and values are their shortest distance (in jumps) from the startSystem.</returns>
		/// <exception cref="ArgumentNullException">Thrown if startSystem is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if maxJumps is negative.</exception>
		public static Dictionary<StarSystem, int> BreadthFirstFromOriginSystem(StarSystem originStarSystem, int maxJumps)
		{
			if (originStarSystem == null)
			{
				throw new ArgumentNullException(nameof(originStarSystem));
			}
			if (maxJumps < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(maxJumps), "Maximum jumps cannot be negative.");
			}

			// Stores the minimum distance found so far for each visited system.
			Dictionary<StarSystem, int> visitedSystems = [];

			// Queue for BFS: Stores tuples of (SystemToVisit, CurrentDistance)
			var queue = new Queue<(StarSystem system, int distance)>();

			// Start BFS from the initial system
			visitedSystems[originStarSystem] = 0;
			queue.Enqueue((originStarSystem, 0));

			while (queue.Count > 0)
			{
				var (currentSystem, currentDistance) = queue.Dequeue();

				// If we've reached the maximum jump distance, don't explore further from this system.
				if (currentDistance >= maxJumps)
				{
					continue;
				}

				// Explore neighbors through jump sites
				foreach (var jumpSite in currentSystem.JumpSites)
				{
					// Check if the jump site has a valid partner and the partner belongs to a system
					if (jumpSite.Partner?.ParentStarSystem != null)
					{
						StarSystem neighbourSystem = jumpSite.Partner.ParentStarSystem;
						int newDistance = currentDistance + 1;

						// If we haven't visited the neighbor OR found a shorter path
						// (BFS naturally finds shortest paths first, so the second condition
						// is usually only relevant in graphs with varying edge weights, but included for completeness)
						if (!visitedSystems.ContainsKey(neighbourSystem) || newDistance < visitedSystems[neighbourSystem])
						{
							visitedSystems[neighbourSystem] = newDistance;
							queue.Enqueue((neighbourSystem, newDistance));
						}
					}
					// Optional: Log or handle cases where jumpSite.Partner or Partner.ParentStarSystem is null if needed.
					// else { Console.WriteLine($"Warning: JumpSite {jumpSite.Id} in {currentSystem.Name} has no valid partner link."); }
				}
			}

			return visitedSystems;

		}
	}
}
