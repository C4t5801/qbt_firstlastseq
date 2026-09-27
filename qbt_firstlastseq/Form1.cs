using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization; // Requires reference to System.Web.Extensions; Project => Add Reference... => Assemblies => Framework => [v] System.Web.Extensions
using System.Windows.Forms;

namespace qbt_firstlastseq
{
    public partial class Form1 : Form
    {
        private readonly string _torrentHash;
        private readonly string _apiBase;
        private TextBox txtLog;

        public Form1(string torrentHash, string port)
        {
            _torrentHash = torrentHash;
            _apiBase = $"http://127.0.0.1:{port}/api/v2"; // Dynamically inject the port
            InitializeComponent();
            SetupCustomTextBox();
        }

        private void SetupCustomTextBox()
        {
            this.Icon = qbt_firstlastseq.Properties.Resources.qbittorrent;
            this.Text = "qbt_firstlastseq  v" + System.Windows.Forms.Application.ProductVersion + "   -   qBittorrent: Set FirstLast + Sequential";
            this.Size = new System.Drawing.Size(1020, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            txtLog = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Multiline = true,
                ScrollBars = ScrollBars.None,
                ReadOnly = true,
                Dock = DockStyle.Fill,

                // Colors & Font:
                BackColor = System.Drawing.Color.FromArgb(46, 48, 50),
                ForeColor = System.Drawing.Color.FromArgb(164, 196, 229),
                Font = new System.Drawing.Font("Consolas", 14)

            };
            this.Controls.Add(txtLog);
        }

        private void Log(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => Log(message)));
                return;
            }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (string.IsNullOrWhiteSpace(_torrentHash))
            {
                Log("⚠️ Error: No torrent hash argument (%K) passed to this program.");
                Log("Keeping window open for review.");
                return;
            }

            Log($"🚀 Target API URL: {_apiBase}");
            Log($"🚀 Target Torrent Hash: {_torrentHash}");

            bool success = await ProcessTorrentAsync();

            if (success)
            {
                Log("🏁 All operations completed successfully! Closing window in 5 seconds...");
                await Task.Delay(5000); // Small Exit delay so you see the final success text.
                Application.Exit();
            }
            else
            {
                Log("❌ Task Failed.");
                Log("Keeping window open for review.");
            }
        }

        private async Task<bool> ProcessTorrentAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                Dictionary<string, object> torrent = null;

                // 1. Retry Loop
                for (int i = 1; i <= 5; i++)
                {
                    Log($"📡 Attempt {i}/5: Querying qBittorrent WebUI...");
                    try
                    {
                        string response = await client.GetStringAsync($"{_apiBase}/torrents/info?hashes={_torrentHash}");

                        var serializer = new JavaScriptSerializer();
                        var array = serializer.Deserialize<List<object>>(response);

                        if (array != null && array.Count > 0)
                        {
                            torrent = array[0] as Dictionary<string, object>;
                            if (torrent != null)
                            {
                                Log("✅ Successfully fetched torrent metadata!");
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log($"⚠️ Connection error: {ex.Message}");
                    }

                    await Task.Delay(1000);
                }

                if (torrent == null)
                {
                    Log("❌ Failure: Could not reach qBittorrent or torrent data is missing after 5 retries.");
                    return false;
                }

                try
                {
                    // 2. Sequential Download
                    bool isSeqDl = Convert.ToBoolean(torrent["seq_dl"]);
                    Log($"ℹ️ Current Sequential Download: {isSeqDl}");
                    if (!isSeqDl)
                    {
                        Log("⚡ Toggling Sequential Download ON...");
                        var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("hashes", _torrentHash) });
                        var result = await client.PostAsync($"{_apiBase}/torrents/toggleSequentialDownload", content);
                        Log($"🔹 Server response: {result.StatusCode}");
                    }

                    // 3. First/Last Piece Priority
                    bool isFirstLastPrio = Convert.ToBoolean(torrent["f_l_piece_prio"]);
                    Log($"ℹ️ Current First/Last Piece Priority: {isFirstLastPrio}");
                    if (!isFirstLastPrio)
                    {
                        Log("⚡ Toggling First/Last Piece Priority ON...");
                        var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("hashes", _torrentHash) });
                        var result = await client.PostAsync($"{_apiBase}/torrents/toggleFirstLastPiecePrio", content);
                        Log($"🔹 Server response: {result.StatusCode}");
                    }

                    return true; // Successfully completed everything without exceptions
                }
                catch (Exception ex)
                {
                    Log($"❌ Exception running API commands: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
