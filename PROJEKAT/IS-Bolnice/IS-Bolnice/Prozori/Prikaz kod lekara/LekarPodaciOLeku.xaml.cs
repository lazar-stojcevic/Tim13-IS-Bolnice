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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarPodaciOLeku.xaml
    /// </summary>
    public partial class LekarPodaciOLeku : Page
    {
        BazaLekova bazaLekova = new BazaLekova();
        ObservableCollection<Lek> sviLekovi;
        Lek lekStari;
        Lek lek = new Lek();
        public LekarPodaciOLeku(Lek odabraniLek, ObservableCollection<Lek> lekovi)
        {
            sviLekovi = lekovi;
            lekStari = odabraniLek;
            InitializeComponent();
            txtSifra.Text = odabraniLek.Sifra;
            txtIme.Text = odabraniLek.Ime;
            txtGramaza.Text = odabraniLek.Opis;

            foreach (Sastojak sastojak in odabraniLek.Alergeni)
            {
                listSastojci.Items.Add(sastojak.Ime);
            }

        }

        private void Button_Izmeni(object sender, RoutedEventArgs e)
        {
            lek.Sifra = txtSifra.Text;
            lek.Ime = txtIme.Text;
            lek.Opis = txtGramaza.Text;
            lek.Alergeni.Clear();
            foreach (string linija in listSastojci.Items)
            {
                lek.Alergeni.Add(new Sastojak(linija));
            }

            lek.PotrebanRecept = boxRecept.IsEnabled;
            sviLekovi.Remove(lekStari);
            sviLekovi.Add(lek);

            bazaLekova.ObrisiILek(lekStari);
            bazaLekova.KreirajLek(lek);
        }

        private void Button_ClickNazad(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
