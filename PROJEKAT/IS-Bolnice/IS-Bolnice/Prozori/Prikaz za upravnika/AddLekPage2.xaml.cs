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

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for AddLekPage2.xaml
    /// </summary>
    public partial class AddLekPage2 : Page
    {
        private ISastojakRepozitorijum sastojakRepo = new SastojakFajlRepozitorijum();
        public Lek noviLek;
        public AddLekPage2(Lek lek)
        {
            InitializeComponent();
            listBox_sastojci.ItemsSource = ParseSastojakToString();
            listBox.ItemsSource = ParseLekToString();
            noviLek = lek;
        }
    



        public List<string> ParseSastojakToString() {
            List<Sastojak> sastojci = sastojakRepo.DobaviSve();
            List<string> tekst = new List<string>();
            foreach (Sastojak sastojak in sastojci)
            {
                tekst.Add(sastojak.Ime);
            }
            return tekst;
        }

        public List<string> ParseLekToString()
        {
            LekFajlRepozitorijum lekFajlRepozitorijum = new LekFajlRepozitorijum();
            List<string> tekst = new List<string>();
            List<Lek> lekovi = lekFajlRepozitorijum.DobaviSve();
            foreach (Lek lek in lekovi)
            {
                string linija = "ID: " + lek.Id + " Naziv: " + lek.Ime;
                tekst.Add(linija);
            }
            return tekst;
        }
       
        private void Potvrdi_btn_Click(object sender, RoutedEventArgs e)
        {
            
            foreach (string linija in listBox_sastojci.SelectedItems) {
                Sastojak noviSastojak = new Sastojak(linija);
                noviLek.Alergeni.Add(noviSastojak);
            }
            
            foreach (string linija in listBox.SelectedItems) {
                string[] deo = linija.Split(' ');
                Lek lek = new Lek(deo[1]);
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
    }
}
