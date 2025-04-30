using Neb25.Core.Utils;
using System.Numerics;
using System.Diagnostics;

namespace Neb25.Core.Galaxy.Generators
{

	public class GalaxyGenerator
	{
		public static Galaxy GenerateGalaxy(GameSettingsObject gameSettings)
		{
			int seedNum = gameSettings.SeedNum;
			int numSys = gameSettings.NumberOfStarSystems;

			int numArms = gameSettings.GalaxyArms;
			float armSeparationDistance = (float)(2 * Math.PI / numArms);
			float armOffsetMax = 0.5f;
			float galaxyRadius = 1000f;

			Random seed = new(seedNum);
			Galaxy newGalaxy = new Galaxy(seed);

			for (int i = 0; i < numSys; i++) {

				float angle = (float)(seed.NextDouble() * 2 * Math.PI);
				float dist = (float)(seed.NextDouble() * galaxyRadius);
				float armIndex = angle / armSeparationDistance;
				float armAngle = (float)Math.Floor(armIndex) * armSeparationDistance;
				float armOffset = (armIndex - (float)Math.Floor(armIndex) - 0.5f) * armSeparationDistance * armOffsetMax;
				float spiralFactor = (float)Math.Pow(dist / galaxyRadius, 0.5);
				float rotatedAngle = armAngle + armOffset + spiralFactor * (float)Math.PI * 0.5f;
				float x = dist * (float)Math.Cos(rotatedAngle);
				float y = dist * (float)Math.Sin(rotatedAngle);
				float zStdDev = galaxyRadius * 0.05f * (1.0f - dist / galaxyRadius);
				float z = (float)Core.Utils.GameFunctions.SampleGaussian(seed, 0, zStdDev);


				StarSystem newStarSystem = StarSystemGenerator.GenerateStarSystem(newGalaxy, seed);
				newStarSystem.Position = new Vector3(x, y, z);
				
				newGalaxy.StarSystems.Add(newStarSystem);

			}


			// for number sys, generate a star system




			return newGalaxy;

			// ensure connectivity
			// populate it 



		}

		/*

		/// <summary>
		/// Iterates the galaxy for disconnect jump points, and finds a partner for each.
		/// </summary>
		/// <param name="galaxy">The galaxy containing the star systems.</param>
		/// <param name="random">The random number generator.</param>
		private static void GenerateConnections(Galaxy galaxy, Random seed)
		{
			List<StarSystem> allSystems = galaxy.StarSystems;
			if (allSystems.Count < 2) return;

			foreach (var currentSystem in allSystems)
			{
				// Find K nearest neighbors (excluding self) once per system
				var neighbors = allSystems
					.Where(s => s != currentSystem)
					.Select(s => new { System = s, DistanceSq = Vector3.DistanceSquared(currentSystem.Position, s.Position) })
					.OrderBy(s => s.DistanceSq)
					.Take(Constants.KNearest)
					.ToList();

				// Determine a max distance based on the closest neighbor
				float maxDistSq = neighbors.Any() ? neighbors.First().DistanceSq * Constants.MaxConnectionDistanceMultiplier * Constants.MaxConnectionDistanceMultiplier : float.MaxValue;

				// Get available (unpartnered) jump sites in the current system
				// Shuffle to add some randomness to which site gets picked first
				var availableSites = currentSystem.JumpSites.Where(js => !js.HasPartner).OrderBy(x => seed.Next()).ToList();

				// Try to connect each available site of the current system
				foreach (var site in availableSites)
				{
					// Double-check if the site got partnered in a previous iteration (by another system connecting to it)
					if (site.HasPartner) continue;

					// Find the best available partner among neighbors within the distance limit
					JumpSite? bestPartnerSite = null;
					StarSystem? targetSystem = null;
					float bestDistSq = maxDistSq; // Start with max allowed distance

					// Iterate through neighbors sorted by distance
					foreach (var neighborInfo in neighbors)
					{
						
						//List<StarSystems> alreadyConnected = site.ParentStarSystem.JumpSites
						if (neighborInfo.System.ConnectedStarSystems.Contains(currentSystem)) continue; // Skip if already connected

						// Check distance limit (redundant if neighbors list is already limited, but safe)
						if (neighborInfo.DistanceSq > bestDistSq) continue; // Don't consider if further than current best or max allowed

						var potentialPartnerSystem = neighborInfo.System;

						// Find the first available partner site in this neighbor
						var partnerSite = potentialPartnerSystem.JumpSites
							.FirstOrDefault(pjs => !pjs.HasPartner);

						// If an available site is found in this neighbor
						if (partnerSite != null)
						{
							// Found a potential partner. Store it as the current best candidate.
							// We continue searching neighbors to ensure we pick the closest one.
							bestPartnerSite = partnerSite;
							targetSystem = potentialPartnerSystem;
							bestDistSq = neighborInfo.DistanceSq;
							// *** REMOVED the 'break;' here ***
							// This allows checking further neighbors to potentially find an even closer one
							// with an available site, although since neighbors are sorted, the first one found
							// *should* be the closest. More importantly, removing break ensures the outer loop
							// for 'site in availableSites' continues trying to connect OTHER sites from the currentSystem.
						}
					} // End neighbor loop

					// If a suitable partner was found after checking all relevant neighbors
					if (bestPartnerSite != null && targetSystem != null)
					{
						// Check one last time if the chosen partner site is still available
						// (it might have been taken by another system connecting in the meantime)
						if (!bestPartnerSite.HasPartner)
						{
							// Create the connection
							site.Partner = bestPartnerSite;
							site.HasPartner = true;
							bestPartnerSite.Partner = site;
							bestPartnerSite.HasPartner = true;
							targetSystem.ConnectedStarSystems.Add(currentSystem); // Add the connection to the target system
							currentSystem.ConnectedStarSystems.Add(targetSystem); // Add the connection to the current system
						}
					}
				} // End loop through available sites for the current system
			} // End loop through all systems
		}

		
		/// <summary>
		/// Ensures the graph of star systems is fully connected by adding links if necessary.
		/// </summary>
		/// <param name="galaxy">The galaxy of all star systems to check.</param>
		/// <param name="random">The random number generator.</param>
		private static void EnsureConnectivity(Galaxy galaxy, Random random)
		{
			var allSystems = galaxy.StarSystems;
			if (allSystems.Count < 2) return;

			var visited = new HashSet<StarSystem>();
			var queue = new Queue<StarSystem>();
			var connectedComponents = new List<List<StarSystem>>();

			// 1. Identify Connected Components using BFS
			foreach (var startSystem in allSystems)
			{
				if (!visited.Contains(startSystem))
				{
					var currentComponent = new List<StarSystem>();
					queue.Clear();

					queue.Enqueue(startSystem);
					visited.Add(startSystem);
					currentComponent.Add(startSystem);

					while (queue.Count > 0)
					{
						var current = queue.Dequeue();

						// Find neighbors via jump site partners
						var neighbors = current.JumpSites
							.Where(js => js.HasPartner && js.Partner != null) // Ensure partner exists
							.Select(js => js.Partner!.ParentStarSystem) // Partner should not be null if HasPartner is true
							.Distinct();

						foreach (var neighbor in neighbors)
						{
							if (!visited.Contains(neighbor))
							{
								visited.Add(neighbor);
								queue.Enqueue(neighbor);
								currentComponent.Add(neighbor);
							}
						}
					}
					connectedComponents.Add(currentComponent);
				}
			}

			Debug.WriteLine($"Found {connectedComponents.Count} connected component(s).");

			// 2. Connect Components if more than one exists
			while (connectedComponents.Count > 1)
			{
				// Find the two closest systems between the first component and any other component
				var component1 = connectedComponents[0];
				StarSystem? bestSys1 = null;
				StarSystem? bestSys2 = null;
				float minDistSq = float.MaxValue;
				int bestOtherComponentIndex = -1;

				// Find the closest pair of systems between component 0 and component i
				for (int i = 1; i < connectedComponents.Count; i++)
				{
					var component2 = connectedComponents[i];
					foreach (var sys1 in component1)
					{
						foreach (var sys2 in component2)
						{
							float distSq = Vector3.DistanceSquared(sys1.Position, sys2.Position);
							if (distSq < minDistSq)
							{
								minDistSq = distSq;
								bestSys1 = sys1;
								bestSys2 = sys2;
								bestOtherComponentIndex = i;
							}
						}
					}
				}

				if (bestSys1 != null && bestSys2 != null && bestOtherComponentIndex != -1)
				{
					// Try to find available jump sites OR CREATE NEW ONES
					var site1 = bestSys1.JumpSites.FirstOrDefault(js => !js.HasPartner);
					var site2 = bestSys2.JumpSites.FirstOrDefault(js => !js.HasPartner);

					// --- Force Creation Logic ---
					if (site1 == null)
					{
						Debug.WriteLine($"Warning: No available JumpSite in {bestSys1.Name}. Creating one for connectivity.");
						// Create a new JumpSite (position can be simple for now)
						site1 = new JumpSite(bestSys1, bestSys1.JumpSites.Count + 1) { Position = Vector3.Zero }; // Assign basic position
						bestSys1.JumpSites.Add(site1); // Add to the system's list
					}
					if (site2 == null)
					{
						Debug.WriteLine($"Warning: No available JumpSite in {bestSys2.Name}. Creating one for connectivity.");
						// Create a new JumpSite
						site2 = new JumpSite(bestSys2, bestSys2.JumpSites.Count + 1) { Position = Vector3.Zero }; // Assign basic position
						bestSys2.JumpSites.Add(site2); // Add to the system's list
					}
					// --- End Force Creation Logic ---


					// Check again (should always pass now if creation worked)
					if (site1 != null && site2 != null && !site1.HasPartner && !site2.HasPartner)
					{
						// Connect them
						site1.Partner = site2;
						site1.HasPartner = true;
						site2.Partner = site1;
						site2.HasPartner = true;
						Debug.WriteLine($"Forced connection between {bestSys1.Name} and {bestSys2.Name} for connectivity.");

						// Merge the components
						component1.AddRange(connectedComponents[bestOtherComponentIndex]);
						connectedComponents.RemoveAt(bestOtherComponentIndex);
						Debug.WriteLine($"Merged components. Remaining components: {connectedComponents.Count}");
					}
					else
					{
						// This should ideally not be reached if creation logic is sound
						Debug.WriteLine($"Error: Failed to force connection between {bestSys1.Name} and {bestSys2.Name} AFTER attempting creation. Check logic.");
						break; // Break to avoid potential infinite loop
					}
				}
				else
				{
					// Should not happen in a normal scenario unless components are empty or logic error
					Debug.WriteLine($"Error: Could not find closest systems between components. Aborting connectivity merge.");
					break;
				}
			}

			if (connectedComponents.Count == 1)
			{
				Debug.WriteLine("Galaxy connectivity ensured.");
			}
			else
			{
				Debug.WriteLine($"Warning: Galaxy connectivity check finished with {connectedComponents.Count} components.");
			}

			// Optional: Add more connections if systems have fewer than MinNeighbors?
			// This part is less critical than ensuring full connectivity.
		}

		*/



	}
}