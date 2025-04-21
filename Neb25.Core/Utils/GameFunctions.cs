using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Neb25.Core.Galaxy;

namespace Neb25.Core.Utils
{
	public class GameFunctions
	{

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
