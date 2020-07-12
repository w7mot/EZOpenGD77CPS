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
	public class WebClientAsync : WebClient
	{
		private int timeoutMS;
		private System.Timers.Timer timer;

		public WebClientAsync(int timeoutSeconds)
		{
			timeoutMS = timeoutSeconds * 1000;

			timer = new System.Timers.Timer(timeoutMS);
			System.Timers.ElapsedEventHandler handler = null;

			handler = ((sender, args) =>
			{
				this.CancelAsync();
				timer.Stop();
				timer.Elapsed -= handler;
			});

			timer.Elapsed += handler;
			timer.Enabled = true;
		}

		protected override WebRequest GetWebRequest(Uri address)
		{
			WebRequest request = base.GetWebRequest(address);
			request.Timeout = timeoutMS;
			((HttpWebRequest)request).ReadWriteTimeout = timeoutMS;

			return request;
		}

		protected override void OnDownloadProgressChanged(DownloadProgressChangedEventArgs e)
		{
			base.OnDownloadProgressChanged(e);
			timer.Stop();
			timer.Start();
		}
	}

	public partial class FirmwareLoaderUI : Form
	{
		public bool IsLoading = false;
		private static String tempFile = "";
		private WebClientAsync wc = null;

		public FirmwareLoaderUI()
		{
			InitializeComponent();
			this.lblMessage.Text = "";
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!

			FirmwareLoader.outputType = FirmwareLoader.OutputType.OutputType_GD77;// FirmwareLoader.OutputType.OutputType_UNKNOWN;// FirmwareLoader.probeModel();
/*
			if ((FirmwareLoader.outputType < FirmwareLoader.OutputType.OutputType_GD77) || (FirmwareLoader.outputType > FirmwareLoader.OutputType.OutputType_DM1801))
			{
				MessageBox.Show("Error: Unable to detect your radio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				FirmwareLoader.outputType = FirmwareLoader.OutputType.OutputType_GD77;
			}
			this.rbModels[(int)FirmwareLoader.outputType].Checked = true;
 * */
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
				Invoke((MethodInvoker)delegate
				{
					this.grpboxModel.Enabled = false;
					this.btnClose.Enabled = false;
					this.grpboxProgress.Enabled = true;
				});

			}
			else
			{
				Invoke((MethodInvoker)delegate
				{
					this.grpboxModel.Enabled = true;
					this.btnClose.Enabled = true;
					this.grpboxProgress.Enabled = false;
					this.progressBar1.Value = 0;
				});
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
					this.btnDownloadFirmware.Enabled = true;
					this.btnUploadFirmware.Enabled = true;
				}
			}
		}

		private DialogResult DialogBox(String title, String message, String btn1Label = "&Yes", String btn2Label = "&No", String btn3Label = "&Cancel")
		{
			int buttonX = 10;
			int buttonY = 120 - 25 - 5;
			Form form = new System.Windows.Forms.Form();
			Label label = new System.Windows.Forms.Label();
			Button button1 = new System.Windows.Forms.Button();
			Button button2 = new System.Windows.Forms.Button();
			Button button3 = new System.Windows.Forms.Button();
			form.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);// Roger Clark. Added correct icon on main form!

			form.SuspendLayout();

			if (btn1Label.Length <= 0)
			{
				button1.Visible = false;
				button1.Enabled = false;
			}

			if (btn2Label.Length <= 0)
			{
				button2.Visible = false;
				button2.Enabled = false;
			}

			if (btn1Label.Length <= 0 || btn2Label.Length <= 0)
			{
				buttonX += 120 + 10;
			}

			form.Text = title;

			// Label
			label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label.Location = new System.Drawing.Point(13, 13);
			label.Name = "LblMessage";
			label.Size = new System.Drawing.Size(380 - (13 * 2), (120 - 24 - 13 - 13));
			label.Text = message;
			label.TextAlign = ContentAlignment.MiddleCenter;

			// Button 1
			button1.Text = btn1Label ?? string.Empty;
			button1.Name = "btnYes";
			button1.Location = new System.Drawing.Point(buttonX, buttonY);
			button1.Size = new System.Drawing.Size(120, 24);
			button1.UseVisualStyleBackColor = true;

			if (button1.Visible)
			{
				buttonX += 120 + 10;
			}

			// Button 2
			button2.Text = btn2Label ?? string.Empty;
			button2.Name = "btnNo";
			button2.Location = new System.Drawing.Point(buttonX, buttonY);
			button2.Size = new System.Drawing.Size(120, 24);
			button2.UseVisualStyleBackColor = true;

			// Button 3
			button3.Text = btn3Label ?? string.Empty;
			button3.Location = new System.Drawing.Point((380 - 100 - 10), buttonY);
			button3.Name = "btnCancel";
			button3.Size = new System.Drawing.Size(100, 24);
			button3.UseVisualStyleBackColor = true;

			// Assign results
			button1.DialogResult = DialogResult.Yes;
			button2.DialogResult = DialogResult.No;
			button3.DialogResult = DialogResult.Cancel;

			form.ClientSize = new System.Drawing.Size(396, 107);
			form.Controls.Add(label);

			if (button1.Visible)
			{
				form.Controls.Add(button1);
			}

			if (button2.Visible)
			{
				form.Controls.Add(button2);
			}

			form.Controls.Add(button3);

			form.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			form.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			form.ClientSize = new System.Drawing.Size(380, 120);
			form.KeyPreview = true;
			form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = (button1.Visible == false ? button2 : button1);
			form.CancelButton = button3;
			form.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			form.ResumeLayout(false);

			DialogResult dialogResult = form.ShowDialog();

			return dialogResult;
		}

		private void downloadedGetReleaseAndDevelURLs(String[] lines, ref String releaseURL, ref String develURL)
		{
			String patternFormat = "";
			String pattern;

			releaseURL = "";
			develURL = "";

			// Define Regex's patterm, according to current Model selection
			switch (FirmwareLoader.outputType)
			{
				case FirmwareLoader.OutputType.OutputType_GD77:
					patternFormat = @"/rogerclarkmelbourne/OpenGD77/releases/download/{0}([0-9\.]+)/OpenGD77\.sgl";
					break;
				case FirmwareLoader.OutputType.OutputType_GD77S:
					patternFormat = @"/rogerclarkmelbourne/OpenGD77/releases/download/{0}([0-9\.]+)/OpenGD77S\.sgl";
					break;
				case FirmwareLoader.OutputType.OutputType_DM1801:
					patternFormat = @"/rogerclarkmelbourne/OpenGD77/releases/download/{0}([0-9\.]+)/OpenDM1801\.sgl";
					break;
				case FirmwareLoader.OutputType.OutputType_RD5R:
					patternFormat = @"/rogerclarkmelbourne/OpenGD77/releases/download/{0}([0-9\.]+)/OpenRD5R\.sgl";
					break;
			}

			pattern = String.Format(patternFormat, 'R');
			foreach (String l in lines)
			{
				Match match = Regex.Match(l, pattern, RegexOptions.IgnoreCase);

				if (match.Success)
				{
					releaseURL = match.Groups[0].Value;
					break;
				}
			}

			pattern = String.Format(patternFormat, 'D');
			foreach (String l in lines)
			{
				Match match = Regex.Match(l, pattern, RegexOptions.IgnoreCase);

				if (match.Success)
				{
					develURL = match.Groups[0].Value;
					break;
				}
			}
		}

		private void downloadProgressChangedCallback(object sender, DownloadProgressChangedEventArgs ev)
		{
			this.progressBarDwnl.Value = ev.ProgressPercentage;
		}

		private void downloadStringCompletedCallback(object sender, DownloadStringCompletedEventArgs ev)
		{
			if (ev.Cancelled)
			{
				MessageBox.Show("Download has been canceled.", "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Error);

				SetLoadingState(false);
				this.progressBarDwnl.Visible = false;
				return;
			}
			else if (ev.Error != null)
			{
				MessageBox.Show(ev.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

				SetLoadingState(false);
				this.progressBarDwnl.Visible = false;
				return;
			}

			String result = ev.Result;
			String urlBase = "http://github.com";
			String urlFW = "";
			String patternR = "", patternD = "";
			String releaseURL = "", develURL = "";

			this.progressBarDwnl.Visible = false;

			// Looking for firmware's URL
			String[] lines = result.Split('\n');

			downloadedGetReleaseAndDevelURLs(lines, ref releaseURL, ref develURL);

			// Is firmware's URL found ?
			if ((releaseURL.Length > 0) || (develURL.Length > 0))
			{
				String message;
				String[] buttonsLabel = new String[2];

				buttonsLabel[0] = "&Stable ";
				buttonsLabel[1] = "&Unstable ";

				// Extract release version
				patternR = @"/R([0-9\.]+)/";
				patternD = @"/D([0-9\.]+)/";

				Match matchR = Regex.Match(releaseURL, patternR, RegexOptions.IgnoreCase);
				Match matchD = Regex.Match(develURL, patternD, RegexOptions.IgnoreCase);

				if (matchR.Success && (releaseURL.Length > 0))
				{
					buttonsLabel[0] += matchR.Groups[0].Value.Trim('/').Remove(0, 1);
				}
				else
				{
					buttonsLabel[0] = "";
				}

				if (matchD.Success && (develURL.Length > 0))
				{
					buttonsLabel[1] += matchD.Groups[0].Value.Trim('/').Remove(0, 1);
				}
				else
				{
					buttonsLabel[1] = "";
				}

				if ((releaseURL.Length > 0) && (develURL.Length > 0))
				{
					message = "Please choose between Stable and Development version to download and install.";
				}
				else
				{
					message = "It will download and install a firmware.\n\nPlease make you choice.";
				}

				DialogResult res = DialogBox("Select version", message, buttonsLabel[0], buttonsLabel[1]);

				switch (res)
				{
					case DialogResult.Yes:
						// Stable
						urlFW = releaseURL;
						break;

					case DialogResult.No:
						// Devel
						urlFW = develURL;
						break;

					case DialogResult.Cancel:
						SetLoadingState(false);
						return;
				}

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
			else
			{
				MessageBox.Show(String.Format("Error: unable to find a firmware for your {0} transceiver.", FirmwareLoader.getModelName()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				SetLoadingState(false);
			}
		}

		private void downloadFileCompletedCallback(object sender, AsyncCompletedEventArgs ev)
		{
			this.progressBarDwnl.Visible = false;
			this.progressBarDwnl.Value = 0;

			if (ev.Cancelled)
			{
				MessageBox.Show("Download has been canceled.", "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Error);

				SetLoadingState(false);
				IsLoading = false;
				return;
			}
			else if (ev.Error != null)
			{
				MessageBox.Show(ev.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

				SetLoadingState(false);
				IsLoading = false;
				return;
			}

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
				{
					File.Delete(tempFile);
				}
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
				{
					File.Delete(tempFile);
				}
			}
		}

		private void btnDetectModel_Click(object sender, EventArgs e)
		{
			this.btnDetectModel.Enabled = false;

			FirmwareLoader.outputType = FirmwareLoader.OutputType.OutputType_UNKNOWN;//FirmwareLoader.probeModel();

			if ((FirmwareLoader.outputType < FirmwareLoader.OutputType.OutputType_GD77) || (FirmwareLoader.outputType > FirmwareLoader.OutputType.OutputType_RD5R))
			{
				MessageBox.Show("Error: Unable to detect your radio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				FirmwareLoader.outputType = FirmwareLoader.OutputType.OutputType_GD77;
			}

			this.rbModels[(int)FirmwareLoader.outputType].Checked = true;
			this.btnDetectModel.Enabled = true;
		}

		private void btnDownloadFirmware_Click(object sender, EventArgs e)
		{
			Uri uri = new Uri("https://github.com/rogerclarkmelbourne/OpenGD77/releases");
		
			this.lblMessage.Text = "";
			wc = new WebClientAsync(40);

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
			catch (WebException ex)
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

