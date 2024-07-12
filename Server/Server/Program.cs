using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Net;

namespace Server
{
    class Program
    {
        public static Hashtable clientsList = new Hashtable();

        static void Main(string[] args)
        {
            //TcpListener server = new TcpListener(7999);
            TcpListener server = new TcpListener(IPAddress.Parse("0.0.0.0"), 7999);


            //TcpListener server = new TcpListener(IPAddress.Any, 7999); // Modificați numărul portului aici
            // ...

            TcpClient clientSocket = default(TcpClient);
            
            String textReceived = null;
           
            ArrayList users = new ArrayList();

            server.Start();
            Console.WriteLine("Chat Server Started....");

            while (true)
            {
                clientSocket = server.AcceptTcpClient();
                NetworkStream stream = clientSocket.GetStream();
                StreamReader streamReader = new StreamReader(stream);
                StreamWriter streamWriter = new StreamWriter(stream);

                textReceived = streamReader.ReadLine();
                if (textReceived.Substring(textReceived.Length - 1) == "#")
                {
                    String nume = textReceived.Substring(0, textReceived.Length - 1);
                    bool gasit = false;
                    if (users.Contains(nume))
                        {
                            gasit = true;
                            streamWriter.WriteLine("Already connected");
                            streamWriter.Flush();
                        }
                    if (!gasit)
                    {
                        Console.WriteLine(nume + " connected");
                   
                      
                        clientsList.Add(nume, clientSocket);
                        String detrimis = null;
                        foreach (DictionaryEntry Item in clientsList)
                        {
                            detrimis += Item.Key + ".";
                        }
                        Console.WriteLine(detrimis);
                        foreach (DictionaryEntry Item in clientsList)
                        {
                            TcpClient clientSocketul;
                            clientSocketul = (TcpClient)Item.Value;
                            NetworkStream networkStream = clientSocketul.GetStream();
                            StreamWriter networkWriter = new StreamWriter(networkStream);
                            networkWriter.WriteLine("Joined"+detrimis);
                            networkWriter.Flush();
                        }
                       }
                    
                    Console.WriteLine(nume + "joined");
                  
                    handleClinet client = new handleClinet();
                    client.startClient(clientSocket, nume, clientsList);
                }
            }
            clientSocket.Close();
            server.Stop();
            Console.WriteLine("Exit");
            Console.ReadLine();
        }


        public static void privateChat(string msg, string sender, string receiver)
        {
            if (clientsList.ContainsKey(receiver))
            {
                TcpClient privateChatSocket = (TcpClient)clientsList[receiver];
                NetworkStream privateChatStream = privateChatSocket.GetStream();
                StreamWriter privateChatWriter = new StreamWriter(privateChatStream);

                string privateMessage = sender + " (privat) : " + msg;

                privateChatWriter.WriteLine("Private" + privateMessage);
                privateChatWriter.Flush();
            }
        }

        public static  void broadcast(string msg, string uName, bool flag)
        {
            foreach (DictionaryEntry Item in clientsList)
            {
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value;
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                StreamWriter broadcastWriter = new StreamWriter(broadcastStream);
                String broadcast = null;

                if (flag == true)
                {
                    broadcast = uName + " : " + msg;
                }
                else
                {
                    broadcast = msg;
                }

                broadcastWriter.WriteLine("Public"+broadcast);
                broadcastWriter.Flush();
            }
        }  //end broadcast function
    }

    public class handleClinet
    {
        TcpClient clientSocket;
        string clName;
        Hashtable clientsList;

        public void startClient(TcpClient inClientSocket, string clineNo, Hashtable cList)
        {
            this.clientSocket = inClientSocket;
            this.clName = clineNo;
            this.clientsList = cList;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }

        private void doChat()
        {
            string dataFromClient = null;

            while (true)
            {
                try
                {
                    NetworkStream stream = clientSocket.GetStream();
                    StreamReader streamReader = new StreamReader(stream);
                    dataFromClient = streamReader.ReadLine();

                    if (dataFromClient.StartsWith("private:"))
                    {
                        string[] splitMessage = dataFromClient.Substring(8).Split(new char[] { ':' }, 2);
                        if (splitMessage.Length == 2)
                        {
                            string receiver = splitMessage[0].Trim();
                            string message = splitMessage[1].Trim();
                            Console.WriteLine("Private message from client " + clName + " to " + receiver + ": " + message);
                            Program.privateChat(message, clName, receiver);
                        }
                    }
                    else
                    {
                        Console.WriteLine("From client " + clName + " : " + dataFromClient);
                        Program.broadcast(dataFromClient, clName, true);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

    }
}
