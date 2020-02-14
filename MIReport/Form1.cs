using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using WSShared;

namespace MIReport
{

    public partial class Form1 : Form
    {
        SettingsFile settings;
        String id_token = String.Empty;

        public Form1()
        {
            InitializeComponent();
            txtSettingsFile.Text = Properties.Settings.Default.SettingsFileName;
            txtSaveTo.Text = Properties.Settings.Default.SaveToFolder;
            btnRun.Focus();
            LoadSettings();
        }

        private void btnPickSettingsFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (File.Exists(txtSettingsFile.Text))
                dlg.FileName = txtSettingsFile.Text;
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            txtSettingsFile.Text = dlg.FileName;
            Properties.Settings.Default.SettingsFileName = txtSettingsFile.Text;
            Properties.Settings.Default.Save();
        }
        private void btnPickSaveTo_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (Directory.Exists(txtSaveTo.Text))
                dlg.SelectedPath = txtSaveTo.Text;
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            txtSaveTo.Text = dlg.SelectedPath;
            Properties.Settings.Default.SaveToFolder = txtSaveTo.Text;

            Properties.Settings.Default.Save();
            LoadSettings();
        }

        private bool CheckSettings()
        {
            if (settings == null)
            {
                LoadSettings();
                if (settings == null)
                    return false;
            }
            return true;
        }
        private void LoadSettings()
        {
            try
            {
                settings = SettingsFile.Load(txtSettingsFile.Text);
                SayUser("Settings loaded " + settings.root);
            }
            catch (Exception ex)
            {
                settings = null;
                SayUser(ex);
            }
        }

        private void SayUser(Exception ex)
        {
            SayUser( String.Format( "ERROR {0}", ex.Message ) );
        }
        private void SayUser( string msg )
        {
            lblSayuser.Text = msg;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (!CheckSettings())
                return;
            if (txtSaveTo.Text == String.Empty || !Directory.Exists(txtSaveTo.Text))
            {
                SayUser("Choose the output folder first");
                return;
            }
            _ = MIReport(txtSaveTo.Text);
        }



        private void btnTest_Click(object sender, EventArgs e)
        {
            if (!CheckSettings())
                return;
            _ = UserMyDetails();
        }

        private HttpClient NewClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(settings.root);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if( id_token != String.Empty )
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", id_token);
            return client;
        }

        private HttpClient NewClientReturningBytes()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(settings.root);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
            if (id_token != String.Empty)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", id_token);
            return client;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!CheckSettings())
                return;
            _ = OIDCRefresh();
        }

        private async Task OIDCRefresh()
        {
            var client = NewClient();
            string json = String.Empty, req = String.Empty;
            try
            {
                req = "/sync/" + settings.bucket + "/_oidc_refresh?refresh_token=" + settings.renewableToken;
                json = await client.GetStringAsync(req);
                OIDCResult oidc = JsonConvert.DeserializeObject<OIDCResult>(json);
                id_token = oidc.id_token;
                SayUser(String.Format("Loaded id token, {0} bytes", id_token.Length));
            }
            catch (Exception ex)
            {
                SayUser(ex);
                Console.WriteLine(ex.Message);
                Console.WriteLine(req);
                Console.WriteLine(json);
            }
        }

        private async Task UserMyDetails()
        {
            if( id_token == String.Empty )
            {
                SayUser("Connect first");
                return;
            }
            try
            {
                var client = NewClient();
                string json = await client.GetStringAsync("/api/user/myDetails");
                UserMyDetailsResult result = JsonConvert.DeserializeObject<UserMyDetailsResult>(json);
                SayUser( String.Format( "{0} / {1} / {2}", result.username, result.role, result.companyId ) );
            }
            catch (Exception ex)
            {
                SayUser(ex);
            }
        }

        private async Task MIReport( string saveToFolder )
        {
            if (id_token == String.Empty)
            {
                SayUser("Connect first");
                return;
            }
            try
            {
                string filename = "WhitespacePlatformMIReport_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
                Console.WriteLine("calling /api/mireport, writing to " + filename);
                var client = NewClientReturningBytes();
                var startTime = DateTime.Now;
                var bytes = await client.GetByteArrayAsync("/api/mireport");
                var endTime = DateTime.Now;
                TimeSpan ts = endTime - startTime;
                var msg = String.Format("{0} bytes written to {1} in {2} seconds", bytes.Length, filename, ts.TotalSeconds);
                Console.WriteLine(msg);
                SayUser(msg);
                using( var fs = new FileStream(Path.Combine(saveToFolder, filename), FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                SayUser(ex);
            }
        }

    }

    public class OIDCResult
    {
        public string id_token;
        public string session_id;
        public string name;
    }
}
