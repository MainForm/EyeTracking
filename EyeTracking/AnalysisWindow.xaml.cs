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

namespace EyeTracking
{
    /// <summary>
    /// AnalysisWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AnalysisWindow : System.Windows.Window
    {
        public AnalysisWindow()
        {
            InitializeComponent();
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

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
