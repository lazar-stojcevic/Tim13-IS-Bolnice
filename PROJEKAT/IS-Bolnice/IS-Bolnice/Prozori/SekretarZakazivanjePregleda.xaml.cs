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
        private BazaLekara bl;
        private List<Lekar> lekari;
        private Pacijent pacijent;

        //public ObservableCollection<Pregled> PreglediLekara
        //{
         //   get;
        //    set;
        //}

        public SekretarZakazivanjePregleda(Pacijent p)
        {
            InitializeComponent();

            bl = new BazaLekara();
            lekari = bl.LekariOpstePrakse();
            //PreglediLekara = new ObservableCollection<Pregled>();
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

        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void comboLekari_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //BazaPregleda bpreg = new BazaPregleda();
            //Lekar lekar = lekari[comboLekari.SelectedIndex];
           // List<Pregled> pregledi = bpreg.PreglediOdredjenogLekara(lekar.Jmbg);
           // PreglediLekara.Clear();
           // foreach (Pregled p in pregledi)
           // {
            //    PreglediLekara.Add(p);
            //}
        }
    }
}
