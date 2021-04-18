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
using System.Windows.Shapes;

namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for OpremaWindow.xaml
    /// </summary>
    public partial class OpremaWindow : Window
    {
        public OpremaWindow()
        {
            InitializeComponent();
        }


        private void Button_Upravljanje(object sender, RoutedEventArgs e)
        {
            UpravljanjeWindow upravljanjeWindow = new UpravljanjeWindow();
            upravljanjeWindow.Show();
        }

        private void Button_Nabavka(object sender, RoutedEventArgs e)
        {
            NabavkaOpremeWindow nabavkaOpremeWindow = new NabavkaOpremeWindow();
            nabavkaOpremeWindow.Show();
            this.Close();
        }

        private void Button_Preraspodela(object sender, RoutedEventArgs e)
        {
            PreraspodelaWindow preraspodelaWindow = new PreraspodelaWindow();
            preraspodelaWindow.Show();
        }
    }
}
