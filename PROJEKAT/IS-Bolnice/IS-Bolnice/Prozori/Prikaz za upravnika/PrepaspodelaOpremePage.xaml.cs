using IS_Bolnice.Kontroleri;
using IS_Bolnice.Prozori.Prikaz_za_upravnika;
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
using IS_Bolnice.Kontroleri.Ustanova;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for PrepaspodelaOpremePage.xaml
    /// </summary>
    public partial class PrepaspodelaOpremePage : Page
    {
        private List<SadrzajSobe> sadrzajPrveSobe = new List<SadrzajSobe>();
        private List<SadrzajSobe> sadrzajDrugeSobe = new List<SadrzajSobe>();
        private SadrzajSobeKontroler kontroler = new SadrzajSobeKontroler();
        private OpremaKontroler opremaKontroler = new OpremaKontroler();
        private BolnicaKontroler bolnicaKontroler = new BolnicaKontroler();
        public PrepaspodelaOpremePage()
        {
            InitializeComponent();
            List<string> sobe = new List<string>();
            foreach (Soba iterSoba in bolnicaKontroler.GetSveSobe())
            {
                sobe.Add(iterSoba.Id);
            }
            sala1_txt.ItemsSource = sobe;
            sala2_txt.ItemsSource = sobe;
        }

        private void soba1_txt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!sala1_txt.SelectedItem.Equals(""))
            {
                sadrzajPrveSobe = kontroler.GetSadrzajSobe(sala1_txt.SelectedItem.ToString());
                listBox1.ItemsSource = sadrzajPrveSobe;
            }
        }

        private void sala2_txt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!sala2_txt.SelectedItem.Equals(""))
            {
                sadrzajDrugeSobe = kontroler.GetSadrzajSobe(sala2_txt.SelectedItem.ToString());
                listBox2.ItemsSource = sadrzajDrugeSobe;
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
                SadrzajSobe noviSadrzaj = (SadrzajSobe)listBox2.SelectedItem;
                if (noviSadrzaj.Kolicina > Int32.Parse(textBox.Text))
                {
                    noviSadrzaj.Kolicina = Int32.Parse(textBox.Text);
                    Predmet odabraneOprema = opremaKontroler.GetPoId(noviSadrzaj.Predmet.Id);
                    bool uspeh = true;
                    if (odabraneOprema.Tip == TipOpreme.dinamicka)
                    {
                        if (!kontroler.PrebaciOpremu(noviSadrzaj,
                            bolnicaKontroler.GetSobaPoId(sala1_txt.SelectedItem.ToString())))
                        {
                            MessageBox.Show("Odabrana kolicina nije validna!");
                            uspeh = false;
                        }
                    }
                    else
                    {
                        PrebaciOpremuUStanjeCekanja(false);
                    }
                    sadrzajPrveSobe = kontroler.GetSadrzajSobe(GetIDProstorije(true));
                    listBox1.ItemsSource = sadrzajPrveSobe;
                    sadrzajDrugeSobe = kontroler.GetSadrzajSobe(GetIDProstorije(false));
                    listBox2.ItemsSource = sadrzajDrugeSobe;
                    if(uspeh) CustomMessageBox.ShowOK("Preraspodela je uspešno izvršena", "Upseh", "Potvrdi");
                }
                else {
                    CustomMessageBox.ShowOK("Količina opreme nije odgovarajuća!", "Greška", "Potvrdi");
                }
            }
            else {
                CustomMessageBox.ShowOK("Nije selektovana oprema za premeštanje!", "Greška", "Potvrdi");
            }
        }

        private void PrebaciOpremuUStanjeCekanja(bool smer) {
            SadrzajSobe noviSadrzaj = new SadrzajSobe(GetIDProstorije(smer), GetIDOpreme(smer), Int32.Parse(textBox.Text));
            Soba soba = new Soba(GetIDProstorije(!smer));
            noviSadrzaj.NovaSoba = soba;
            PreraspodelaStatickeOpremeWindow preraspodelaStatickeOpreme = new PreraspodelaStatickeOpremeWindow(noviSadrzaj);
            preraspodelaStatickeOpreme.Show();

        }

        private string GetIDProstorije(bool smer) {
            string id;
            if (smer == true)
            {
                id = sala1_txt.SelectedItem.ToString();
            }
            else {
                id = sala2_txt.SelectedItem.ToString();
            }
            return id;
        }

        private string GetIDOpreme(bool smer)
        {
            SadrzajSobe sadrzaj;
            if (smer == true)
            {
                sadrzaj = (SadrzajSobe)listBox1.SelectedItem;
            }
            else
            {
                sadrzaj = (SadrzajSobe)listBox2.SelectedItem;
            }
            return sadrzaj.Predmet.Id;
        }




        private void DownMoveButton_Click(object sender, RoutedEventArgs e)
        {
            OpremaFajlRepozitorijum opremaFajlRepozitorijum = new OpremaFajlRepozitorijum();
            if (listBox1.SelectedIndex != -1)
            {
                SadrzajSobe noviSadrzaj = (SadrzajSobe)listBox1.SelectedItem;
                if (noviSadrzaj.Kolicina > Int32.Parse(textBox.Text))
                {
                    noviSadrzaj.Kolicina = Int32.Parse(textBox.Text);
                    Predmet odabraneOprema = opremaKontroler.GetPoId(noviSadrzaj.Predmet.Id);
                if (odabraneOprema.Tip == TipOpreme.dinamicka)
                {
                    kontroler.PrebaciOpremu(noviSadrzaj, bolnicaKontroler.GetSobaPoId(sala2_txt.SelectedItem.ToString()));
                }
                else
                {
                    PrebaciOpremuUStanjeCekanja(true);
                }
                sadrzajPrveSobe = kontroler.GetSadrzajSobe(GetIDProstorije(true));
                listBox1.ItemsSource = sadrzajPrveSobe;
                sadrzajDrugeSobe = kontroler.GetSadrzajSobe(GetIDProstorije(false));
                listBox2.ItemsSource = sadrzajDrugeSobe;
                    CustomMessageBox.ShowOK("Preraspodela je uspešno izvršena", "Upseh", "Potvrdi");
                }
                else
                {
                    CustomMessageBox.ShowOK("Količina opreme nije odgovarajuća!", "Greška", "Potvrdi");
                }
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
