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
    /// Interaction logic for EditLekPage1.xaml
    /// </summary>
    public partial class EditLekPage1 : Page
    {
        Lek noviLek;
        bool kreiranjeIzmena;
        public EditLekPage1(Lek lek, bool izmenaKreiranje)
        {
            InitializeComponent();
            noviLek = lek;
            kreiranjeIzmena = izmenaKreiranje;
            id_txt.Text = noviLek.Sifra;
            naziv_txt.Text = noviLek.Ime;
            opis_txt.Text = noviLek.Opis;
            if (noviLek.PotrebanRecept == true)
            {
                potreban_recept_txt.SelectedIndex = 0;
            }
            else {
                potreban_recept_txt.SelectedIndex = 1;
            }
        }

        private void Dalje_btn_Click(object sender, RoutedEventArgs e)
        {
            Page editLek2 = new EditLekPage2(noviLek, kreiranjeIzmena);
            this.NavigationService.Navigate(editLek2);
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            if (kreiranjeIzmena) {
                Page lekovi = new LekoviPage();
                this.NavigationService.Navigate(lekovi);
            }
            else
            {
                Page odgovoriNaZahteve = new OdgovoriNaZahteveLekoviPage();
                this.NavigationService.Navigate(odgovoriNaZahteve);
            }
        }

        private void id_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            noviLek.Sifra = id_txt.Text;
        }

        private void naziv_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            noviLek.Ime = naziv_txt.Text;
        }

        private void opis_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            noviLek.Opis = opis_txt.Text;
        }

        private void potreban_recept_txt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (noviLek != null)
            {
                if (potreban_recept_txt.SelectedIndex == 0)
                {
                    noviLek.PotrebanRecept = true;
                }
                else
                {
                    noviLek.PotrebanRecept = false;
                }
            }
        }
    }
}
