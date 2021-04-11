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
    /// Interaction logic for SekretarZakazivanjeOperacije.xaml
    /// </summary>
    public partial class SekretarZakazivanjeOperacije : Window
    {
        private BazaLekara bl;
        private BazaBolnica bb;
        private List<Lekar> lekari;
        private List<Bolnica> bolnice;
        private List<Soba> sobe;
        private Pacijent pacijent;


        public SekretarZakazivanjeOperacije(Pacijent p)
        {
            InitializeComponent();

            bl = new BazaLekara();
            bb = new BazaBolnica();

            lekari = bl.LekariSpecijalisti();
            bolnice = bb.SveBolnice();
            sobe = new List<Soba>();
            List<Soba> sveSobe = bolnice[0].Soba; // za sada se podrazumeva da postoji samo jedna bolnica

            foreach(Soba s in sveSobe)
            {
                if (!(s.PodRenoviranje || s.Obrisano) && (s.Tip == RoomType.operacionaSala))    // samo operacione sale
                {
                    sobe.Add(s);
                }
            }

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

            // inicijalno dugme za novi termin nije dostupno dok se ne odabere lekar
            odabirTermina.IsEnabled = false;

            List<string> sobeString = new List<string>();
            // formiranje stringa za svaku sobu
            foreach (Soba s in sobe)
            {
                string sobaString = s.Id + " " + s.Tip.ToString();
                sobeString.Add(sobaString);
            }
            comboSobe.ItemsSource = sobeString;
            comboSobe.SelectedIndex = -1;
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
