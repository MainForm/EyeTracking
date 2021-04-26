using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;

using System.Threading;

using System.Diagnostics;

using System.Windows;

namespace EyeTracking
{
    class CameraServer
    {
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IAsyncResult asyncResult;
        List<Socket> Cameras = new List<Socket>();

        public CameraServer()
        {
        }

        public CameraServer(string IP,int port,int backlog = 5)
        {
            Bind(IP, port);
            Listen(backlog);
        }

        ~CameraServer()
        {
            foreach(Socket client in Cameras)
            {
                if (client.Connected)
                    client.Close();
            }

            if (server.Connected)
                server.Close();
        }

        public bool Bind(string IP, int port)
        {
            if(server == null && server.IsBound)
                return false;

            server.Bind(new IPEndPoint(IPAddress.Parse(IP), port));

            return true;
        }
        
        public void Listen(int backlog)
        {
            server.Listen(backlog);
        }

        
        public void BeginAccepting()
        {
            SocketAsyncEventArgs event_accept = new SocketAsyncEventArgs();

            event_accept.Completed += new EventHandler<SocketAsyncEventArgs>(Client_Accepted);
            server.AcceptAsync(event_accept);
        }
        
        private void Client_Accepted(object obj,SocketAsyncEventArgs e)
        {
            Socket client = e.AcceptSocket;

            Cameras.Add(client);

            try
            {
                MessageBox.Show("New camera is accepted");

            }
            catch(SocketException err)
            {
                Trace.WriteLine(string.Format("Socket Exception : {0}",err.Message));
            }
            catch(Exception err)
            {
                Trace.WriteLine(string.Format("Exception : {0}", err.Message));
            }
            finally
            {
                Cameras.Remove(client);
            }
        }


    }
}
