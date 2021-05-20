using IS_Bolnice.Baze;
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
using IS_Bolnice.Kontroleri;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarProduzenjeHospitalizacije.xaml
    /// </summary>
    public partial class LekarProduzenjeHospitalizacije : Window
    {
        private Hospitalizacija hospitalizacija;
        private HospitalizacijaKontroler hospitalizacijaKontroler = new HospitalizacijaKontroler();
        public LekarProduzenjeHospitalizacije(Hospitalizacija hospitalizacijaZaIzmenu)
        {
            InitializeComponent();
            datumProduzenja.DisplayDateStart = hospitalizacijaZaIzmenu.KrajHospitalizacije;
            datumProduzenja.SelectedDate = hospitalizacijaZaIzmenu.KrajHospitalizacije;
            hospitalizacija = hospitalizacijaZaIzmenu;
        }

        private void ButtonClick_Produzi(object sender, RoutedEventArgs e)
        {
            hospitalizacijaKontroler.ObrisiHospitalizaciju(hospitalizacija);
            hospitalizacija.KrajHospitalizacije = datumProduzenja.SelectedDate.Value;
            hospitalizacijaKontroler.KreirajHospitalizaciju(hospitalizacija);

            MessageBox.Show("Hospitalizacije uspešno produžena", "Produžena hospitalizacija",
                MessageBoxButton.OK);
            this.Close();
        }

        private void ButtonClickOdustati(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
