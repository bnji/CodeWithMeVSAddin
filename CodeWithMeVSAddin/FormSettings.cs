using CodeWithMe;
using CodeWithMeVSAddin.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeWithMeVSAddin
{
    public partial class FormSettings : Form
    {
        private CodeWithMeServer server;

        public FormSettings()
        {
            InitializeComponent();
        }

        public FormSettings(CodeWithMeServer _server) : this()
        {
            server = _server;
            txtEmail.Text = Settings.Default.Email;

            txtApiKeyPublic.Text = Settings.Default.ApiKeyPublic;
            txtApiKeyPrivate.Text = Settings.Default.ApiKeyPrivate;

            txtDbHost.Text = Settings.Default.DBHost;
            txtDbPort.Text = Settings.Default.DBPort.ToString();
            txtDbName.Text = Settings.Default.DBName;
            txtDbUsername.Text = Settings.Default.DBUsername;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            server.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            server.Stop();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            string apiKeyPrivate = txtApiKeyPublic.Text;
            string apiKeyPublic = txtApiKeyPrivate.Text;
            string apiUrl = txtApiUrl.Text;
            ApiHandler apiAuth = new ApiHandler(apiUrl, apiKeyPrivate, apiKeyPublic);
            if (apiAuth.IsAuthenticated)
            {
                lblStatus.Text = "Successfully authorized! Token: " + apiAuth.TokenValue;
                Settings.Default.ApiKeyPublic = apiKeyPrivate;
                Settings.Default.ApiKeyPrivate = apiKeyPublic;
                Settings.Default.ApiUrl = apiUrl;
                Settings.Default.Save();
                //server.Start();
            }
            else
            {
                lblStatus.Text = "Authorization failed. Please check your API keys!";
            }
            /*var email = txtEmail.Text;
            var password = Common.Sha1Hash(mtxtPassword.Text);
            if (server.CheckUserCredentials(new UserCredentials(email, password)))
            {
                Settings.Default.Email = email;
                Settings.Default.Password = password;
                Settings.Default.ApiKeyPublic = txtApiKeyPublic.Text;
                Settings.Default.ApiKeyPrivate = txtApiKeyPrivate.Text;
                Settings.Default.Save();
                server.Start();
                MessageBox.Show("Success!", "Successfully updated your account settings!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                MessageBox.Show("Couldn't connect!", "Email or password is incorrect...", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }*/
        }

        private void btnSaveDbSettings_Click(object sender, EventArgs e)
        {
            var dbHost = txtDbHost.Text;
            var dbPort = 3306;
            Int32.TryParse(txtDbPort.Text, out dbPort);
            var dbName = txtDbName.Text;
            var dbUsername = txtDbUsername.Text;
            var dbPassword = Common.Sha1Hash(mtxtDbPassword.Text);
            if (server.CheckServerCredentials(new StorageCredentials(dbHost, dbPort, dbName, dbUsername, dbPassword)))
            {
                Settings.Default.DBHost = dbHost;
                Settings.Default.DBPort = dbPort;
                Settings.Default.DBName = dbName;
                Settings.Default.DBUsername = dbUsername;
                Settings.Default.DBPassword = dbPassword;
                Settings.Default.Save();
                //server.Start();
                MessageBox.Show("Success!", "Successfully updated your server settings!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                MessageBox.Show("Couldn't connect!", "Please check your settings...", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            tabControl1.TabPages.RemoveAt(2);
        }

        private void btnStart_Click_1(object sender, EventArgs e)
        {
            server.Start();
        }

        private void btnStop_Click_1(object sender, EventArgs e)
        {
            server.Stop();
        }
    }
}
