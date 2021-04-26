using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;

using System.Threading;

using System.Diagnostics;

using System.Windows;

using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System.Linq;

namespace EyeTracking
{
    class CameraServer
    {
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IAsyncResult asyncResult;
        List<Socket> Cameras = new List<Socket>();

        public CameraServer()
        {
            server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public CameraServer(string IP,int port,int backlog = 5)
        {
            server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
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

        public bool Bind(string IP , int port)
        {
            if(server == null && server.IsBound)
                return false;

            IPAddress ia;

            if (IP == "")
                ia = IPAddress.Any;
            else
                ia = IPAddress.Parse(IP);

            IPEndPoint ep = new IPEndPoint(ia, port);
            server.Bind(ep);

            return true;
        }
        
        public void Listen(int backlog)
        {
            server.Listen(backlog);
        }

        public void Close()
        {
            foreach (Socket client in Cameras)
            {
                if (client.Connected)
                    client.Close();
            }

            if (server.Connected)
                server.Close();
        }


        
        public void BeginAccepting()
        {
            SocketAsyncEventArgs event_accept = new SocketAsyncEventArgs();
            event_accept.Completed += new EventHandler<SocketAsyncEventArgs>(Client_Accepted);
            server.AcceptAsync(event_accept);
        }
        
        private void Client_Accepted(object sender, SocketAsyncEventArgs e)
        {
            Socket client = e.AcceptSocket;

            Cameras.Add(client);

            SocketAsyncEventArgs event_accept = new SocketAsyncEventArgs();
            event_accept.Completed += new EventHandler<SocketAsyncEventArgs>(Client_Disconnected);
            client.DisconnectAsync(event_accept);

            try
            {
                MessageBox.Show("New camera is accepted");

                e.AcceptSocket = null;
                server.AcceptAsync(e);
            }
            catch(SocketException err)
            {
                Trace.WriteLine(string.Format("Socket Exception : {0}",err.Message));
                Cameras.Remove(client);
            }
            catch(Exception err)
            {
                Trace.WriteLine(string.Format("Exception : {0}", err.Message));
                Cameras.Remove(client);
            }
        }

        private void Client_Disconnected(object sender,SocketAsyncEventArgs e)
        {
            MessageBox.Show("Client is disconnected");
            Cameras.Remove(sender as Socket);
        }

        public void SendAll(string msg)
        {
            foreach(Socket client in Cameras)
            {
                client.Send(Encoding.UTF8.GetBytes(msg + '\0'));
            }
        }


        private void SendInt(Socket sock, int number)
        {
            sock.Send(UTF8Encoding.UTF8.GetBytes(number.ToString() + '\0'));
        }

        private int RecvInt(Socket sock)
        {
            return int.Parse(RecvString(sock));
        }

        private String RecvString(Socket sock)
        {
            int RecvSize = 0;
            List<byte> lstData = new List<byte>();

            while (true)
            {
                byte[] data = new byte[1];
                RecvSize = sock.Receive(data);
                if (data[0] == 0)
                    break;
                lstData.Add(data[0]);
            }

            return UTF8Encoding.UTF8.GetString(lstData.ToArray());
        }

        private void SendString(Socket sock,string msg)
        {
            sock.Send(Encoding.UTF8.GetBytes(msg + '\0'));
        }

        private Mat RecvImage(Socket sock)
        {
            List<byte> lstData = new List<byte>();
            int ImgSize = RecvInt(sock);

            if (ImgSize == 0)
                return null;

            while (ImgSize > lstData.Count())
            {
                byte[] bData = new byte[ImgSize - lstData.Count()];

                int RecvSize = sock.Receive(bData);
                Array.Resize(ref bData, RecvSize);
                lstData.AddRange(bData);
            }

            Mat Img = Mat.ImDecode(lstData.ToArray());

            return Img;
        }
    }
}
