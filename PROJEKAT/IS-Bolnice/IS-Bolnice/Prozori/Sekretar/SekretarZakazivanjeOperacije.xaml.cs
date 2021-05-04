using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for SekretarZakazivanjeOperacije.xaml
    /// </summary>
    public partial class SekretarZakazivanjeOperacije : Window
    {
        private BazaLekara bazaLekara;
        private BazaBolnica bazaBolnica;
        private BazaOperacija bazaOperacija;
        private List<Lekar> lekari;
        private List<Bolnica> bolnice;
        private List<Soba> sobe;
        private Pacijent pacijent;
        private Operacija operacija;


        public SekretarZakazivanjeOperacije(Pacijent p)
        {
            InitializeComponent();

            bazaLekara = new BazaLekara();
            bazaBolnica = new BazaBolnica();
            bazaOperacija = new BazaOperacija();

            lekari = bazaLekara.LekariSpecijalisti();
            bolnice = bazaBolnica.SveBolnice();
            sobe = new List<Soba>();
            operacija = new Operacija();
            List<Soba> sveSobe = bolnice[0].Soba; // za sada se podrazumeva da postoji samo jedna bolnica

            foreach(Soba s in sveSobe)
            {
                // samo operacione sale koje nisu obrisane i koje se trenutno ne renoviraju
                if (!(s.PodRenoviranje || s.Obrisano) && (s.Tip == RoomType.operacionaSala))
                {
                    sobe.Add(s);
                }
            }

            pacijent = p;
            operacija.Pacijent = p;

            txtIme.Text = pacijent.Ime;
            txtPrezime.Text = pacijent.Prezime;
            txtJmbg.Text = pacijent.Jmbg;

            List<string> lekariString = new List<string>();
            // formiranje stringa za svakog lekara
            foreach (Lekar l in lekari)
            {
                string lekarString = l.Ime + " " + l.Prezime + " (" + l.Oblast.Naziv + ")";
                lekariString.Add(lekarString);
            }

            comboLekari.ItemsSource = lekariString;
            comboLekari.SelectedIndex = -1;

            List<string> sobeString = new List<string>();
            // formiranje stringa za svaku sobu
            foreach (Soba s in sobe)
            {
                string sobaString = s.Id + " " + s.Tip.ToString();
                sobeString.Add(sobaString);
            }
            comboSale.ItemsSource = sobeString;
            comboSale.SelectedIndex = -1;

            // popunjavanje ponudjenih trajanja
            List<double> trajanja = new List<double>();
            for (double i = 0.5; i <= 24; i += 0.5)
            {
                trajanja.Add(i);
            }
            comboTrajanja.ItemsSource = trajanja;
            comboTrajanja.IsEnabled = false;

            // inicijalno dugme za novi termin nije dostupno dok se ne odabere lekar
            odabirTermina.IsEnabled = false;
        }

        private void comboLekari_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            odabirTermina.IsEnabled = true;

            Lekar lekar = lekari[comboLekari.SelectedIndex];
            operacija.Lekar = lekar;
        }

        private void comboSala_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Soba sala = sobe[comboSale.SelectedIndex];
            operacija.Soba = sala;
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            if (comboLekari.SelectedIndex != -1 && comboSale.SelectedIndex != -1 && operacija.VremePocetkaOperacije != null)
            {
                bazaOperacija.ZakaziOperaciju(operacija);
                this.Close();
            }
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_Odaberi_Termin(object sender, RoutedEventArgs e)
        {
            // na prvom mestu je pocetak termina a na drugom kraj termina
            List<DateTime> termin = new List<DateTime>();
            Lekar lekar = lekari[comboLekari.SelectedIndex];
            OdredjivanjeTermina ot = new OdredjivanjeTermina(termin, lekar);
            ot.ShowDialog();

            // unutar liste termin moze da se nalazi samo pocetak i samo kraj termina
            if (termin.Count == 2)
            {
                operacija.VremePocetkaOperacije = termin[0];
                operacija.VremeKrajaOperacije = termin[1];
                comboTrajanja.IsEnabled = true;
                txtTermin.Text = termin[0].ToString();
            }
        }

        private void comboTrajanje_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            double trajanje = (double)comboTrajanja.SelectedItem;
            operacija.VremeKrajaOperacije = operacija.VremePocetkaOperacije.AddHours(trajanje);
        }
    }
}
