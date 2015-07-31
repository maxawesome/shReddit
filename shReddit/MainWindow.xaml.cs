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

namespace shReddit
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

        private void ShredButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you certain you want to shred your reddit history?","Shred for real?", MessageBoxButton.YesNo,MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;
            
            var sure = MessageBox.Show("Shredding is irreversible. Are you really, really sure?", "No, seriously...", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (sure == MessageBoxResult.Yes)
            {
                //TODO: Implement
            }
        }
    }
}
