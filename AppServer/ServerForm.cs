using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SoundTransport;

namespace AppServer
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
        }


        private Boolean isStarted = false;
        private ServerRecorder serverRecorder;


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

        private void startServer()
        {
            stopServer();

            serverRecorder = new ServerRecorder(new ServerTcp((ushort) numPort.Value));
            serverRecorder.Start((int) numExposure.Value);
            isStarted = true;

            addLog("Server started (port=" + numPort.Value + ", exposure=" + numExposure.Value + ")");
        }

        private void stopServer()
        {
            if (!isStarted)
                return;

            serverRecorder.Server.Stop();
            serverRecorder.Stop();
            isStarted = false;

            addLog("Server stopped");
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopServer();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            addLog(DateTime.Now.ToLongTimeString() + "  " + DateTime.Now.ToLongDateString());

            stopServer();
            startServer();
        }
    }
}
