namespace Neb25.UI.Forms
{
	partial class NewGameOptions
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			txt_NGSeedPhrase = new TextBox();
			lbl_SeedPhrase = new Label();
			lbl_StarSysNum = new Label();
			btn_backtomainmenu = new Button();
			btn_launchgame = new Button();
			num_NGNumSys = new NumericUpDown();
			num_GalaxyArms = new NumericUpDown();
			lbl_GalaxyArms = new Label();
			((System.ComponentModel.ISupportInitialize)num_NGNumSys).BeginInit();
			((System.ComponentModel.ISupportInitialize)num_GalaxyArms).BeginInit();
			SuspendLayout();
			// 
			// txt_NGSeedPhrase
			// 
			txt_NGSeedPhrase.Location = new Point(198, 81);
			txt_NGSeedPhrase.MaxLength = 32;
			txt_NGSeedPhrase.Name = "txt_NGSeedPhrase";
			txt_NGSeedPhrase.PlaceholderText = "Sinew Sample Sunday";
			txt_NGSeedPhrase.Size = new Size(216, 23);
			txt_NGSeedPhrase.TabIndex = 0;
			txt_NGSeedPhrase.Text = "Whimpering Willows Wandering";
			txt_NGSeedPhrase.TextChanged += txt_NGSeedPhrase_TextChanged;
			// 
			// lbl_SeedPhrase
			// 
			lbl_SeedPhrase.AutoSize = true;
			lbl_SeedPhrase.Location = new Point(122, 84);
			lbl_SeedPhrase.Name = "lbl_SeedPhrase";
			lbl_SeedPhrase.Size = new Size(70, 15);
			lbl_SeedPhrase.TabIndex = 1;
			lbl_SeedPhrase.Text = "Seed Phrase";
			// 
			// lbl_StarSysNum
			// 
			lbl_StarSysNum.AutoSize = true;
			lbl_StarSysNum.Location = new Point(82, 113);
			lbl_StarSysNum.Name = "lbl_StarSysNum";
			lbl_StarSysNum.Size = new Size(110, 15);
			lbl_StarSysNum.TabIndex = 2;
			lbl_StarSysNum.Text = "Number of systems";
			// 
			// btn_backtomainmenu
			// 
			btn_backtomainmenu.Location = new Point(12, 415);
			btn_backtomainmenu.Name = "btn_backtomainmenu";
			btn_backtomainmenu.Size = new Size(121, 23);
			btn_backtomainmenu.TabIndex = 4;
			btn_backtomainmenu.Text = "Back to main menu";
			btn_backtomainmenu.UseVisualStyleBackColor = true;
			btn_backtomainmenu.Click += btn_BackToMainMenu_Click;
			// 
			// btn_launchgame
			// 
			btn_launchgame.Location = new Point(663, 415);
			btn_launchgame.Name = "btn_launchgame";
			btn_launchgame.Size = new Size(115, 23);
			btn_launchgame.TabIndex = 5;
			btn_launchgame.Text = "Launch game";
			btn_launchgame.UseVisualStyleBackColor = true;
			btn_launchgame.Click += btn_LaunchGame_Click;
			// 
			// num_NGNumSys
			// 
			num_NGNumSys.Location = new Point(198, 110);
			num_NGNumSys.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
			num_NGNumSys.Name = "num_NGNumSys";
			num_NGNumSys.Size = new Size(120, 23);
			num_NGNumSys.TabIndex = 6;
			num_NGNumSys.Value = new decimal(new int[] { 75, 0, 0, 0 });
			// 
			// num_GalaxyArms
			// 
			num_GalaxyArms.Location = new Point(198, 139);
			num_GalaxyArms.Maximum = new decimal(new int[] { 99, 0, 0, 0 });
			num_GalaxyArms.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			num_GalaxyArms.Name = "num_GalaxyArms";
			num_GalaxyArms.Size = new Size(120, 23);
			num_GalaxyArms.TabIndex = 8;
			num_GalaxyArms.Value = new decimal(new int[] { 4, 0, 0, 0 });
			// 
			// lbl_GalaxyArms
			// 
			lbl_GalaxyArms.AutoSize = true;
			lbl_GalaxyArms.Location = new Point(120, 141);
			lbl_GalaxyArms.Name = "lbl_GalaxyArms";
			lbl_GalaxyArms.Size = new Size(72, 15);
			lbl_GalaxyArms.TabIndex = 7;
			lbl_GalaxyArms.Text = "Galaxy Arms";
			// 
			// NewGameOptions
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(num_GalaxyArms);
			Controls.Add(lbl_GalaxyArms);
			Controls.Add(num_NGNumSys);
			Controls.Add(btn_launchgame);
			Controls.Add(btn_backtomainmenu);
			Controls.Add(lbl_StarSysNum);
			Controls.Add(lbl_SeedPhrase);
			Controls.Add(txt_NGSeedPhrase);
			Name = "NewGameOptions";
			Text = "NewGameOptions";
			((System.ComponentModel.ISupportInitialize)num_NGNumSys).EndInit();
			((System.ComponentModel.ISupportInitialize)num_GalaxyArms).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private TextBox txt_NGSeedPhrase;
		private Label lbl_SeedPhrase;
		private Label lbl_StarSysNum;
		private Button btn_backtomainmenu;
		private Button btn_launchgame;
		private NumericUpDown num_NGNumSys;
		private NumericUpDown num_GalaxyArms;
		private Label lbl_GalaxyArms;
	}
}