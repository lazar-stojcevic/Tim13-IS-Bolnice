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
        IzvestajKontroler izvestajKontroler = new IzvestajKontroler();
        ObservableCollection<Terapija> terapije = new ObservableCollection<Terapija>();
        string jmbgPac;
        string jmbgLek;
        public LekarIzvestaj(string jmbgPacijenta, string jmbgLekara)
        {
            InitializeComponent();
            listaLekova.ItemsSource = terapije;
            jmbgPac = jmbgPacijenta;
            jmbgLek = jmbgLekara;
        }

        private void Button_DodajLek(object sender, RoutedEventArgs e)
        {
            LekarDodavanjeLeka prozor = new LekarDodavanjeLeka(terapije, jmbgPac);
            prozor.Show();
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
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Near;

                    Pacijent p = new PacijentKontroler().GetPacijentSaOvimJMBG(jmbgPac);

                    string imeBolnice = "Zdravo bolnica";
                    string imeIPrezimePacijenta = p.Ime + " " + p.Prezime;
                    string datumRodjenjaPacijenta = p.DatumRodjenja.ToString("dd MM yyyy");
                    string danasniDatum = DateTime.Now.ToString("dd MM yyyy");
                    int brojRecepta = new Random().Next(0, 50000);

                    foreach (Terapija ter in terapije)
                    {
                        if (!ter.Lek.PotrebanRecept) { continue; }
                        KreirajReceptZaStampanje(imeBolnice, stringFormat, imeIPrezimePacijenta, datumRodjenjaPacijenta, danasniDatum, ter, ref brojRecepta);
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


        private void KreirajReceptZaStampanje(string imeBolnice, StringFormat stringFormat, string imePacijenta, string datumRodjenjaPacijenta,
            string danasniDatum, Terapija ter, ref int broj)
        {
            System.Drawing.Image bitmap = new Bitmap(@"..\..\Slike\recept.bmp");
            Graphics graphicsImage = Graphics.FromImage(bitmap);
            //ISCRTAVANJAE NA SLIKU RECEPTA
            IscrtavanjeImenaBolnice(imeBolnice, stringFormat, graphicsImage);

            IscrtavanjeImenaIPrezimenaPacijenta(stringFormat, imePacijenta, graphicsImage);

            IscrtavanjeDatumaRodjenjaPacijenta(stringFormat, datumRodjenjaPacijenta, graphicsImage);

            IcrtavanjeDanasnjegDatuma(stringFormat, danasniDatum, graphicsImage);

            IscrtavanjaIDLekara(stringFormat, graphicsImage);

            IcrtavanjeSifreLeka(stringFormat, ter, graphicsImage);

            IscrtavanjeKolikoSePutaDnevnoLekUzima(stringFormat, ter, graphicsImage);

            IspisNaReceptuNaKolikoDanaSeLekUzima(stringFormat, ter, graphicsImage);

            ++broj;
            bitmap.Save(@"..\..\Recepti\noviRecept" + jmbgPac + "_(" + broj + ").bmp");
        }

        private static void IscrtavanjeKolikoSePutaDnevnoLekUzima(StringFormat stringFormat, Terapija ter,
            Graphics graphicsImage)
        {
            graphicsImage.DrawString(ter.UcestanostKonzumiranja + "puta na dan", new Font("arail", 10),
                new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(80, 360), stringFormat);
        }

        private static void IcrtavanjeSifreLeka(StringFormat stringFormat, Terapija ter, Graphics graphicsImage)
        {
            graphicsImage.DrawString(ter.Lek.Id, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.Point(30, 310), stringFormat);
        }

        private void IscrtavanjaIDLekara(StringFormat stringFormat, Graphics graphicsImage)
        {
            graphicsImage.DrawString(jmbgLek, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.Point(100, 280), stringFormat);
        }

        private static void IcrtavanjeDanasnjegDatuma(StringFormat stringFormat, string danasniDatum, Graphics graphicsImage)
        {
            graphicsImage.DrawString(danasniDatum, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.Point(110, 210), stringFormat);
        }

        private static void IscrtavanjeDatumaRodjenjaPacijenta(StringFormat stringFormat, string datumRodjenjaPacijenta,
            Graphics graphicsImage)
        {
            graphicsImage.DrawString(datumRodjenjaPacijenta, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.Point(30, 140), stringFormat);
        }

        private static void IscrtavanjeImenaIPrezimenaPacijenta(StringFormat stringFormat, string imePacijenta,
            Graphics graphicsImage)
        {
            graphicsImage.DrawString(imePacijenta, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.Point(30, 110), stringFormat);
        }

        private static void IscrtavanjeImenaBolnice(string imeBolnice, StringFormat stringFormat, Graphics graphicsImage)
        {
            graphicsImage.DrawString(imeBolnice, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.Point(30, 70), stringFormat);
        }

        private static void IspisNaReceptuNaKolikoDanaSeLekUzima(StringFormat stringFormat, Terapija ter,
            Graphics graphicsImage)
        {
            switch (ter.RazlikaNaKolikoSeDanaUzimaLek)
            {
                case 0:
                    IscrtavanjeNaKolikoSeDanaIzimaLek("Svaki dan",stringFormat, graphicsImage);
                    break;
                case 1:
                    IscrtavanjeNaKolikoSeDanaIzimaLek("Svaki drugi dan", stringFormat, graphicsImage);
                    break;
                case 2:
                    IscrtavanjeNaKolikoSeDanaIzimaLek("Svaki treći dan", stringFormat, graphicsImage);
                    break;
                case 3:
                    IscrtavanjeNaKolikoSeDanaIzimaLek("Svaki četvrti dan", stringFormat, graphicsImage);
                    break;
                case 4:
                    IscrtavanjeNaKolikoSeDanaIzimaLek("Svaki peti dan", stringFormat, graphicsImage);
                    break;
                case 5:
                    IscrtavanjeNaKolikoSeDanaIzimaLek("Svaki šesti dan", stringFormat, graphicsImage);
                    break;
                default:
                    IscrtavanjeNaKolikoSeDanaIzimaLek("Na svakih nedelju dana", stringFormat, graphicsImage);
                    break;
            }
        }

        private static void IscrtavanjeNaKolikoSeDanaIzimaLek(string text, StringFormat stringFormat, Graphics graphicsImage)
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
