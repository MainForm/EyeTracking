using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EyeTracking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_Test_Click(object sender, RoutedEventArgs e)
        {
            AnalysisWindow winAnalysis = new AnalysisWindow();
            //창을 숨김
            this.Visibility = Visibility.Collapsed;
            winAnalysis.ShowDialog();
            //창을 다시 표시
            this.Visibility = Visibility.Visible;
        }

        private void btn_Play_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
