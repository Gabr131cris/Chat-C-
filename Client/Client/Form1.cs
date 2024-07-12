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
using System.Collections;

namespace Client
{
    public partial class Form1 : Form
    {
        TcpClient client = null;
        NetworkStream stream = default(NetworkStream);
        StreamReader streamReader = null;
        StreamWriter streamWriter = null;
        String receivedText;
        bool connected = false;
        String me, s1 = null;
        ArrayList users = new ArrayList();


        public Form1()
        {
            InitializeComponent();
        }


        private void connectBtn_Click(object sender, EventArgs e)
        {

                client = new TcpClient("localhost", 7999);
                stream = client.GetStream();
                connected = true;

                streamWriter = new StreamWriter(stream);
                streamWriter.WriteLine(userBox.Text + "#");
                streamWriter.Flush();

                me = userBox.Text;

                Thread msgThread = new Thread(rcv);
                msgThread.Start();

        }

        public void rcv()
        {
            while (true)
            {
                stream = client.GetStream();
                streamReader = new StreamReader(stream);
                receivedText = streamReader.ReadLine();
                if (receivedText == "User existent")
                {
                    MessageBox.Show("Already connected");
                }
                else
                {
                    if (receivedText.Substring(0, 6) == "Joined" || receivedText.Substring(0, 6) == "Public") 
                    {
                        dispacher();
                    }

                }
            }
        }

        private void dispacher()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(dispacher));
            else
            {
                if (receivedText.Substring(0, 6) == "Joined" || receivedText.Substring(0, 6) == "Public" || receivedText.Substring(0, 7) == "Private")
                {
                    if (receivedText.Substring(0, 6) == "Joined")
                    {
                        chatBox.Text = "You joined the chat!";
                        bunifuButton1.Text = "Disconnect";
                        userList.Items.Clear();
                        s1 = receivedText.Substring(6);
                        while (s1.Contains("."))
                        {
                            userList.Items.Add(s1.Substring(0, s1.IndexOf(".")));
                            s1 = s1.Substring(s1.IndexOf(".") + 1);
                        }
                    }
                    else if (receivedText.Substring(0, 6) == "Public")
                    {
                        chatBox.Text = chatBox.Text + Environment.NewLine + receivedText.Substring(6);
                    }
                    else
                    {
                        chatBox.Text = chatBox.Text + Environment.NewLine + receivedText.Substring(7);
                    }
                }
            }
        }


        private void userList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            client = new TcpClient("localhost", 7999);
            stream = client.GetStream();
            connected = true;

            streamWriter = new StreamWriter(stream);
            streamWriter.WriteLine(userBox.Text + "#");
            streamWriter.Flush();

            me = userBox.Text;

            Thread msgThread = new Thread(rcv);
            msgThread.Start();
        }

        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            stream = client.GetStream();
            streamWriter = new StreamWriter(stream);
            streamWriter.WriteLine(bunifuTextBox1.Text);
            streamWriter.Flush();
        }

        private void userBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void msgBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void bunifuTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void chatBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void chatBox_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void userBox_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            stream = client.GetStream();
            streamWriter = new StreamWriter(stream);

            if (userList.SelectedItems.Count > 0)
            {
                string selectedUser = userList.SelectedItems[0].ToString();
                streamWriter.WriteLine("private:" + selectedUser + ": " + bunifuTextBox1.Text);
            }
            else
            {
                streamWriter.WriteLine(bunifuTextBox1.Text);
            }

            streamWriter.Flush();
        }

    }
}

