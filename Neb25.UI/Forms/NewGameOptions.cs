using Neb25.Core.Utils;
using Neb25.Core.Galaxy;


namespace Neb25.UI.Forms
{
	public partial class NewGameOptions : Form
	{

		public NewGameOptions()
		{
			InitializeComponent();
	
		}


		private void btn_LaunchGame_Click(object sender, EventArgs e)
		{
			// we only launch if  
			// a positive number of systems was entered  
			// the seed phrase has nonzero chars  
			
			int numSys = (int)num_NGNumSys.Value;
			bool numSysIsValid = numSys > 0;
			string seedPhrase = txt_NGSeedPhrase.Text;
			bool seedIsValid = seedPhrase.Length > 0;
			bool launchIsValid = numSysIsValid && seedIsValid;

			if (launchIsValid)
			{
				GameSettingsObject newGameSettings = new(numSys, seedPhrase);
				newGameSettings.GalaxyArms = (int)num_GalaxyArms.Value;
				Galaxy newGameGalaxy = Core.Galaxy.Generators.GalaxyGenerator.GenerateGalaxy(newGameSettings);
				GalaxyMap newGameGalaxyMap = new(newGameGalaxy);
				this.Hide();
				newGameGalaxyMap.Show();
			}
		}

		private void btn_BackToMainMenu_Click(object sender, EventArgs e)
		{
			MainMenu mainMenu = new MainMenu();
			this.Close();
			mainMenu.Show();
		}

		private void txt_NGSeedPhrase_TextChanged(object sender, EventArgs e)
		{

		}

		private void txt_NGNumSys_TextChanged(object sender, EventArgs e)
		{
			// if it's not numeric input, grey out the new game button  
		}
	}
}
