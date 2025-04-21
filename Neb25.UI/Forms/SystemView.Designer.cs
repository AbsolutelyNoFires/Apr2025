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
			this.systemPictureBox = new System.Windows.Forms.PictureBox();
			this.lblSystemName = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.lblInfo = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.systemPictureBox)).BeginInit();
			this.SuspendLayout();
			//
			// systemPictureBox
			//
			this.systemPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.systemPictureBox.BackColor = System.Drawing.Color.Black;
			this.systemPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.systemPictureBox.Location = new System.Drawing.Point(12, 41);
			this.systemPictureBox.Name = "systemPictureBox";
			this.systemPictureBox.Size = new System.Drawing.Size(610, 397);
			this.systemPictureBox.TabIndex = 0;
			this.systemPictureBox.TabStop = false;
			// Note: Paint, MouseDown, etc. events will be attached in SystemViewForm.cs constructor
			//
			// lblSystemName
			//
			this.lblSystemName.AutoSize = true;
			this.lblSystemName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.lblSystemName.Location = new System.Drawing.Point(12, 9);
			this.lblSystemName.Name = "lblSystemName";
			this.lblSystemName.Size = new System.Drawing.Size(114, 21);
			this.lblSystemName.TabIndex = 1;
			this.lblSystemName.Text = "System Name";
			//
			// btnClose
			//
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.Location = new System.Drawing.Point(644, 415);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(144, 23);
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "Back to Galaxy";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			//
			// lblInfo
			//
			this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblInfo.Location = new System.Drawing.Point(628, 41);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Padding = new System.Windows.Forms.Padding(5);
			this.lblInfo.Size = new System.Drawing.Size(160, 358);
			this.lblInfo.TabIndex = 3;
			this.lblInfo.Text = "System Information:\r\n--------------------\r\nSelect an object...";
			//
			// SystemViewForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.lblInfo);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.lblSystemName);
			this.Controls.Add(this.systemPictureBox);
			this.MinimumSize = new System.Drawing.Size(600, 400);
			this.Name = "SystemViewForm";
			this.Text = "System View";
			this.Load += new System.EventHandler(this.SystemViewForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.systemPictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox systemPictureBox;
		private System.Windows.Forms.Label lblSystemName;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label lblInfo;

	}
}