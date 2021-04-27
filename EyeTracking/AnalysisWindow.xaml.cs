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
        Thread td_recvFrame;

        public AnalysisWindow()
        {
            InitializeComponent();

            grid_Control.DataContext = data;

            cap_Face.Connect("127.0.0.1", 8456);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {

            if (data.bStart == false)
            {
                data.bStart = true;

                td_recvFrame = new Thread(ThreadFunc_RecvFrame);
                td_recvFrame.IsBackground = true;
                td_recvFrame.Start();
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
                while (data.bStart)
                {
                    Dispatcher.Invoke((Action)(() =>
                    {
                        img_Face.Source = BitmapSourceConverter.ToBitmapSource(cap_Face.GetFrame());
                    }));
                    Cv2.WaitKey(16);
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
