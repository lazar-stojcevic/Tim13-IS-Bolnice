using IS_Bolnice.Repozitorijumi;
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
using IS_Bolnice.Kontroleri.Termini;
using IS_Bolnice.Servisi;
using IS_Bolnice.Servisi.Ustanova;
using WPFCustomMessageBox;

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
            hospitalizacija.KrajHospitalizacije = datumProduzenja.SelectedDate.Value;
            DateTime validFrom = hospitalizacija.PocetakHospitalizacije;
            DateTime validTo = hospitalizacija.KrajHospitalizacije;
            for (DateTime dt = validFrom; dt <= validTo; dt = dt.AddDays(1))
            {
                if (new RenovacijaServis().SalaNaRenoviranjuUOdredjenomPeriodu(hospitalizacija.Soba.Id, dt))
                {
                    CustomMessageBox.Show("Hospitalizacije se ne može produžiti zbog renoviranja sobe", "Hospitalizacija se ne može produžiti",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            hospitalizacijaKontroler.ObrisiHospitalizaciju(hospitalizacija);
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
