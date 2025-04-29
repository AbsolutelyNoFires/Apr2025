namespace Neb25.UI.Forms
{
	partial class SystemViewForm
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
				// Dispose custom graphics objects
				_starBrush?.Dispose();
				_planetBrush?.Dispose();
				_jumpSiteBrush?.Dispose();
				_jumpSiteConnectedBrush?.Dispose();
				_infoFont?.Dispose();
				_infoBrush?.Dispose();
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
			systemPictureBox = new PictureBox();
			lblSystemName = new Label();
			btnClose = new Button();
			lblInfo = new Label();
			((System.ComponentModel.ISupportInitialize)systemPictureBox).BeginInit();
			SuspendLayout();
			// 
			// systemPictureBox
			// 
			systemPictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			systemPictureBox.BackColor = Color.Black;
			systemPictureBox.BorderStyle = BorderStyle.FixedSingle;
			systemPictureBox.Location = new Point(12, 41);
			systemPictureBox.Name = "systemPictureBox";
			systemPictureBox.Size = new Size(610, 397);
			systemPictureBox.TabIndex = 0;
			systemPictureBox.TabStop = false;
			// 
			// lblSystemName
			// 
			lblSystemName.AutoSize = true;
			lblSystemName.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
			lblSystemName.Location = new Point(12, 9);
			lblSystemName.Name = "lblSystemName";
			lblSystemName.Size = new Size(115, 21);
			lblSystemName.TabIndex = 1;
			lblSystemName.Text = "System Name";
			// 
			// btnClose
			// 
			btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnClose.Location = new Point(644, 415);
			btnClose.Name = "btnClose";
			btnClose.Size = new Size(144, 23);
			btnClose.TabIndex = 2;
			btnClose.Text = "Back to Galaxy";
			btnClose.UseVisualStyleBackColor = true;
			btnClose.Click += btnClose_Click;
			// 
			// lblInfo
			// 
			lblInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			lblInfo.BorderStyle = BorderStyle.FixedSingle;
			lblInfo.Location = new Point(628, 41);
			lblInfo.Name = "lblInfo";
			lblInfo.Padding = new Padding(5);
			lblInfo.Size = new Size(160, 358);
			lblInfo.TabIndex = 3;
			lblInfo.Text = "System Information:\r\n--------------------\r\nSelect an object...";
			// 
			// SystemViewForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(lblInfo);
			Controls.Add(btnClose);
			Controls.Add(lblSystemName);
			Controls.Add(systemPictureBox);
			MinimumSize = new Size(600, 400);
			Name = "SystemViewForm";
			Text = "System:";
			Load += SystemViewForm_Load;
			((System.ComponentModel.ISupportInitialize)systemPictureBox).EndInit();
			ResumeLayout(false);
			PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox systemPictureBox;
		private System.Windows.Forms.Label lblSystemName;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label lblInfo;

	}
}