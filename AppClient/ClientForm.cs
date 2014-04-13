using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SoundTransport;

namespace AppClient
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
        }


        private Boolean isStarted = false;
        private ClientTcp clientTcp;
        private ClientPlayer clientPlayer;


        private void addLog(String msg)
        {
            List<String> lines = txtLog.Lines.ToList();
            lines.Add("> " + msg);
            while (lines.Count > 3)
            {
                lines.RemoveAt(0);
            }
            txtLog.Lines = lines.ToArray();
        }

        private void startClient()
        {
            stopClient();

            clientTcp = new ClientTcp(txtHost.Text.Trim(), (int) numPort.Value);
            clientPlayer = new ClientPlayer(clientTcp);
            isStarted = true;

            addLog("Client started (host=" + txtHost.Text.Trim() + ", port=" + numPort.Value + ")");
        }

        private void stopClient()
        {
            if (!isStarted)
                return;

            clientTcp.Stop();
            clientPlayer.Stop();
            isStarted = false;

            addLog("Client stopped");
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopClient();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            addLog(DateTime.Now.ToLongTimeString() + "  " + DateTime.Now.ToLongDateString());

            startClient();
        }
    }
}
