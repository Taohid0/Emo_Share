using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace EMO_SHARE_SERVER
{
    public partial class Form1 : Form
    {
        private delegate void UpdateStatusCallback(string strMessage);

        public Form1()
        {
            InitializeComponent();
           
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            IPAddress ipAddr = IPAddress.Parse(txtIp.Text);

            ChatServer mainServer = new ChatServer(ipAddr);
            ChatServer.StatusChanged += new StatusChangedEventHandler(mainServer_StatusChanged);

            mainServer.StartListening();
            txtLog.AppendText("Monitoring for connections....\r\n");

        }
        public void mainServer_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            try
            {
                this.Invoke(new UpdateStatusCallback(this.UpdateStatus), new object[] { e.EventMessage });
            }
            catch(Exception )
            {
                MessageBox.Show("here");
                Application.Exit();
            }
        }
        public void UpdateStatus(string strMessage)
        { 
            txtLog.AppendText(strMessage + "\r\n");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(1);
            Application.ExitThread();
            Application.Exit();
        }
    }
}
