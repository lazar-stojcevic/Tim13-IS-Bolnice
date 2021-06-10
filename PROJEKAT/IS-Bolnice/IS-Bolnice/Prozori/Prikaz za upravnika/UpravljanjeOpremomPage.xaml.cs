using IS_Bolnice.Kontroleri;
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

namespace IS_Bolnice.Prozori.UpravnikPages
{
    /// <summary>
    /// Interaction logic for UpravljanjeOpremomPage.xaml
    /// </summary>
    public partial class UpravljanjeOpremomPage : Page
    {
        private List<Predmet> svaOprema = new List<Predmet>();
        private OpremaKontroler kontroler = new OpremaKontroler();

        public UpravljanjeOpremomPage()
        {
            InitializeComponent();
            List<Predmet> predmeti = kontroler.GetSvaOprema();
            svaOprema.Clear();
            foreach (Predmet predmet in predmeti)
            {
                if (predmet.Obrisano == false)
                {
                    svaOprema.Add(predmet);
                }
            }
            listBox.ItemsSource = svaOprema;
        }

        private void tip_opreme_txt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool svaOpremaSelektovana = SelectovanaSvaOprema();
            TipOpreme tip = SelektovaniTipOpreme();
            List<Predmet> predmeti = kontroler.GetSvaOprema();
            svaOprema.Clear();
            foreach (Predmet predmet in predmeti)
            {

                if (predmet.Obrisano == false && (svaOpremaSelektovana == true || predmet.Tip == tip))
                {
                    svaOprema.Add(predmet);
                }
            }
            listBox.ItemsSource = svaOprema;

        }

        private bool SelectovanaSvaOprema() {
            if (tip_opreme_txt.SelectedIndex == 0) {
                return true;
            }
            return false;
        }

        private TipOpreme SelektovaniTipOpreme() {
            if (tip_opreme_txt.SelectedIndex == 2)
            {
                return TipOpreme.staticka;
            }
            return TipOpreme.dinamicka;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Predmet selektovaniPredmet = (Predmet)listBox.SelectedItem;
            Page editOpreme = new EditOpremuPage(selektovaniPredmet.Id);
            this.NavigationService.Navigate(editOpreme);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Page adding = new AddOpremuPage();
            this.NavigationService.Navigate(adding);
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Predmet> predmeti = kontroler.GetSvaOprema();
            svaOprema.Clear();
            foreach (Predmet predmet in predmeti)
            {

                if (predmet.Obrisano == false && predmet.Naziv.ToLower().Contains(search.Text.ToLower()))
                {
                    svaOprema.Add(predmet);
                }
            }
            listBox.ItemsSource = svaOprema;
        }
    }
}
