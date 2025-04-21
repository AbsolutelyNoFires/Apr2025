namespace Neb25.UI.Forms
{
	partial class Game
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;



		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			galaxyPictureBox = new PictureBox();
			((System.ComponentModel.ISupportInitialize)galaxyPictureBox).BeginInit();
			SuspendLayout();
			// 
			// galaxyPictureBox
			// 
			galaxyPictureBox.Dock = DockStyle.Fill;
			galaxyPictureBox.Location = new Point(0, 0);
			galaxyPictureBox.Name = "galaxyPictureBox";
			galaxyPictureBox.Size = new Size(800, 450);
			galaxyPictureBox.TabIndex = 0;
			galaxyPictureBox.TabStop = false;
			galaxyPictureBox.Click += galaxyPictureBox_Click;
			// 
			// Game
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(galaxyPictureBox);
			Name = "Game";
			Text = "Game";
			((System.ComponentModel.ISupportInitialize)galaxyPictureBox).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private PictureBox galaxyPictureBox;
		private void galaxyPictureBox_Click(object sender, EventArgs e)
		{

		}
	}
}