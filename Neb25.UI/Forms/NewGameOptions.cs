using Neb25.Core.Galaxy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neb25.UI.Forms
{
	public partial class NewGameOptions : Form
	{
		public NewGameOptions()
		{
			InitializeComponent();
		}

		private void btn_launchgame_Click(object sender, EventArgs e)
		{
			// we only launch if a positive number of systems was entered, and the seed phrase is valid
			bool launchIsValid = txt_NGNumSys.TextLength > 0 && txt_NGNumSys.Text.All(char.IsDigit);
			launchIsValid = launchIsValid && txt_NGSeedPhrase.TextLength > 0;
			if (launchIsValid) { 
				
				Int32 numSys = Int32.Parse(txt_NGNumSys.Text);

				String seedPhrase = txt_NGSeedPhrase.Text;
				// convert seed phrase to numeric
				SHA256 sha256 = SHA256.Create();
				byte[] inputBytes = Encoding.UTF8.GetBytes(seedPhrase);
				byte[] hashBytes = sha256.ComputeHash(inputBytes);
				int seedNum = BitConverter.ToInt32(hashBytes, 0);
				Random gameRNG = new Random(seedNum);

				Galaxy newGalaxy = GalaxyGenerator.GenerateGalaxy(numSys, gameRNG);
				GalaxyMap gameForm = new(newGalaxy);
				this.Hide();
				gameForm.Show();
			}


		}

		private void btn_backtomainmenu_Click(object sender, EventArgs e)
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
