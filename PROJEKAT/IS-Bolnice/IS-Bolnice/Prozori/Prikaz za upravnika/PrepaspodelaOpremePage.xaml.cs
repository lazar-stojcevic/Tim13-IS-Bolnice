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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for PrepaspodelaOpremePage.xaml
    /// </summary>
    public partial class PrepaspodelaOpremePage : Page
    {
        public PrepaspodelaOpremePage()
        {
            InitializeComponent();
            BazaBolnica baza = new BazaBolnica();
            List<Soba> prostorije = baza.GetSobe();
            List<string> izvor = new List<string>();
            izvor.Add("");
            foreach (Soba s in prostorije) {
                izvor.Add(s.Id);
            }
            sala1_txt.ItemsSource = izvor;
            sala2_txt.ItemsSource = izvor;
        }

        private void soba1_txt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!sala1_txt.SelectedItem.Equals(""))
            {
                BazaSadrzaja baza = new BazaSadrzaja();
                List<SadrzajSobe> sadrzajSobe = baza.GetSadrzajSobe(sala1_txt.SelectedItem.ToString());
                listBox1.ItemsSource = ParseToString(sadrzajSobe);
            }
        }

        private List<string> ParseToString(List<SadrzajSobe> sadrzajSobe) {
            List<string> tekst = new List<string>();
            BazaOpreme baza = new BazaOpreme();
            foreach (SadrzajSobe sadrzaj in sadrzajSobe) {
                string linija = "ID: " + sadrzaj.Predmet.Id + " Naziv: " + baza.GetPredmet(sadrzaj.Predmet.Id).Naziv + " Kolicina " + sadrzaj.Kolicina;
                tekst.Add(linija);
            }
            return tekst;
        }

        private void sala2_txt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!sala2_txt.SelectedItem.Equals(""))
            {
                BazaSadrzaja baza = new BazaSadrzaja();
                List<SadrzajSobe> sadrzajSobe = baza.GetSadrzajSobe(sala2_txt.SelectedItem.ToString());
                listBox2.ItemsSource = ParseToString(sadrzajSobe);
            }
        }

        private void Oduzmi_btn_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.Parse(textBox.Text) != 0)
            {
                int brojac = Int32.Parse(textBox.Text) - 1;
                textBox.Text = brojac.ToString();
            }
        }

        private void Dodaj_btn_Click(object sender, RoutedEventArgs e)
        {
                int brojac = Int32.Parse(textBox.Text) + 1;
                textBox.Text = brojac.ToString();
            
        }

        private void UpMoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                OduzmiOdabranuKolicinuOpreme(2);
                DodajOdabranuKolicinuOpreme(1);
            }
            else {
                MessageBox.Show("Nije selektovana oprema za premestanje!");
            }
        }

        private void OduzmiOdabranuKolicinuOpreme(int smer) {
            BazaSadrzaja bazaSadrzaja = new BazaSadrzaja();
            List<SadrzajSobe> sadrzajSobe = bazaSadrzaja.GetSadrzajSobe(GetIDProstorije(smer));
            foreach (SadrzajSobe predmet in sadrzajSobe)
            {
                if (predmet.Predmet.Id.Equals(GetIDOpreme(smer)))
                {
                    if (predmet.Kolicina > Int32.Parse(textBox.Text))
                    {
                        predmet.Kolicina = predmet.Kolicina - Int32.Parse(textBox.Text);
                        bazaSadrzaja.IzmeniSadrzaj(predmet);
                    }
                    else
                    {
                        MessageBox.Show("Odabrana kolicina nije validna!");
                    }
                }
            }
        }

        private string GetIDProstorije(int smer) {
            string id;
            if (smer == 1)
            {
                id = sala1_txt.SelectedItem.ToString();
            }
            else {
                id = sala2_txt.SelectedItem.ToString();
            }
            return id;
        }

        private string GetIDOpreme(int smer)
        {
            string[] linija;
            if (smer == 1)
            {
                linija = listBox1.SelectedItem.ToString().Split(' ');
            }
            else
            {
                linija = listBox2.SelectedItem.ToString().Split(' ');
            }
            return linija[1];
        }

        private void DodajOdabranuKolicinuOpreme(int smer) {
            BazaSadrzaja bazaSadrzaja = new BazaSadrzaja();
            int smer1;
            if (smer == 1)
            {
                smer1 = 2;
            }
            else {
                smer1 = 1;
            }
            List<SadrzajSobe> sadrzajSobe = bazaSadrzaja.GetSadrzajSobe(GetIDProstorije(smer));
            foreach (SadrzajSobe predmet in sadrzajSobe)
            {
                if (predmet.Predmet.Id.Equals(GetIDOpreme(smer1)))
                {
                        predmet.Kolicina = predmet.Kolicina + Int32.Parse(textBox.Text);
                        bazaSadrzaja.IzmeniSadrzaj(predmet);
                    return;
                }
            }
            SadrzajSobe noviSadrzaj = new SadrzajSobe(GetIDProstorije(smer), GetIDOpreme(smer1), Int32.Parse(textBox.Text));
            bazaSadrzaja.KreirajSadrzaj(noviSadrzaj);
        }



        private void DownMoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                OduzmiOdabranuKolicinuOpreme(1);
                DodajOdabranuKolicinuOpreme(2);
                BazaSadrzaja bazaSadrzaja = new BazaSadrzaja();
                List<SadrzajSobe> sadrzajSobe1 = bazaSadrzaja.GetSadrzajSobe(GetIDProstorije(1));
                listBox1.ItemsSource = ParseToString(sadrzajSobe1);
                List<SadrzajSobe> sadrzajSobe2 = bazaSadrzaja.GetSadrzajSobe(GetIDProstorije(2));
                listBox2.ItemsSource = ParseToString(sadrzajSobe2);
            }
            else
            {
                MessageBox.Show("Nije selektovana oprema za premestanje!");
            }
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
