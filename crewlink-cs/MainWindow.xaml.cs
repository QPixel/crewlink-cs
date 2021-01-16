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
using crewlink.memoryreader;

namespace crewlink
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameReader gameReader;
        public MainWindow()
        {
            InitializeComponent();
            gameReader = new GameReader();
        }

        private void OnVoiceTestClick(object sender, RoutedEventArgs e)
        {
            //.WriteLine("BUTTON CLICKED");
            voicebtn.Background = Brushes.LightBlue;
            gameReader.test();
        }
    }
}