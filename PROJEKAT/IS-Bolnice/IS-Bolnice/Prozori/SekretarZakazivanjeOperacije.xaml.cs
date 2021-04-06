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
    /// Interaction logic for SekretarZakazivanjeOperacije.xaml
    /// </summary>
    public partial class SekretarZakazivanjeOperacije : Window
    {
        private BazaLekara bl;
        private List<Lekar> lekari;
        private Pacijent pacijent;

        public SekretarZakazivanjeOperacije()
        {
            InitializeComponent();
            // Lazar treba da doda dobavljanje lekara specijalista
        }

        private void comboLekari_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboSala_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_Odaberi_Termin(object sender, RoutedEventArgs e)
        {

        }
    }
}
