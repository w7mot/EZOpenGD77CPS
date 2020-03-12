using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace DMR
{
	public partial class FirmwareLoaderUI : Form
	{
		private string _filename;
		public bool IsLoading = false;
		public FirmwareLoaderUI(string filename)
		{
			_filename = filename;
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
					UpdateUploadButtonLabel();
				}
			}
		}

		private void btnUploadFirmware_Click(object sender, EventArgs e)
		{
			if (IsLoading)
			{
				return;
			}

			//if (MessageBox.Show("This feature is experimental.\nYou use it at your own risk\n\nAre you sure you want to upload firmware to the radio?","Warning", MessageBoxButtons.YesNo)==DialogResult.Yes)
			{
				Action<object> action = (object obj) =>
				{
					IsLoading = true;
					SetLoadingState(true);
					FirmwareLoader.UploadFirmare(_filename, this);
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

