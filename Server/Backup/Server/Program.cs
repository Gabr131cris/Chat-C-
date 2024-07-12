using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Collections;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(8000);
            TcpClient client = default(TcpClient);
            NetworkStream stream = null;
            StreamReader streamReader = null;
            StreamWriter streamWriter = null;
            String textReceived = null;
            int nrclienti = 0;
            ArrayList users = new ArrayList();

            server.Start();
            Console.WriteLine("Server started!");

            while (true)
            {
                client = server.AcceptTcpClient();
                stream = client.GetStream();
                streamReader = new StreamReader(stream);
                streamWriter = new StreamWriter(stream);
                textReceived = streamReader.ReadLine();
                if (textReceived.Substring(textReceived.Length - 1) == "#")
                {
                    String nume = textReceived.Substring(0, textReceived.Length - 1);
                    bool gasit = false;
                    if (users.Contains(nume))
                        {
                            gasit = true;
                            streamWriter.WriteLine("User existent");
                            streamWriter.Flush();
                        }
                    if (!gasit)
                    {
                        Console.WriteLine(nume + "joined");
                        nrclienti++;
                        users.Add(nume);
                        streamWriter.WriteLine("Joined");
                    }
                    Console.WriteLine(nrclienti);
                }
                stream.Close();
            }
        }
    }
}
