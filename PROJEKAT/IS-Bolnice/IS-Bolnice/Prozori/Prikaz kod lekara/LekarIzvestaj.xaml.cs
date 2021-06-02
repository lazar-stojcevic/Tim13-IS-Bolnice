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
using System.Drawing;
using IS_Bolnice.Kontroleri;
using WPFCustomMessageBox;


namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarIzvestaj.xaml
    /// </summary>
    public partial class LekarIzvestaj : Page
    {
        ObservableCollection<Terapija> terapije = new ObservableCollection<Terapija>();
        string jmbgPac;
        string jmbgLek;

        StringFormat stringFormat = new StringFormat();
        Graphics graphicsImage = Graphics.FromImage(new Bitmap(@"..\..\Slike\Lekar\recept.bmp"));
        public LekarIzvestaj(string jmbgPacijenta, string jmbgLekara)
        {
            stringFormat.Alignment = StringAlignment.Near;
            InitializeComponent();
            listaLekova.ItemsSource = terapije;
            jmbgPac = jmbgPacijenta;
            jmbgLek = jmbgLekara;
        }

        private void Button_DodajLek(object sender, RoutedEventArgs e)
        {
            LekarDodavanjeLeka prozor = new LekarDodavanjeLeka(terapije, jmbgPac);
            NavigationService.Navigate(prozor);
        }

        private void Button_ObrisiLek(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da želite da obrišete ovaj lek", "Brisanje leka", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                terapije.Remove((Terapija)listaLekova.SelectedItem);
            }
            
        }

        private void Button_ZavrsiPregled(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigirsni da želida završite pregled", "Kraj pregleda", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (terapije.Count != 0)
                {
                    Pacijent p = new PacijentKontroler().GetPacijentSaOvimJMBG(jmbgPac);

                    string imeBolnice = "Zdravo bolnica";
                    string imeIPrezimePacijenta = p.Ime + " " + p.Prezime;
                    string datumRodjenjaPacijenta = p.DatumRodjenja.ToString("dd MM yyyy");
                    string danasniDatum = DateTime.Now.ToString("dd MM yyyy");
                    int brojRecepta = new Random().Next(0, 50000);

                    foreach (Terapija ter in terapije)
                    {
                        if (!ter.Lek.PotrebanRecept) { continue; }
                        KreirajReceptZaStampanje(imeBolnice, imeIPrezimePacijenta, datumRodjenjaPacijenta, danasniDatum, ter, ref brojRecepta);
                    }
                }



                Izvestaj izvestaj = new Izvestaj(new Lekar(jmbgLek), new Pacijent(jmbgPac), txtAnamneza.Text,
                    DateTime.Now);
                izvestaj.Terapija = terapije.ToList();
                SacuvajIzvestaj(izvestaj);

                LekarGlavniMeni glavniMeni = new LekarGlavniMeni(jmbgLek);
                NavigationService.Navigate(glavniMeni);
            }
            

        }

        private void SacuvajIzvestaj(Izvestaj izvestaj)
        {
            IzvestajKontroler izvestajKontroler = new IzvestajKontroler();
            izvestajKontroler.KreirajIzvestaj(izvestaj);
        }


        private void KreirajReceptZaStampanje(string imeBolnice, string imePacijenta, string datumRodjenjaPacijenta,
            string danasniDatum, Terapija ter, ref int broj)
        {
            //ISCRTAVANJAE NA SLIKU RECEPTA
            IscrtavanjeImenaBolnice(imeBolnice);

            IscrtavanjeImenaIPrezimenaPacijenta(imePacijenta);

            IscrtavanjeDatumaRodjenjaPacijenta(datumRodjenjaPacijenta);

            IcrtavanjeDanasnjegDatuma(danasniDatum);

            IscrtavanjaIdLekara();

            IcrtavanjeSifreLeka(ter);

            IscrtavanjeKolikoSePutaDnevnoLekUzima(ter);

            IspisNaReceptuNaKolikoDanaSeLekUzima(ter);

            ++broj;

            new Bitmap(@"..\..\Slike\Lekar\recept.bmp").Save(@"..\..\Recepti\noviRecept" + jmbgPac + "_(" + broj + ").bmp");
        }

        private void IscrtavanjeKolikoSePutaDnevnoLekUzima(Terapija ter)
        {
            graphicsImage.DrawString(ter.UcestanostKonzumiranja + "puta na dan", new Font("arail", 10),
                new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(80, 360), stringFormat);
        }

        private void IcrtavanjeSifreLeka(Terapija ter)
        {
            graphicsImage.DrawString(ter.Lek.Id, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.Point(30, 310), stringFormat);
        }

        private void IscrtavanjaIdLekara()
        {
            graphicsImage.DrawString(jmbgLek, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.Point(100, 280), stringFormat);
        }

        private void IcrtavanjeDanasnjegDatuma(string danasniDatum)
        {
            graphicsImage.DrawString(danasniDatum, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.Point(110, 210), stringFormat);
        }

        private void IscrtavanjeDatumaRodjenjaPacijenta(string datumRodjenjaPacijenta)
        {
            graphicsImage.DrawString(datumRodjenjaPacijenta, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.Point(30, 140), stringFormat);
        }

        private void IscrtavanjeImenaIPrezimenaPacijenta(string imePacijenta)
        {
            graphicsImage.DrawString(imePacijenta, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.Point(30, 110), stringFormat);
        }

        private void IscrtavanjeImenaBolnice(string imeBolnice)
        {
            graphicsImage.DrawString(imeBolnice, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.Point(30, 70), stringFormat);
        }

        private void IspisNaReceptuNaKolikoDanaSeLekUzima(Terapija ter)
        {
            switch (ter.RazlikaNaKolikoSeDanaUzimaLek)
            {
                case 0:
                    IscrtavanjeNaKolikoSeDanaIzimaLek("Svaki dan");
                    break;
                case 1:
                    IscrtavanjeNaKolikoSeDanaIzimaLek("Svaki drugi dan");
                    break;
                case 2:
                    IscrtavanjeNaKolikoSeDanaIzimaLek("Svaki treći dan");
                    break;
                case 3:
                    IscrtavanjeNaKolikoSeDanaIzimaLek("Svaki četvrti dan");
                    break;
                case 4:
                    IscrtavanjeNaKolikoSeDanaIzimaLek("Svaki peti dan");
                    break;
                case 5:
                    IscrtavanjeNaKolikoSeDanaIzimaLek("Svaki šesti dan");
                    break;
                default:
                    IscrtavanjeNaKolikoSeDanaIzimaLek("Na svakih nedelju dana");
                    break;
            }
        }

        private void IscrtavanjeNaKolikoSeDanaIzimaLek(string text)
        {
            graphicsImage.DrawString(text, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.Point(80, 410), stringFormat);
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 1;
        }

        private void ToggleButton_OnUnchecked_UnChecked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 0;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
