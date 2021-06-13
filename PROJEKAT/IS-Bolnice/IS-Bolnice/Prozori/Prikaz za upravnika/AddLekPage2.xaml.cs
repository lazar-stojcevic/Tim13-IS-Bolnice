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
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Kontroleri.Lekovi;

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for AddLekPage2.xaml
    /// </summary>
    public partial class AddLekPage2 : Page
    {
        private ISastojakRepozitorijum sastojakRepo = new SastojakFajlRepozitorijum();
        private LekKontroler kontroler = new LekKontroler();
        public Lek noviLek;

        public List<Lek> sviLekovi;
        public AddLekPage2(Lek lek)
        {
            InitializeComponent();
            sviLekovi = kontroler.GetSviLekovi();
            listBox_sastojci.ItemsSource = ParseSastojakToString();
            listBox.ItemsSource = sviLekovi;
            noviLek = lek;
        }
    

        public List<string> ParseSastojakToString() {
            List<Sastojak> sastojci = sastojakRepo.GetSve();
            List<string> tekst = new List<string>();
            foreach (Sastojak sastojak in sastojci)
            {
                tekst.Add(sastojak.Ime);
            }
            return tekst;
        }
       
        private void Potvrdi_btn_Click(object sender, RoutedEventArgs e)
        {
            
            foreach (string linija in listBox_sastojci.SelectedItems) {
                Sastojak noviSastojak = new Sastojak(linija);
                noviLek.Alergeni.Add(noviSastojak);
            }
            
            foreach (Lek lek in listBox.SelectedItems) {
                noviLek.ZamenskiLekovi.Add(lek);
            }
            Page addLekSledeci = new AddLekPage3(noviLek);
            this.NavigationService.Navigate(addLekSledeci);

        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            Page lekovi = new LekoviPage();
            this.NavigationService.Navigate(lekovi);
        }

        private void Sastojci_btn_Click(object sender, RoutedEventArgs e)
        {
            Page sastojci = new SastojciPage();
            this.NavigationService.Navigate(sastojci);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

    }
}
