using Neb25.Core.Utils;

namespace Neb25.Core.Galaxy
{
	public class Galaxy
	{
		/// <summary>
		/// Unique identifier for the galaxy.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Galaxy friendly name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// List of stars in the galaxy.
		/// </summary>
		public List<StarSystem> StarSystems { get; set; }

		/// <summary>
		/// The game settings.
		/// </summary>
		public GameSettingsObject GameSettings { get; set; }

		public Galaxy(Random seed)
		{
			Id = Guid.NewGuid();
			Name = "Milky Way";
			StarSystems = new List<StarSystem>();
			GameSettingsObject newGameSettingsObject = new(1, "Placeholder");
			GameSettings = newGameSettingsObject;
		}
	}
}
