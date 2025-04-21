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
			label1 = new Label();
			label2 = new Label();
			txt_NGNumSys = new TextBox();
			btn_backtomainmenu = new Button();
			btn_launchgame = new Button();
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
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(122, 84);
			label1.Name = "label1";
			label1.Size = new Size(70, 15);
			label1.TabIndex = 1;
			label1.Text = "Seed Phrase";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(82, 113);
			label2.Name = "label2";
			label2.Size = new Size(110, 15);
			label2.TabIndex = 2;
			label2.Text = "Number of systems";
			// 
			// txt_NGNumSys
			// 
			txt_NGNumSys.Location = new Point(198, 110);
			txt_NGNumSys.MaxLength = 32;
			txt_NGNumSys.Name = "txt_NGNumSys";
			txt_NGNumSys.PlaceholderText = "Sinew Sample Sunday";
			txt_NGNumSys.Size = new Size(100, 23);
			txt_NGNumSys.TabIndex = 3;
			txt_NGNumSys.Text = "750";
			txt_NGNumSys.TextChanged += txt_NGNumSys_TextChanged;
			// 
			// btn_backtomainmenu
			// 
			btn_backtomainmenu.Location = new Point(12, 415);
			btn_backtomainmenu.Name = "btn_backtomainmenu";
			btn_backtomainmenu.Size = new Size(121, 23);
			btn_backtomainmenu.TabIndex = 4;
			btn_backtomainmenu.Text = "Back to main menu";
			btn_backtomainmenu.UseVisualStyleBackColor = true;
			btn_backtomainmenu.Click += btn_backtomainmenu_Click;
			// 
			// btn_launchgame
			// 
			btn_launchgame.Location = new Point(663, 415);
			btn_launchgame.Name = "btn_launchgame";
			btn_launchgame.Size = new Size(115, 23);
			btn_launchgame.TabIndex = 5;
			btn_launchgame.Text = "Launch game";
			btn_launchgame.UseVisualStyleBackColor = true;
			btn_launchgame.Click += btn_launchgame_Click;
			// 
			// NewGameOptions
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(btn_launchgame);
			Controls.Add(btn_backtomainmenu);
			Controls.Add(txt_NGNumSys);
			Controls.Add(label2);
			Controls.Add(label1);
			Controls.Add(txt_NGSeedPhrase);
			Name = "NewGameOptions";
			Text = "NewGameOptions";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private TextBox txt_NGSeedPhrase;
		private Label label1;
		private Label label2;
		private TextBox txt_NGNumSys;
		private Button btn_backtomainmenu;
		private Button btn_launchgame;
	}
}