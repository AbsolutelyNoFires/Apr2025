namespace Neb25.UI.Forms
{
	partial class GalaxyMap
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
			selectedSystemInfoPanel = new Panel();
			enterSystemButton = new Button();
			systemNameLabel = new Label();
			subwayMapPictureBox = new PictureBox();
			((System.ComponentModel.ISupportInitialize)galaxyPictureBox).BeginInit();
			selectedSystemInfoPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)subwayMapPictureBox).BeginInit();
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
			// selectedSystemInfoPanel
			// 
			selectedSystemInfoPanel.Controls.Add(enterSystemButton);
			selectedSystemInfoPanel.Controls.Add(systemNameLabel);
			selectedSystemInfoPanel.Controls.Add(subwayMapPictureBox);
			selectedSystemInfoPanel.Location = new Point(0, 0);
			selectedSystemInfoPanel.Name = "selectedSystemInfoPanel";
			selectedSystemInfoPanel.Size = new Size(155, 183);
			selectedSystemInfoPanel.TabIndex = 1;
			selectedSystemInfoPanel.Visible = false;
			// 
			// enterSystemButton
			// 
			enterSystemButton.Dock = DockStyle.Bottom;
			enterSystemButton.Location = new Point(0, 160);
			enterSystemButton.Name = "enterSystemButton";
			enterSystemButton.Size = new Size(155, 23);
			enterSystemButton.TabIndex = 3;
			enterSystemButton.Text = "Enter System";
			enterSystemButton.UseVisualStyleBackColor = true;
			enterSystemButton.Visible = false;
			enterSystemButton.Click += EnterSystemButton_Click;
			// 
			// systemNameLabel
			// 
			systemNameLabel.AutoSize = true;
			systemNameLabel.Location = new Point(5, 5);
			systemNameLabel.Name = "systemNameLabel";
			systemNameLabel.Size = new Size(0, 15);
			systemNameLabel.TabIndex = 0;
			systemNameLabel.Visible = false;
			// 
			// subwayMapPictureBox
			// 
			subwayMapPictureBox.Dock = DockStyle.Top;
			subwayMapPictureBox.Location = new Point(0, 0);
			subwayMapPictureBox.Name = "subwayMapPictureBox";
			subwayMapPictureBox.Size = new Size(155, 78);
			subwayMapPictureBox.TabIndex = 2;
			subwayMapPictureBox.TabStop = false;
			subwayMapPictureBox.Visible = false;
			// 
			// GalaxyMap
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(selectedSystemInfoPanel);
			Controls.Add(galaxyPictureBox);
			Name = "GalaxyMap";
			Text = "Game";
			((System.ComponentModel.ISupportInitialize)galaxyPictureBox).EndInit();
			selectedSystemInfoPanel.ResumeLayout(false);
			selectedSystemInfoPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)subwayMapPictureBox).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private PictureBox galaxyPictureBox;
		private void galaxyPictureBox_Click(object sender, EventArgs e)
		{

		}
		private Panel selectedSystemInfoPanel;
		private Label systemNameLabel;
		private PictureBox subwayMapPictureBox;
		private Button enterSystemButton;
	}
}