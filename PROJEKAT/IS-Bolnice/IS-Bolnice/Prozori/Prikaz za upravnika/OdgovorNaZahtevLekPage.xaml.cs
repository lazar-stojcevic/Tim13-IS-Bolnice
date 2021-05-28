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
    /// Interaction logic for OdgovorNaZahtevLekPage.xaml
    /// </summary>
    public partial class OdgovorNaZahtevLekPage : Page
    {
        Lek noviLek;
        public OdgovorNaZahtevLekPage(string idLeka)
        {
            InitializeComponent();
            BazaOdgovoraNaZahteveZaValidacijuLekova baza = new BazaOdgovoraNaZahteveZaValidacijuLekova();
            noviLek = baza.DobaviPoId(idLeka).Lek;
            opis_txt.Text = baza.DobaviPoId(idLeka).Obrazlozenje;
        }

        private void Promeni_btn_Click(object sender, RoutedEventArgs e)
        {
            Page editPage = new EditLekPage1(noviLek, false);
            this.NavigationService.Navigate(editPage);
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
