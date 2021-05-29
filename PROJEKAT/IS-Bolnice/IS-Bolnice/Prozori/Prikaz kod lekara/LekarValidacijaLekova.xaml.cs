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
using IS_Bolnice.Kontroleri;

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


            ZahtevZaValidacijuKontroler zahteviZaValidacijuKontroler = new ZahtevZaValidacijuKontroler();
            List<ZahtevZaValidacijuLeka> zahteviZaValidacijuLeka = zahteviZaValidacijuKontroler.GetSviZaValidacijuLeka();
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
            LekarZahtevValidacije zahtev = new LekarZahtevValidacije(zahtevZaValidaciju.Id, sifraLekara);

            this.NavigationService.Navigate(zahtev);

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
