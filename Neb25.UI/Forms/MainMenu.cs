﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Neb25.Core;
using System.Collections.Generic;
using Neb25.Core.Galaxy;

namespace Neb25.UI.Forms
{
	public partial class MainMenu : Form
	{
		public MainMenu()
		{
			InitializeComponent();
		}

		private void btnNewGame_Click(object sender, EventArgs e)
		{
			NewGameOptions newGameOptions = new NewGameOptions();
			this.Hide();
			newGameOptions.Show();
			
		}

		private void btnOptions_Click(object sender, EventArgs e)
		{
			OptionsForm optionsForm = new OptionsForm(this);
			optionsForm.ShowDialog();
		}
		private void btnExit_Click(object sender, EventArgs e) {Application.Exit();}
	}
}
