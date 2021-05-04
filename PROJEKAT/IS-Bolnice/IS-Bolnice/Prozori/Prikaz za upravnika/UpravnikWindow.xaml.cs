using IS_Bolnice.Prozori.UpravnikPages;
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

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for UpravnikWindow.xaml
    /// </summary>
    public partial class UpravnikWindow : Window
    {
        public UpravnikWindow()
        {
            InitializeComponent();
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NalogMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Page nalog = new NalogPage();
            this.frame.NavigationService.Navigate(nalog);
        }

        private void ProstorijeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Page sala = new SalePage();
            this.frame.NavigationService.Navigate(sala);
        }

        private void OpremaMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Page oprema = new OpremaPage();
            this.frame.NavigationService.Navigate(oprema);
        }

        private void LekoviMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
