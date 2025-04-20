using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace Neb25.UI.Forms
{
	public partial class OptionsForm : Form
	{
		private MainMenu MainMenu;
		public OptionsForm(MainMenu parentForm)
		{
			InitializeComponent();
			this.MainMenu = parentForm;
		}
		private void button1_Click(object sender, EventArgs e) {this.Close();}
	}
}
