using System.Text;
using System.Security.Cryptography;

namespace Neb25.Core.Utils
{
	public class GameSettingsObject
	{
		public int NumberOfStarSystems { get; set; }

		public string SeedPhrase { get; set; }

		public int SeedNum {  get; set; }
		public int GalaxyArms { get; set; }

		public GameSettingsObject(int numberOfStarSystems, string seedPhrase) {
			// 
			NumberOfStarSystems = numberOfStarSystems;
			SeedPhrase = seedPhrase;

			SHA256 sha256 = SHA256.Create();
			byte[] inputBytes = Encoding.UTF8.GetBytes(seedPhrase);
			byte[] hashBytes = sha256.ComputeHash(inputBytes);
			SeedNum = BitConverter.ToInt32(hashBytes, 0);
			GalaxyArms = 4;


		}
	}
}
