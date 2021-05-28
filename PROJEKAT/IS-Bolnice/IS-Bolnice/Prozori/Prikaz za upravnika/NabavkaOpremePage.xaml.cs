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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for NabavkaOpremePage.xaml
    /// </summary>
    public partial class NabavkaOpremePage : Page
    {
        public NabavkaOpremePage()
        {
            InitializeComponent();
            BazaOpreme baza = new BazaOpreme();
            List<Predmet> predmeti = new List<Predmet>();
            predmeti = baza.DobaviSve();
            List<string> tekst = new List<string>();
            foreach (Predmet predmet in predmeti)
            {

                if (predmet.Obrisano == false)
                {
                    tekst.Add("ID: " + predmet.Id + " Naziv: " + predmet.Naziv + " Tip: " + predmet.Tip);
                }
            }
            listBox.ItemsSource = tekst;

        }

        private void AddOpremaButton_Click(object sender, RoutedEventArgs e)
        {
            Page addOpremu = new AddOpremuPage();
            this.NavigationService.Navigate(addOpremu);
        }

        private void tip_opreme_txt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool svaOpremaSelektovana = SelectovanaSvaOprema();
            TipOpreme tip = SelektovaniTipOpreme();
            BazaOpreme baza = new BazaOpreme();
            List<Predmet> predmeti = new List<Predmet>();
            predmeti = baza.DobaviSve();
            List<string> tekst = new List<string>();
            foreach (Predmet predmet in predmeti)
            {

                if (predmet.Obrisano == false && (svaOpremaSelektovana == true || predmet.Tip == tip))
                {
                    tekst.Add("ID: " + predmet.Id + " Naziv: " + predmet.Naziv + " Tip: " + predmet.Tip);
                }
            }
            listBox.ItemsSource = tekst;
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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                BazaBolnica baza = new BazaBolnica();
                BazaSadrzaja bazaSadrzaja = new BazaSadrzaja();
                List<SadrzajSobe> sadrzajSobe = bazaSadrzaja.GetSadrzajSobe(baza.GetMagacin().Id);
                if (OpremaPostojiUMagaciju(sadrzajSobe))
                {
                    SadrzajSobe s = IzmenaSadrzaja(sadrzajSobe);
                    MessageBox.Show(s.Kolicina.ToString());
                    bazaSadrzaja.IzmeniSadrzaj(s);
                }
                else { 
                    string[] item = listBox.SelectedItem.ToString().Split(' ');
                    SadrzajSobe noviSadrzaj = new SadrzajSobe(baza.GetMagacin().Id, item[1], Int32.Parse(textBox.Text));
                    bazaSadrzaja.KreirajSadrzaj(noviSadrzaj);
                }
            }
            else {
                MessageBox.Show("Nije selektovan deo opreme za nabavku");
            }
        }
        private bool SelectovanaSvaOprema()
        {
            if (tip_opreme_txt.SelectedIndex == 0)
            {
                return true;
            }
            return false;
        }

        private TipOpreme SelektovaniTipOpreme()
        {
            if (tip_opreme_txt.SelectedIndex == 2)
            {
                return TipOpreme.staticka;
            }
            return TipOpreme.dinamicka;
        }


        private bool OpremaPostojiUMagaciju(List<SadrzajSobe> sadrzajSobe) {
            bool postoji = false;
            string[] item = listBox.SelectedItem.ToString().Split(' ');
            foreach (SadrzajSobe sadrzaj in sadrzajSobe) {
                if (sadrzaj.Predmet.Id.Equals(item[1])) {
                    postoji = true;
                }
            }

            return postoji;
        }

        private SadrzajSobe IzmenaSadrzaja(List<SadrzajSobe> sadrzajSobe) {
            SadrzajSobe noviSadrzaj = new SadrzajSobe();
            string[] item = listBox.SelectedItem.ToString().Split(' ');
            foreach (SadrzajSobe sadrzaj in sadrzajSobe) {
                if (sadrzaj.Predmet.Id.Equals(item[1])) {
                    sadrzaj.Kolicina = sadrzaj.Kolicina + Int32.Parse(textBox.Text);
                    return sadrzaj;
                }
            }
            return noviSadrzaj;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            BazaOpreme baza = new BazaOpreme();
            List<Predmet> predmeti = new List<Predmet>();
            predmeti = baza.DobaviSve();
            List<string> tekst = new List<string>();
            foreach (Predmet predmet in predmeti)
            {

                if (predmet.Obrisano == false && predmet.Naziv.ToLower().Contains(search.Text.ToLower()))
                {
                    tekst.Add("ID: " + predmet.Id + " Naziv: " + predmet.Naziv + " Tip: " + predmet.Tip);
                }
            }
            listBox.ItemsSource = tekst;
        }
    }
}
