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

namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for SekretarZakazivanjePregleda.xaml
    /// </summary>
    public partial class SekretarZakazivanjePregleda : Window
    {
        private BazaPregleda bpreg;
        private BazaLekara bl;
        private List<Lekar> lekari;
        private Pacijent pacijent;

        public ObservableCollection<Pregled> PreglediLekara
        {
            get;
            set;
        }

        public SekretarZakazivanjePregleda(Pacijent p)
        {
            InitializeComponent();

            this.DataContext = this;
            bl = new BazaLekara();
            bpreg = new BazaPregleda();
            lekari = bl.LekariOpstePrakse();
            PreglediLekara = new ObservableCollection<Pregled>();
            pacijent = p;

            txtIme.Text = pacijent.Ime;
            txtPrezime.Text = pacijent.Prezime;
            txtJmbg.Text = pacijent.Jmbg;

            List<string> lekariString = new List<string>();

            // formiranje stringa za svakog lekara
            foreach (Lekar l in lekari)
            {
                string lekarString = l.Ime + " " + l.Prezime + " (" + l.Tip + ")";
                lekariString.Add(lekarString);
            }

            comboLekari.ItemsSource = lekariString;
            comboLekari.SelectedIndex = -1;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Lekar izabraniLekar = pacijent.IzabraniLekar;
            if (izabraniLekar != null)
            {
                int indeks = 0;
                foreach (Lekar l in lekari)
                {
                    if (l.Jmbg.Equals(izabraniLekar.Jmbg))
                    {
                        break;
                    }
                    indeks++;
                }
                comboLekari.SelectedIndex = indeks;
            }
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            int index = prikazTermina.SelectedIndex;
            if (index != -1)
            {
                Pregled pregled = PreglediLekara[index];
                pregled.Pacijent = pacijent;
                bpreg.ZakaziPregled(pregled);
                this.Close();
            }
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void comboLekari_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Lekar lekar = lekari[comboLekari.SelectedIndex];
            List<Pregled> pregledi = bpreg.PreglediOdredjenogLekara(lekar.Jmbg);
            PreglediLekara.Clear();

            int i = 0;
            DateTime trenutnoVreme = DateTime.Now;
            while (i < 5)
            {
                Pregled pregled = new Pregled();
                pregled.VremePocetkaPregleda = trenutnoVreme.AddMinutes(5);
                pregled.VremeKrajaPregleda = pregled.VremePocetkaPregleda.AddMinutes(45);   // pretpostavka da termin traje 45min
                pregled.Lekar = lekar;
                trenutnoVreme = pregled.VremeKrajaPregleda; // potencijalno sledeci slobodan termin je 5min nakon zavrsetka ovog termina
                //TODO: provera da li se termin vec preklapa sa nekim terminom i koje je radno vreme lekara
                i++;
                PreglediLekara.Add(pregled);
            }
        }

        private void RadioButton_Termin_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Lekar_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
