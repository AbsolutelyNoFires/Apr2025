using Neb25.Core.Utils;
using System.Numerics;
using System.Diagnostics;
using System.Collections.Generic; // Required for List, Dictionary, HashSet, Queue
using System.Linq;             // Required for Linq operations like FirstOrDefault, Any, OrderBy, Take, Select, ToList, ToDictionary

namespace Neb25.Core.Galaxy.Generators
{

	public class GalaxyGenerator
	{
		public static Galaxy GenerateGalaxy(GameSettingsObject gameSettings)
		{
			int seedNum = gameSettings.SeedNum;
			int numSys = gameSettings.NumberOfStarSystems;

			Random seed = new(seedNum);
			Galaxy newGalaxy = new Galaxy(seed); // Galaxy constructor now only needs seed
			newGalaxy.GameSettings = gameSettings; // Assign game settings after creation
			newGalaxy.JumpConnections = new List<JumpConnection>(); // Initialize JumpConnections list

			Debug.WriteLine("Generating star systems...");
			if (numSys <= 0)
			{
				Debug.WriteLine("Warning: NumberOfStarSystems is zero or negative. No systems generated.");
				return newGalaxy; // Return early if no systems are requested
			}
			for (int i = 0; i < numSys; i++)
			{
				StarSystemGenerator.GenerateStarSystem(newGalaxy, seed);
			}
			Debug.WriteLine($"Generated {newGalaxy.StarSystems.Count} star systems.");


			// Ensure there are systems to connect before proceeding
			if (newGalaxy.StarSystems.Count > 1)
			{
				Debug.WriteLine("Generating initial jump connections...");
				GenerateInitialConnections(newGalaxy, seed);
				Debug.WriteLine("Ensuring galaxy connectivity...");
				EnsureGalaxyConnectivity(newGalaxy, seed);
			}
			else
			{
				Debug.WriteLine("Skipping connection generation as there are not enough star systems.");
			}


			Debug.WriteLine("--- Galaxy Generation Complete ---");

			return newGalaxy;
		}

		/// <summary>
		/// Generates initial jump connections between nearby star systems based on available jump sites.
		/// Attempts to connect each system to its closest neighbors first.
		/// </summary>
		/// <param name="galaxy">The galaxy object containing star systems and jump sites.</param>
		/// <param name="seed">Random number generator.</param>
		private static void GenerateInitialConnections(Galaxy galaxy, Random seed)
		{
			if (galaxy?.StarSystems == null || galaxy.StarSystems.Count < 2)
			{
				Debug.WriteLine("Not enough star systems to generate connections.");
				return; // Cannot generate connections with less than 2 systems
			}


			int kNearest = Constants.KNearest; // How many nearest neighbors to consider
			float maxDistMultiplier = Constants.MaxConnectionDistanceMultiplier; // Max distance relative to nearest


			foreach (var originSystem in galaxy.StarSystems)
			{
				// Find potential neighbors
				// Note: NearbyStarSystems returns a dictionary sorted by distance
				var neighbors = GameFunctions.NearbyStarSystems(originSystem, kNearest, 50); // Is there some max distance between stars beyond which a jump connection is impossible? maybe differs on star type etc. 50 here is galactic map units, maybe light years


				if (!neighbors.Any()) continue; // Skip if no neighbors found within radius


				double closestNeighborDist = neighbors.First().Value; // Distance to the absolute closest neighbor
				double maxConnectionDistance = closestNeighborDist * maxDistMultiplier;


				// Get available (unpartnered) jump sites in the origin system
				var availableOriginSites = originSystem.JumpSites.Where(js => !js.HasPartner).ToList();


				foreach (var originSite in availableOriginSites)
				{
					if (originSite.HasPartner) continue; // Double-check in case it got partnered in this loop


					// Iterate through neighbors, checking distance constraint
					foreach (var neighborPair in neighbors)
					{
						StarSystem neighborSystem = neighborPair.Key;
						double distanceToNeighbor = neighborPair.Value;


						if (distanceToNeighbor > maxConnectionDistance) break; // Since neighbors are sorted, no need to check further


						// Find an available jump site in the neighbor system
						var availableNeighborSite = neighborSystem.JumpSites
																 .FirstOrDefault(js => !js.HasPartner);


						if (availableNeighborSite != null)
						{
							// Found a pair! Create the connection.
							List<JumpSite> connectionPair = new List<JumpSite> { originSite, availableNeighborSite };
							try
							{
								// Note: Need to pass the seed to the generator if it needs it (e.g., for ancient gates)
								JumpConnectionGenerator.GenerateJumpConnection(connectionPair, seed); // Pass seed
								Debug.WriteLine($"Created connection: {originSystem.Name} ({originSite.Number}) <-> {neighborSystem.Name} ({availableNeighborSite.Number})");


								// This origin site is now partnered, move to the next available origin site
								goto NextOriginSite;
							}
							catch (ArgumentException argEx)
							{
								// Log if a connection fails (e.g., same system, already partnered - though we check)
								Debug.WriteLine($"ArgumentException while creating connection: {argEx.Message}");
							}
							catch (Exception ex)
							{
								// Catch unexpected errors during connection generation
								Debug.WriteLine($"Error creating connection between {originSystem.Name} and {neighborSystem.Name}: {ex.Message}");
							}
						}
					}
				NextOriginSite:; // Label to break inner loop and continue with the next origin site
				}
			}
			Debug.WriteLine($"Initial connection generation complete. Total connections: {galaxy.JumpConnections?.Count ?? 0}");
		}


		/// <summary>
		/// Ensures the entire galaxy is a single connected graph. If multiple disconnected
		/// components exist, it iteratively adds connections between the closest systems
		/// of different components until only one component remains.
		/// </summary>
		/// <param name="galaxy">The galaxy object.</param>
		/// <param name="seed">Random number generator.</param>
		private static void EnsureGalaxyConnectivity(Galaxy galaxy, Random seed)
		{
			if (galaxy?.StarSystems == null || galaxy.StarSystems.Count < 2)
			{
				Debug.WriteLine("Not enough star systems for connectivity check.");
				return; // No need to check connectivity for 0 or 1 system
			}


			int maxAttempts = galaxy.StarSystems.Count; // Safety break for potential infinite loops
			int attempts = 0;


			while (attempts < maxAttempts)
			{
				attempts++;
				List<HashSet<StarSystem>> components = FindConnectedComponents(galaxy.StarSystems);


				if (components.Count <= 1)
				{
					Debug.WriteLine($"Galaxy is fully connected with {components.Count} component(s).");
					break; // Galaxy is connected
				}


				Debug.WriteLine($"Galaxy is fragmented into {components.Count} components. Attempting to bridge...");


				// Find the closest pair of systems belonging to different components
				StarSystem system1 = null;
				StarSystem system2 = null;
				double minDistanceSq = double.MaxValue;


				// Compare systems between all pairs of components
				for (int i = 0; i < components.Count; i++)
				{
					for (int j = i + 1; j < components.Count; j++) // Compare component i with component j
					{
						foreach (var s1 in components[i])
						{
							foreach (var s2 in components[j])
							{
								float distSq = Vector3.DistanceSquared(s1.GalacticPosition, s2.GalacticPosition);
								if (distSq < minDistanceSq)
								{
									minDistanceSq = distSq;
									system1 = s1;
									system2 = s2;
								}
							}
						}
					}
				}


				if (system1 != null && system2 != null)
				{
					Debug.WriteLine($"Found closest disconnected pair: {system1.Name} and {system2.Name} (Dist: {Math.Sqrt(minDistanceSq):F2}). Creating bridge connection.");
					// Create new jump sites in these systems
					JumpSite newSite1 = JumpSiteGenerator.GenerateJumpSite(system1, seed);
					JumpSite newSite2 = JumpSiteGenerator.GenerateJumpSite(system2, seed);


					// Connect them
					List<JumpSite> bridgePair = new List<JumpSite> { newSite1, newSite2 };
					try
					{
						JumpConnectionGenerator.GenerateJumpConnection(bridgePair, seed); // Pass seed
						Debug.WriteLine($"Successfully created bridge: {system1.Name} ({newSite1.Number}) <-> {system2.Name} ({newSite2.Number})");
					}
					catch (ArgumentException argEx)
					{
						Debug.WriteLine($"ArgumentException while creating bridge connection: {argEx.Message}");
						// Consider what to do if bridging fails - maybe try next closest pair?
					}
					catch (Exception ex)
					{
						Debug.WriteLine($"Error creating bridge connection between {system1.Name} and {system2.Name}: {ex.Message}");
					}
				}
				else
				{
					Debug.WriteLine("Error: Could not find a pair of systems to bridge components. Aborting connectivity assurance.");
					break; // Should not happen if components > 1, but safety break
				}
			}
			if (attempts >= maxAttempts)
			{
				Debug.WriteLine("Warning: Max attempts reached while ensuring galaxy connectivity. The galaxy might still be fragmented.");
			}
		}




		/// <summary>
		/// Finds all disconnected subgraphs (connected components) in the galaxy.
		/// </summary>
		/// <param name="allSystems">A list of all star systems in the galaxy.</param>
		/// <returns>A list where each element is a HashSet containing the StarSystems of a single connected component.</returns>
		private static List<HashSet<StarSystem>> FindConnectedComponents(List<StarSystem> allSystems)
		{
			List<HashSet<StarSystem>> components = new List<HashSet<StarSystem>>();
			HashSet<StarSystem> visitedSystems = new HashSet<StarSystem>();


			foreach (var startSystem in allSystems)
			{
				if (!visitedSystems.Contains(startSystem))
				{
					// Start a new BFS from this unvisited system to find its component
					HashSet<StarSystem> currentComponent = new HashSet<StarSystem>();
					Queue<StarSystem> queue = new Queue<StarSystem>();


					queue.Enqueue(startSystem);
					visitedSystems.Add(startSystem);
					currentComponent.Add(startSystem);


					while (queue.Count > 0)
					{
						var currentSystem = queue.Dequeue();


						// Check neighbors through existing jump connections (use ConnectedStarSystems property)
						// Ensure ConnectedStarSystems is correctly populated by JumpConnectionGenerator
						if (currentSystem.ConnectedStarSystems != null)
						{
							foreach (var neighborSystem in currentSystem.ConnectedStarSystems)
							{
								if (neighborSystem != null && !visitedSystems.Contains(neighborSystem))
								{
									visitedSystems.Add(neighborSystem);
									currentComponent.Add(neighborSystem);
									queue.Enqueue(neighborSystem);
								}
							}
						}
						else
						{
							// This suggests ConnectedStarSystems might not be initialized or populated correctly.
							Debug.WriteLine($"Warning: ConnectedStarSystems list is null for {currentSystem.Name}");
						}
					}
					components.Add(currentComponent);
				}
			}
			return components;
		}
	}
}