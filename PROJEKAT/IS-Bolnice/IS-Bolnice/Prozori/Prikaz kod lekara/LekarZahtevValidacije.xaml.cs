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

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarZahtevValidacije.xaml
    /// </summary>
    public partial class LekarZahtevValidacije : Page
    {
        ZahtevZaValidacijuLeka zahtev = new ZahtevZaValidacijuLeka();
        BazaZahtevaZaValidacijuLeka bazaZahteva = new BazaZahtevaZaValidacijuLeka();
        string sifra;
        public LekarZahtevValidacije(string sifraLeka, string sifraLekara)
        {
            sifra = sifraLekara;
            InitializeComponent();
            List<ZahtevZaValidacijuLeka> sviZahtevi = bazaZahteva.SviZahtevi();
            foreach (ZahtevZaValidacijuLeka zahtevIter in sviZahtevi)
            {
                if (zahtevIter.Lek.Sifra.Equals(sifraLeka))
                {
                    zahtev = zahtevIter;
                    break;
                }   
            }

            txtIme.Text = zahtev.Lek.Ime;
            txtGramaza.Text = zahtev.Lek.Opis;
            foreach (Sastojak sastojak in zahtev.Lek.Alergeni)
            {
                listSastojci.Items.Add(sastojak.Ime);
            }
            txtSifra.Text = zahtev.Lek.Sifra;
            /* ZA SAD NISMO DODALI ZAMENSKE LEKOVE
            foreach (Lek iter in zahtev.Lek.ZamenskiLekovi)
            {

                listZamesnski.Items.Add(iter.Sifra + "  " + iter.Ime);
            }*/
            boxRecept.IsChecked = zahtev.Lek.PotrebanRecept;

        }

        private void Button_ClickOdbij(object sender, RoutedEventArgs e)
        {
            LekarRazlogOdbijanjaLeka razlogOdbijanjaLeka = new LekarRazlogOdbijanjaLeka(zahtev);
            razlogOdbijanjaLeka.Show();
        }

        private void Button_Potvrdi(object sender, RoutedEventArgs e)
        {
            BazaLekova bazaLekova = new BazaLekova();
            bazaLekova.KreirajLek(zahtev.Lek);
            bazaZahteva.ObrisiZahtev(zahtev);

            LekarGlavniMeni meni = new LekarGlavniMeni(sifra);
            this.NavigationService.Navigate(meni);
        }
    }
}
