using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;

namespace DMR
{
	public partial class FirmwareLoaderUI : Form
	{
		public bool IsLoading = false;
		private static String tempFile = "";
		private WebClient wc = null;

		public FirmwareLoaderUI()
		{
			FirmwareLoader.outputType = FirmwareLoader.probeModel();

			if ((FirmwareLoader.outputType < FirmwareLoader.OutputType.OutputType_GD77) || (FirmwareLoader.outputType > FirmwareLoader.OutputType.OutputType_DM1801))
			{
				MessageBox.Show("Error: Unable to detect your radio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				FirmwareLoader.outputType = FirmwareLoader.OutputType.OutputType_GD77;
			}

			InitializeComponent();
			this.lblMessage.Text = "";
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!

		}

		public void SetLabel(string txt)
		{
			if (this.lblMessage.InvokeRequired)
			{
				this.Invoke(new Action(() => SetLabel(txt)));
			}
			else
			{
				this.lblMessage.Text = txt;
			}
		}
		public void SetProgressPercentage(int perc)
		{
			if (this.progressBar1.InvokeRequired)
			{
				this.Invoke(new Action(() => SetProgressPercentage(perc)));
			}
			else
			{
				this.progressBar1.Value = perc;
			}
		}

		public void SetLoadingState(bool loading)
		{
			if (loading)
			{
				this.grpboxModel.Enabled = false;
				this.btnClose.Enabled = false;
				this.grpboxProgress.Enabled = true;
			}
			else
			{
				this.grpboxModel.Enabled = true;
				this.btnClose.Enabled = true;
				this.grpboxProgress.Enabled = false;
				this.progressBar1.Value = 0;
			}
		}

		private void FirmwareLoaderUI_Load(object sender, EventArgs e)
		{

		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			if (!IsLoading)
			{
				this.Close();
			}
			else
			{
				MessageBox.Show("You can't interrupt the upload process");
			}
		}


		private void rbModel_CheckedChanged(object sender, EventArgs e)
		{
			RadioButton rb = sender as RadioButton;

			if (rb != null)
			{
				if (rb.Checked)
				{
					FirmwareLoader.outputType = (FirmwareLoader.OutputType)rb.Tag;
				}
			}
		}

		private void downloadProgressChangedCallback(object sender, DownloadProgressChangedEventArgs ev)
		{
			this.progressBarDwnl.Value = ev.ProgressPercentage;
		}

		private void downloadStringCompletedCallback(object sender, DownloadStringCompletedEventArgs ev)
		{
			String result = ev.Result;
			String urlBase = "http://github.com";
			String pattern = "";
			String urlFW = "";

			this.progressBarDwnl.Visible = false;

			// Define Regex's patterm, according to current Model selection
			switch (FirmwareLoader.outputType)
			{
				case FirmwareLoader.OutputType.OutputType_GD77:
					pattern = @"/rogerclarkmelbourne/OpenGD77/releases/download/R([0-9\.]+)/OpenGD77\.sgl";
					break;
				case FirmwareLoader.OutputType.OutputType_GD77S:
					pattern = @"/rogerclarkmelbourne/OpenGD77/releases/download/R([0-9\.]+)/OpenGD77S_HS\.sgl";
					break;
				case FirmwareLoader.OutputType.OutputType_DM1801:
					pattern = @"/rogerclarkmelbourne/OpenGD77/releases/download/R([0-9\.]+)/OpenDM1801\.sgl";
					break;
			}

			// Looking for firmware's URL
			String[] lines = result.Split('\n');
			foreach (String l in lines)
			{
				Match match = Regex.Match(l, pattern, RegexOptions.IgnoreCase);

				if (match.Success)
				{
					urlFW = match.Groups[0].Value;
					break;
				}
			}

			// Is firmware's URL found ?
			if (urlFW.Length > 0)
			{
				tempFile = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".sgl";

				// Download the firmware binary to a temporary file
				try
				{
					Application.DoEvents();
					this.progressBarDwnl.Value = 0;
					this.progressBarDwnl.Visible = true;
					wc.DownloadFileAsync(new Uri(urlBase + urlFW), tempFile);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

					if (File.Exists(tempFile))
						File.Delete(tempFile);

					SetLoadingState(false);
					this.progressBarDwnl.Visible = false;
					return;
				}
			}
		}

		private void downloadFileCompletedCallback(object sender, AsyncCompletedEventArgs ev)
		{
			this.progressBarDwnl.Visible = false;
			this.progressBarDwnl.Value = 0;

			// Now flash the downloaded firmware
			Action<object> action = (object obj) =>
			{
				IsLoading = true;
				SetLoadingState(true);
				FirmwareLoader.UploadFirmare(tempFile, this);
				SetLoadingState(false);
				IsLoading = false;
				// Cleanup
				if (File.Exists(tempFile))
					File.Delete(tempFile);
			};
			try
			{
				Task t1 = new Task(action, "LoaderUSB");
				t1.Start();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				SetLoadingState(false);
				IsLoading = false;
				// Cleanup
				if (File.Exists(tempFile))
					File.Delete(tempFile);
			}
		}

		private void btnDetectModel_Click(object sender, EventArgs e)
		{
			this.btnDetectModel.Enabled = false;

			FirmwareLoader.outputType = FirmwareLoader.probeModel();

			if ((FirmwareLoader.outputType < FirmwareLoader.OutputType.OutputType_GD77) || (FirmwareLoader.outputType > FirmwareLoader.OutputType.OutputType_DM1801))
			{
				MessageBox.Show("Error: Unable to detect your radio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				FirmwareLoader.outputType = FirmwareLoader.OutputType.OutputType_GD77;
			}

			this.rbModels[(int)FirmwareLoader.outputType].Checked = true;
			this.btnDetectModel.Enabled = true;
		}

		private void btnDownloadFirmware_Click(object sender, EventArgs e)
		{
			Uri uri = new Uri("https://github.com/rogerclarkmelbourne/OpenGD77/releases/latest");
		
			this.lblMessage.Text = "";
			wc = new WebClient();

			ServicePointManager.Expect100Continue = true;
			// If you have .Net 4.5
			//ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			// otherwise
			ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

			this.progressBarDwnl.Value = 0;

			wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(downloadProgressChangedCallback);
			wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(downloadStringCompletedCallback);
			wc.DownloadFileCompleted += new AsyncCompletedEventHandler(downloadFileCompletedCallback);

			this.progressBarDwnl.Visible = true;
			SetLoadingState(true);

			// Retrieve release webpage
			try
			{
				Application.DoEvents();
				wc.DownloadStringAsync(uri);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				SetLoadingState(false);
				this.progressBarDwnl.Visible = false;
				return;
			}
		}

		private void btnUploadFirmware_Click(object sender, EventArgs e)
		{
			if (IsLoading)
			{
				return;
			}

			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "firmware files|*.sgl";
			openFileDialog.InitialDirectory = IniFileUtils.getProfileStringWithDefault("Setup", "LastFirmwareLocation", null);

			if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileName != null)
			{
				IniFileUtils.WriteProfileString("Setup", "LastFirmwareLocation", Path.GetDirectoryName(openFileDialog.FileName));

				this.lblMessage.Text = "";

				Action<object> action = (object obj) =>
				{
					IsLoading = true;
					SetLoadingState(true);
					FirmwareLoader.UploadFirmare(openFileDialog.FileName, this);
					SetLoadingState(false);
					IsLoading = false;
				};
				try
				{
					Task t1 = new Task(action, "LoaderUSB");
					t1.Start();
				}
				catch (Exception)
				{
					IsLoading = false;
					SetLoadingState(false);
				}

			}
		}

		private void FirmwareLoaderUI_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (IsLoading)
			{
				MessageBox.Show("You can't interrupt the upload process");
			}
			e.Cancel = IsLoading;
		}
	}
}

