namespace Neb25.Core.Galaxy
{
	public class Moon
	{
		// name, parent world, orbital index
		public Guid Id { get; set; }

		public string Name { get; set; }
		public Planet ParentPlanet { get; set; }
		public int OrbitalIndex { get; set; }

		public Moon(Planet parentPlanet) {
			Id = Guid.NewGuid();
			ParentPlanet = parentPlanet;
			OrbitalIndex = ParentPlanet.Moons.Count;
			Name = parentPlanet.Name + " - Moon " + OrbitalIndex;
			parentPlanet.Moons.Add(this);
		}
	}
}
