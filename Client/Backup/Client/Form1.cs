using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Client
{
    public partial class Form1 : Form
    {
        TcpClient client = null;
        NetworkStream stream = null;
        StreamReader streamReader = null;
        StreamWriter streamWriter = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
             client = new TcpClient("127.0.0.1", 8000);
        
             stream = client.GetStream();
             streamReader = new StreamReader(stream);
             streamWriter = new StreamWriter(stream);
             streamWriter.WriteLine(userBox.Text + "#");
             streamWriter.Flush();
             stream.Close();
             client.Close();
        }
    }
}
