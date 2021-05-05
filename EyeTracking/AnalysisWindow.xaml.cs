using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Net;
using System.Net.Sockets;

using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System.ComponentModel;
using System.Diagnostics;

using System.Threading;

namespace EyeTracking
{
    /// <summary>
    /// AnalysisWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AnalysisWindow : System.Windows.Window
    {
        AnalysisData data = new AnalysisData()
        {
            bStart = false
        };

        CameraClient cap_Face = new CameraClient();
        CameraClient cap_LeftEye = new CameraClient();
        CameraClient cap_RightEye = new CameraClient();

        Thread td_recvFrame;

        public AnalysisWindow()
        {
            InitializeComponent();

            this.DataContext = data;

            try
            {
                cap_Face.Connect("127.0.0.1", 8456);
                cap_LeftEye.Connect("127.0.0.1", 8457);
                cap_RightEye.Connect("127.0.0.1", 8458);
            }
            catch(SocketException err)
            {
                MessageBox.Show(err.Message);
            }


            cap_Face.ReceivedFrame += (object obj, EventArgs arg) =>
            {
                Dispatcher.Invoke((Action)(() =>
                {
                    img_Face.Source = BitmapSourceConverter.ToBitmapSource((arg as FrameCallbackArg).frame);
                }));
            };

            cap_LeftEye.ReceivedFrame += (object obj, EventArgs arg) =>
            {
                Dispatcher.Invoke((Action)(() =>
                {
                    img_LeftEye.Source = BitmapSourceConverter.ToBitmapSource((arg as FrameCallbackArg).frame);
                }));
            };

            cap_RightEye.ReceivedFrame += (object obj, EventArgs arg) =>
            {
                Dispatcher.Invoke((Action)(() =>
                {
                    img_RightEye.Source = BitmapSourceConverter.ToBitmapSource((arg as FrameCallbackArg).frame);
                }));
            };


            td_recvFrame = new Thread(ThreadFunc_RecvFrame);
            td_recvFrame.IsBackground = true;
            td_recvFrame.Start();

        }

        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {

            if (data.bStart == false)
            {
                data.bStart = true;
            }
            else
            {
                data.bStart = false;
            }
        }

        private void ThreadFunc_RecvFrame()
        {
            try
            {
                while (cap_Face.isConnected)
                {

                    cap_Face.GetFrame();
                    cap_LeftEye.GetFrame();
                    cap_RightEye.GetFrame();

                    Cv2.WaitKey(15);
                }
            }
            catch(Exception err)
            {

            }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DockPanel_Unloaded(object sender, RoutedEventArgs e)
        {
            cap_Face.Close();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }

    public class AnalysisData : INotifyPropertyChanged
    {
        public string name { get; set; }

        int dist;
        public int pupil_distance
        {
            get
            {
                return dist;
            }
            set
            {
                dist = value;
                NotifyPropertyChanged("pupil_distance");
            }
        }

        private bool pbStart = false;
        public bool bStart
        {
            get
            {
                return pbStart;
            }
            set
            {
                pbStart = value;
                NotifyPropertyChanged("bStart");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
