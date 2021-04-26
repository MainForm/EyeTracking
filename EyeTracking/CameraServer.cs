﻿using System;
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
    class CameraClient
    {

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
