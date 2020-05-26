namespace DMR
{
	partial class FirmwareLoaderUI
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
			this.lblMessage = new System.Windows.Forms.Label();
			this.progressBarDwnl = new System.Windows.Forms.ProgressBar();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnDetectModel = new System.Windows.Forms.Button();
			this.btnDownloadFirmware = new System.Windows.Forms.Button();
			this.btnUploadFirmware = new System.Windows.Forms.Button();
			this.SuspendLayout();
			//
			// Progress group box
			//
			this.grpboxProgress = new System.Windows.Forms.GroupBox();
			this.grpboxProgress.Location = new System.Drawing.Point(5, 105);
			this.grpboxProgress.Size = new System.Drawing.Size(410, 60);
			this.grpboxProgress.Text = " Progress ";
			// 
			// lblMessage
			// 
			this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMessage.Location = new System.Drawing.Point(20, 18);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(370, 20);
			this.lblMessage.TabIndex = 0;
			this.lblMessage.Text = "label1";
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(15, 40);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(380, 11);
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar1.TabIndex = 1;

			this.grpboxProgress.Controls.Add(this.lblMessage);
			this.grpboxProgress.Controls.Add(this.progressBar1);
			this.grpboxProgress.Enabled = false;
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(340, 175);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "&Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			//
			// Model selector
			//
			this.grpboxModel = new System.Windows.Forms.GroupBox();
			this.grpboxModel.Text = " Select your radio type ";
			this.grpboxModel.Location = new System.Drawing.Point(5, 5);
			this.grpboxModel.Size = new System.Drawing.Size(410, 98);
			this.rbModels = new System.Windows.Forms.RadioButton[4];

			this.rbModels[0] = new System.Windows.Forms.RadioButton();
			this.rbModels[0].Text = "Radioddity GD-&77 / TYT MD-760";
			this.rbModels[0].Location = new System.Drawing.Point(5, 15);
			this.rbModels[0].UseVisualStyleBackColor = true;
			this.rbModels[0].Tag = (int)FirmwareLoader.OutputType.OutputType_GD77;
			this.rbModels[0].AutoSize = true;
			this.rbModels[0].CheckedChanged += new System.EventHandler(this.rbModel_CheckedChanged);

			this.rbModels[1] = new System.Windows.Forms.RadioButton();
			this.rbModels[1].Text = "Radioddity GD-77&S / TYT MD-730";
			this.rbModels[1].Location = new System.Drawing.Point(5, 35);
			this.rbModels[1].UseVisualStyleBackColor = true;
			this.rbModels[1].Tag = (int)FirmwareLoader.OutputType.OutputType_GD77S;
			this.rbModels[1].AutoSize = true;
			this.rbModels[1].CheckedChanged += new System.EventHandler(this.rbModel_CheckedChanged);

			this.rbModels[2] = new System.Windows.Forms.RadioButton();
			this.rbModels[2].Text = "Baofeng DM-&1801 / DM-860";
			this.rbModels[2].Location = new System.Drawing.Point(5, 55);
			this.rbModels[2].UseVisualStyleBackColor = true;
			this.rbModels[2].Tag = (int)FirmwareLoader.OutputType.OutputType_DM1801;
			this.rbModels[2].AutoSize = true;
			this.rbModels[2].CheckedChanged += new System.EventHandler(this.rbModel_CheckedChanged);

			this.rbModels[3] = new System.Windows.Forms.RadioButton();
			this.rbModels[3].Text = "Baofeng RD-&5R / DM-5R Tier2";
			this.rbModels[3].Location = new System.Drawing.Point(5, 75);
			this.rbModels[3].UseVisualStyleBackColor = true;
			this.rbModels[3].Tag = (int)FirmwareLoader.OutputType.OutputType_RD5R;
			this.rbModels[3].AutoSize = true;
			this.rbModels[3].CheckedChanged += new System.EventHandler(this.rbModel_CheckedChanged);


			//
			// progressBarDwnl
			//
			this.progressBarDwnl.Location = new System.Drawing.Point(233, 35);
			this.progressBarDwnl.Name = "progressBarDwnl";
			this.progressBarDwnl.Size = new System.Drawing.Size(170, 8);
			this.progressBarDwnl.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBarDwnl.TabIndex = 4;
			this.progressBarDwnl.Visible = false;
			this.progressBarDwnl.Maximum = 100;
			this.progressBarDwnl.Minimum = 0;
			this.progressBarDwnl.Value = 0;

			// 
			// btnDownloadFirmware
			// 
			this.btnDownloadFirmware.Location = new System.Drawing.Point(233, 10);//44
			this.btnDownloadFirmware.Name = "btnDownloadFirmware";
			this.btnDownloadFirmware.MinimumSize = new System.Drawing.Size(170, 25);
			this.btnDownloadFirmware.AutoSize = true;
			this.btnDownloadFirmware.TabIndex = 5;
			this.btnDownloadFirmware.Text = "&Download && Update";
			this.btnDownloadFirmware.UseVisualStyleBackColor = true;
			this.btnDownloadFirmware.Enabled = false;
			this.btnDownloadFirmware.Click += new System.EventHandler(this.btnDownloadFirmware_Click);
		
			// 
			// btnUploadFirmware
			// 
			this.btnUploadFirmware.Location = new System.Drawing.Point(233, 44);//69
			this.btnUploadFirmware.Name = "btnUploadFirmware";
			this.btnUploadFirmware.MinimumSize = new System.Drawing.Size(170, 25);
			this.btnUploadFirmware.AutoSize = true;
			this.btnUploadFirmware.TabIndex = 6;
			this.btnUploadFirmware.Text = "Select a &File && Update";
			this.btnUploadFirmware.UseVisualStyleBackColor = true;
			this.btnUploadFirmware.Enabled = false;
			this.btnUploadFirmware.Click += new System.EventHandler(this.btnUploadFirmware_Click);


			// 
			// btnDetectModel
			// 
			this.btnDetectModel.Location = new System.Drawing.Point(233, 69);//10
			this.btnDetectModel.Name = "btnDetectModel";
			this.btnDetectModel.MinimumSize = new System.Drawing.Size(170, 25);
			this.btnDetectModel.AutoSize = true;
			this.btnDetectModel.TabIndex = 3;
			this.btnDetectModel.Text = "Detect Radio Type";
			this.btnDetectModel.UseVisualStyleBackColor = true;
			this.btnDetectModel.Click += new System.EventHandler(this.btnDetectModel_Click);
			this.btnDetectModel.Visible = false;

			this.grpboxModel.Controls.Add(this.rbModels[0]);
			this.grpboxModel.Controls.Add(this.rbModels[1]);
			this.grpboxModel.Controls.Add(this.rbModels[2]);
			this.grpboxModel.Controls.Add(this.rbModels[3]);
			this.grpboxModel.Controls.Add(this.btnDetectModel);
			this.grpboxModel.Controls.Add(this.progressBarDwnl);
			this.grpboxModel.Controls.Add(this.btnDownloadFirmware);
			this.grpboxModel.Controls.Add(this.btnUploadFirmware);

			if (FirmwareLoader.outputType != FirmwareLoader.OutputType.OutputType_UNKNOWN)
			{
				this.rbModels[(int)FirmwareLoader.outputType].Checked = true;
				this.btnDownloadFirmware.Enabled = true;
				this.btnUploadFirmware.Enabled = true;
			}

			// 
			// FirmwareLoaderUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(420, 205);
			//this.Controls.Add(this.btnUploadFirmware);
			this.Controls.Add(this.grpboxModel);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.grpboxProgress);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "FirmwareLoaderUI";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Firmware loader";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FirmwareLoaderUI_FormClosing);
			this.Load += new System.EventHandler(this.FirmwareLoaderUI_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.ProgressBar progressBarDwnl;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnDetectModel;
		private System.Windows.Forms.Button btnDownloadFirmware;
		private System.Windows.Forms.Button btnUploadFirmware;
		private System.Windows.Forms.GroupBox grpboxModel;
		//private System.Windows.Forms.Panel panelProgress;
		private System.Windows.Forms.GroupBox grpboxProgress;
		private System.Windows.Forms.RadioButton[] rbModels;
	}
}
