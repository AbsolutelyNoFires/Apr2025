using Neb25.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neb25.UI.Forms
{
	public partial class Game : Form
	{
		private Galaxy _galaxy;

		/// <summary>
		/// Constructor for the Game form.
		/// </summary>
		/// <param name="galaxy">The galaxy object for this game.</param>
		public Game(Galaxy galaxy)
		{
			_galaxy = galaxy;
			InitializeComponent();
			this.Text = _galaxy.Name;

		}
	}
}
