namespace Neb25.UI.Forms
{
	partial class MainMenu
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
			btnNewGame = new Button();
			btnOptions = new Button();
			btnExit = new Button();
			SuspendLayout();
			// 
			// btnNewGame
			// 
			btnNewGame.Location = new Point(345, 158);
			btnNewGame.Name = "btnNewGame";
			btnNewGame.Size = new Size(75, 23);
			btnNewGame.TabIndex = 0;
			btnNewGame.Text = "New Game";
			btnNewGame.UseVisualStyleBackColor = true;
			btnNewGame.Click += btnNewGame_Click;
			// 
			// btnOptions
			// 
			btnOptions.Location = new Point(345, 205);
			btnOptions.Name = "btnOptions";
			btnOptions.Size = new Size(75, 23);
			btnOptions.TabIndex = 1;
			btnOptions.Text = "Options";
			btnOptions.UseVisualStyleBackColor = true;
			btnOptions.Click += btnOptions_Click;
			// 
			// btnExit
			// 
			btnExit.Location = new Point(345, 252);
			btnExit.Name = "btnExit";
			btnExit.Size = new Size(75, 23);
			btnExit.TabIndex = 2;
			btnExit.Text = "Exit";
			btnExit.UseVisualStyleBackColor = true;
			btnExit.Click += btnExit_Click;
			// 
			// MainMenu
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(btnExit);
			Controls.Add(btnOptions);
			Controls.Add(btnNewGame);
			Name = "MainMenu";
			Text = "Form1";
			ResumeLayout(false);
		}

		#endregion

		private Button btnNewGame;
		private Button btnOptions;
		private Button btnExit;
	}
}