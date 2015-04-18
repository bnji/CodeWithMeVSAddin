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
            var email = txtEmail.Text;
            var password = Common.Sha1Hash(mtxtPassword.Text);
            if (server.CheckUserCredentials(new UserCredentials(email, password)))
            {
                Settings.Default.Email = email;
                Settings.Default.Password = password;
                Settings.Default.Save();
                server.Start();
                MessageBox.Show("Successfully updated account settings!");
            }
            else
            {
                MessageBox.Show("Email or password is incorrect!");
            }
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
                server.Start();
                MessageBox.Show("Successfully updated server settings!");
            }
            else
            {
                MessageBox.Show("Email or password is incorrect!");
            }
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {

        }
    }
}
