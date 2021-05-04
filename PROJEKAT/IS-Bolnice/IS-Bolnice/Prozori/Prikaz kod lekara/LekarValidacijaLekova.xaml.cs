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
    /// Interaction logic for LekarValidacijaLekova.xaml
    /// </summary>
    public partial class LekarValidacijaLekova : Page
    {
        string sifraLekara;
        public LekarValidacijaLekova(string jmbgLekara)
        {
            InitializeComponent();
            sifraLekara = jmbgLekara;
            List<ZahtevZaValidacijuLeka> itemSource = new List<ZahtevZaValidacijuLeka>();


            BazaZahtevaZaValidacijuLeka bz = new BazaZahtevaZaValidacijuLeka();
            List<ZahtevZaValidacijuLeka> zahteviZaValidacijuLeka = bz.SviZahtevi();
            foreach (ZahtevZaValidacijuLeka zahtev in zahteviZaValidacijuLeka)
            {
                foreach (Lekar lekar in zahtev.lekariKomeIdeNaValidaciju)
                {
                    if (lekar.Jmbg.Equals(jmbgLekara))
                    {
                        itemSource.Add(zahtev);
                    }
                }
            }

            listaZahteva.ItemsSource = itemSource;

        }

        private void otvoriLekClick(object sender, RoutedEventArgs e)
        {
            ZahtevZaValidacijuLeka zahtevZaValidaciju = (ZahtevZaValidacijuLeka)listaZahteva.SelectedItem;
            LekarZahtevValidacije zahtev = new LekarZahtevValidacije(zahtevZaValidaciju.Lek.Sifra, sifraLekara);

            this.NavigationService.Navigate(zahtev);

        }
    }
}
