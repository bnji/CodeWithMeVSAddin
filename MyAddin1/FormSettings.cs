using CodeWithMe;
using MyAddin1.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyAddin1
{
    public partial class FormSettings : Form
    {
        static string MSG_AUTH_SUCCESS = "Successfully authorized!";
        static string MSG_AUTH_FAILED = "Authorization failed. Please check your API keys!";
        private ApiHandler apiAuth;
        private CodeWithMeServer server;

        public FormSettings()
        {
            InitializeComponent();
        }

        public FormSettings(CodeWithMeServer _server) : this()
        {
            server = _server;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            server.SetApiData(Settings.Default.ApiUrl, Settings.Default.ApiKeyPrivate, Settings.Default.ApiKeyPublic);
            if (server.Start())
            {
                lblStatus.Text = "Service started...";
                btnStart.Enabled = false;
                btnStop.Enabled = true;
            }
            else
            {
                lblStatus.Text = "Authorization failed. Please check your API keys!";
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            server.Stop();
            lblStatus.Text = "Service stopped...";
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string apiKeyPrivate = txtApiKeyPrivate.Text;
            string apiKeyPublic = txtApiKeyPublic.Text;
            string apiUrl = txtApiUrl.Text;

            bool result = Authorize(apiUrl, apiKeyPrivate, apiKeyPublic);
            
            //SetButtonState(result);
            
            if(result)
            {
                btnStart.Enabled = true;
                lblStatus.Text = MSG_AUTH_SUCCESS;
                txtTokenValue.Text = apiAuth.TokenValue;
                Settings.Default.ApiKeyPublic = apiKeyPublic;
                Settings.Default.ApiKeyPrivate = apiKeyPrivate;
                Settings.Default.ApiUrl = apiUrl;
                Settings.Default.TokenValue = apiAuth.TokenValue;
                Settings.Default.Save();
            }
            else
            {
                lblStatus.Text = MSG_AUTH_FAILED;
            }
        }

        private bool Authorize(string apiUrl, string apiKeyPrivate, string apiKeyPublic)
        {
 	        apiAuth = new ApiHandler(apiUrl,
                                    apiKeyPrivate,
                                    apiKeyPublic);
            apiAuth.Authorize();
            return apiAuth.IsAuthenticated;
        }

        private void SetButtonState(bool status)
        {
            btnStart.Enabled = status && !server.IsRunning;
            btnStop.Enabled = status && server.IsRunning;
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            bool result = Authorize(Settings.Default.ApiUrl, Settings.Default.ApiKeyPrivate, Settings.Default.ApiKeyPublic);

            SetButtonState(result);

            if (result)
            {
                lblStatus.Text = MSG_AUTH_SUCCESS;
            }
            else
            {
                lblStatus.Text = MSG_AUTH_FAILED;
                Settings.Default.TokenValue = "";
                Settings.Default.Save();
            }
            txtApiUrl.Text = Settings.Default.ApiUrl;
            txtApiKeyPublic.Text = Settings.Default.ApiKeyPublic;
            txtApiKeyPrivate.Text = Settings.Default.ApiKeyPrivate;
            txtTokenValue.Text = Settings.Default.TokenValue;
            chkUseAutosave.Checked = Settings.Default.UseAutosave;
            txtAutosaveInterval.Text = Convert.ToString(Settings.Default.AutosaveInterval);
        }

        private void btnCopyTokenValue_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtTokenValue.Text);
        }

        private void txtAutosaveInterval_TextChanged(object sender, EventArgs e)
        {
            int interval = 5;
            int.TryParse(txtAutosaveInterval.Text, out interval);
            if (interval < 1 || interval > 3600) // if less than 1 second or greater than 1 hour, then set it to be 5 seconds
            {
                interval = 5;
                txtAutosaveInterval.Text = interval.ToString();
            }
            else
            {
                Settings.Default.AutosaveInterval = interval;
                Settings.Default.Save();
                server.SetAutoSaveIntervalSeconds(Settings.Default.AutosaveInterval);
            }
        }

        private void chkUseAutosave_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.UseAutosave = chkUseAutosave.Checked;
            Settings.Default.Save();
            server.UseAutoSave = Settings.Default.UseAutosave;
        }

        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Reload();
        }
    }
}
