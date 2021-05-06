using IS_Bolnice.Baze;
using IS_Bolnice.Model;
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

namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for ZakazivanjeHitnogTermina.xaml
    /// </summary>
    public partial class ZakazivanjeHitnogTermina : Window
    {
        private Pacijent odabraniPacijent = new Pacijent();
        private BazaPacijenata bazaPacijenata = new BazaPacijenata();
        private BazaOblastiLekara bazaOblastiLekara = new BazaOblastiLekara();

        public ZakazivanjeHitnogTermina()
        {
            InitializeComponent();

            PopunjavanjeOblastiLekara();
            PopunjavanjePonudjenihTrajanja();
        }

        private void PopunjavanjeOblastiLekara()
        {
            List<OblastLekara> sveOblastiIzBaze = bazaOblastiLekara.SveOblasti();
            List<string> sveOblastiZaPrikaz = new List<string>();

            foreach (OblastLekara oblast in sveOblastiIzBaze)
            {
                sveOblastiZaPrikaz.Add(oblast.Naziv);
            }
            comboOblastLekara.ItemsSource = sveOblastiZaPrikaz;
        }

        private void PopunjavanjePonudjenihTrajanja()
        {
            List<double> trajanja = new List<double>();
            for (double i = 0.5; i <= 24; i += 0.5)
            {
                trajanja.Add(i);
            }
            comboTrajanja.ItemsSource = trajanja;
        }

        private void UpdateTextBox()
        {
            if (odabraniPacijent != null)
            {
                odabraniPacijentTxt.Text = odabraniPacijent.Ime + " " + odabraniPacijent.Prezime + " (JMBG: " + odabraniPacijent.Jmbg + ")";
            }
        }

        private void Button_Click_Postojeci(object sender, RoutedEventArgs e)
        {
            PrikazSvihPacijenata prikazSvihPacijenata = new PrikazSvihPacijenata(odabraniPacijent);
            prikazSvihPacijenata.ShowDialog();
            UpdateTextBox();
        }

        private void Button_Click_Gostujuci(object sender, RoutedEventArgs e)
        {
            DodavanjeGuestNalogaWindow dodavanjeGostujuceg = new DodavanjeGuestNalogaWindow();
            dodavanjeGostujuceg.ShowDialog();
            // u slucaju dodavanja gostujuceg uzima se poslednji iz baze
            odabraniPacijent = bazaPacijenata.poslednjiDodat();
            UpdateTextBox();
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            if (!ValidnoPopunjenaPolja())
            {
                return;
            }

            if ((bool)rbPregled.IsChecked)
            {
                ZakazivanjePregleda();
            }
            else if ((bool)rbOperacija.IsChecked)
            {

            }
            else
            {
                string message = "Odaberite tip termina!";
                MessageBox.Show(message);
            }
        }

        private bool ValidnoPopunjenaPolja()
        {
            return odabraniPacijent != null && comboOblastLekara.SelectedIndex != -1 && comboTrajanja.SelectedIndex != -1;
        }

        private DateTime NajbliziSlobodanTermin()
        {
            DateTime najbliziTermin = DateTime.Now;
            while (najbliziTermin.Minute % 5 != 0)
            {
                najbliziTermin = najbliziTermin.AddMinutes(1);
            }
            return najbliziTermin;
        }

        private void ZakazivanjePregleda()
        {
            //TODO : dovrsiti i refaktorisati
            DateTime pocetakTermina = NajbliziSlobodanTermin();
            DateTime krajTermina = pocetakTermina.AddHours((double)comboTrajanja.SelectedItem);

            BazaLekara bazaLekara = new BazaLekara();
            List<Lekar> sviLekariOdredjeneOblasti = bazaLekara.LekariOdredjeneOblasti((string)comboOblastLekara.SelectedItem);
            BazaPregleda bazaPregleda = new BazaPregleda();

            Pregled pregled = new Pregled(odabraniPacijent, sviLekariOdredjeneOblasti[0], pocetakTermina, krajTermina);
            bazaPregleda.ZakaziPregled(pregled);
            Close();
        }

        private void ZakazivanjeOperacije()
        {
            //TODO : dovrsiti i refaktorisati
            //DateTime pocetakTermina = NajbliziTermin();
            //DateTime krajTermina = pocetakTermina.AddHours((double)comboTrajanja.SelectedItem);
            //BazaBolnica bazaBolnica = new BazaBolnica();
            
            //BazaLekara bazaLekara = new BazaLekara();
            //List<Lekar> sviLekariOdredjeneOblasti = bazaLekara.LekariOdredjeneOblasti((string)comboOblastLekara.SelectedItem);
            //BazaOperacija bazaOperacija = new BazaOperacija(odabraniPacijent, sviLekariOdredjeneOblasti[0], pocetakTermina, krajTermina, );
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
