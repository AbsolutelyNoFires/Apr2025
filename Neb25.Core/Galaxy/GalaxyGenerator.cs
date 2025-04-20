using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neb25.Core
{
	/// <summary>
	/// Responsible for generating the game's galaxy.
	/// </summary>
	public static class GalaxyGenerator // Making it static for simplicity, could be instance-based
	{
		private static Random random = new Random(); // Random number generator

		/// <summary>
		/// Generates a new galaxy based on specified parameters.
		/// </summary>
		/// <param name="numberOfStars">The number of star systems to create.</param>
		/// <returns>A Galaxy object.</returns>
		public static Galaxy GenerateGalaxy(int numberOfStars)
		{
			Console.WriteLine($"Generating galaxy with {numberOfStars} stars..."); // Debug output
			Galaxy galaxy = new Galaxy("New1"); // Create a new galaxy instance
			return(galaxy);

		}

	}
}