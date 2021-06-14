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
using IS_Bolnice.Repozitorijumi.Interfejsi;

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for SastojciPage.xaml
    /// </summary>
    public partial class SastojciPage : Page
    {
        private ISastojakRepozitorijum sastojakRepo = new SastojakFajlRepozitorijum();
        public SastojciPage()
        {
            InitializeComponent();
            listBox.ItemsSource = ParseToString();

        }

        public List<string> ParseToString() {
            List<Sastojak> sastojci = sastojakRepo.GetSve();
            List<string> tekst = new List<string>();
            foreach (Sastojak sastojak in sastojci) {
                tekst.Add(sastojak.Ime);
            }
            return tekst;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddSastojakWindow addSastojakWindow = new AddSastojakWindow();
            addSastojakWindow.Show();
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditSastojakWindow editSastojakWindow = new EditSastojakWindow(listBox.SelectedItem.ToString());
            editSastojakWindow.Show();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Page addLek = new AddLekPage();
            this.NavigationService.Navigate(addLek);
        }
    }
}
