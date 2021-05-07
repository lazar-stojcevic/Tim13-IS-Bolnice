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
        ObservableCollection<Lek> zamensnkiLekovi = new ObservableCollection<Lek>();
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
            listZamesnski.ItemsSource = zamensnkiLekovi;
            boxRecept.IsChecked = odabraniLek.PotrebanRecept;

            foreach (Sastojak sastojak in odabraniLek.Alergeni)
            {
                listSastojci.Items.Add(sastojak.Ime);
            }

            foreach (Lek zamenskiLek in odabraniLek.ZamenskiLekovi)
            {
                foreach (Lek lek in sviLekovi)
                {
                    if (lek.Sifra.Equals(zamenskiLek.Sifra))
                    {
                        zamenskiLek.Ime = lek.Ime;
                    }
                }
                zamensnkiLekovi.Add(zamenskiLek);
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

            foreach (Lek zamesni in listZamesnski.Items)
            {
                lek.ZamenskiLekovi.Add(zamesni);
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

        private void Button_ClickDodaj(object sender, RoutedEventArgs e)
        {
            LekarDodavanjeSastojaka dodavanjeSastojaka = new LekarDodavanjeSastojaka(listSastojci);
            dodavanjeSastojaka.Show();
        }

        private void Button_ClickUkloni(object sender, RoutedEventArgs e)
        {
            if (listSastojci.SelectedIndex != -1)
                listSastojci.Items.RemoveAt(listSastojci.SelectedIndex);

        }

        private void Button_ClickUkloniZamenski(object sender, RoutedEventArgs e)
        {
            if (listZamesnski.SelectedIndex != -1)
            {
                zamensnkiLekovi.RemoveAt(listZamesnski.SelectedIndex);
            }
        }

        private void Button_ClickDodajZamenski(object sender, RoutedEventArgs e)
        {
            LekarDodavanjeZamenskogLeka dodavanje = new LekarDodavanjeZamenskogLeka(zamensnkiLekovi, lekStari.Sifra);
            dodavanje.Show();
        }
    }
}
