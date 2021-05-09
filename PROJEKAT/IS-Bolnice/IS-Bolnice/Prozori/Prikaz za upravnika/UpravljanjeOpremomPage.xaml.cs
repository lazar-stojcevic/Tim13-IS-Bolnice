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
        public UpravljanjeOpremomPage()
        {
            InitializeComponent();
            BazaOpreme baza = new BazaOpreme();
            List<Predmet> predmeti = new List<Predmet>();
            predmeti = baza.SvaOprema();
            List<string> tekst = new List<string>();
            foreach (Predmet predmet in predmeti)
            {

                if (predmet.Obrisano == false)
                {
                    tekst.Add(ParseToString(predmet));
                }
            }
            listBox.ItemsSource = tekst;
        }

        private void tip_opreme_txt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            bool svaOpremaSelektovana = SelectovanaSvaOprema();
            TipOpreme tip = SelektovaniTipOpreme();
            BazaOpreme baza = new BazaOpreme();
            List<Predmet> predmeti = baza.SvaOprema();
            List<string> tekst = new List<string>();
            foreach (Predmet predmet in predmeti)
            {

                if (predmet.Obrisano == false && (svaOpremaSelektovana == true || predmet.Tip == tip))
                {
                    tekst.Add(ParseToString(predmet));
                }
            }
            listBox.ItemsSource = tekst;

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

        private string ParseToString(Predmet predmet) {
            string text = "ID: " + predmet.Id + " Naziv: " + predmet.Naziv + " Tip: " + predmet.Tip;
            return text;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] podaciOOpremi = listBox.SelectedItem.ToString().Split(' ');
            Page editOpreme = new EditOpremuPage(podaciOOpremi[1]);
            this.NavigationService.Navigate(editOpreme);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Page adding = new AddOpremuPage();
            this.NavigationService.Navigate(adding);
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            BazaOpreme baza = new BazaOpreme();
            List<Predmet> predmeti = new List<Predmet>();
            predmeti = baza.SvaOprema();
            List<string> tekst = new List<string>();
            foreach (Predmet predmet in predmeti)
            {

                if (predmet.Obrisano == false && predmet.Naziv.ToLower().Contains(search.Text.ToLower()))
                {
                    tekst.Add("ID: " + predmet.Id + " Naziv: " + predmet.Naziv + " Tip: " + predmet.Tip);
                }
            }
            listBox.ItemsSource = tekst;
        }
    }
}
