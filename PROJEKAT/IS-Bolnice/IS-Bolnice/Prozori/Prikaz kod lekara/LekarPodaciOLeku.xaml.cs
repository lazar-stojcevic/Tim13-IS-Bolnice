using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using IS_Bolnice.Kontroleri.Lekovi;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarPodaciOLeku.xaml
    /// </summary>
    public partial class LekarPodaciOLeku : Page
    {
        LekKontroler lekKontroler = new LekKontroler();
        ObservableCollection<Lek> sviLekovi;
        ObservableCollection<Lek> zamensnkiLekovi = new ObservableCollection<Lek>();
        Lek lekStari = new Lek();
        Lek lek = new Lek();
        public LekarPodaciOLeku(Lek odabraniLek, ObservableCollection<Lek> lekovi)
        {
            sviLekovi = lekovi;
            lekStari = odabraniLek;
            InitializeComponent();
            txtSifra.Text = odabraniLek.Id;
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
                    if (lek.Id.Equals(zamenskiLek.Id))
                    {
                        zamenskiLek.Ime = lek.Ime;
                    }
                }
                zamensnkiLekovi.Add(zamenskiLek);
            }

        }

        private void Button_Izmeni(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da želite da promenite podatke o leku?", "Izmena leka", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {

                lek.Id = txtSifra.Text;
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

                lekKontroler.ObrisiLek(lekStari);
                lekKontroler.KreirajLek(lek);

                NavigationService.GoBack();
            }
        }

        private void Button_ClickNazad(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Button_ClickDodaj(object sender, RoutedEventArgs e)
        {
            LekarDodavanjeSastojaka dodavanjeSastojaka = new LekarDodavanjeSastojaka(listSastojci);
            dodavanjeSastojaka.ShowDialog();
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
            LekarDodavanjeZamenskogLeka dodavanje = new LekarDodavanjeZamenskogLeka(zamensnkiLekovi, lekStari.Id);
            dodavanje.ShowDialog();
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 1;
        }

        private void ToggleButton_OnUnchecked_UnChecked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 0;
        }

        private void ValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[#]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
