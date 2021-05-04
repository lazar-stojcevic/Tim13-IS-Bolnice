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
                    tekst.Add("ID: " + predmet.Id + " Naziv: " + predmet.Naziv + " Tip: " + predmet.Tip);
                }
            }
            listBox.ItemsSource = tekst;
        }

        private void tip_opreme_txt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            bool svaOpremaSelektovana = false;
            TipOpreme tip = TipOpreme.dinamicka;
            switch (tip_opreme_txt.SelectedIndex)
            {
                case 0:
                    svaOpremaSelektovana = true;
                    break;
                case 1:
                    svaOpremaSelektovana = false;
                    tip = TipOpreme.dinamicka;
                    break;
                default:
                    svaOpremaSelektovana = false;
                    tip = TipOpreme.staticka;
                    break;
            }
            BazaOpreme baza = new BazaOpreme();
            List<Predmet> predmeti = new List<Predmet>();
            predmeti = baza.SvaOprema();
            List<string> tekst = new List<string>();
            foreach (Predmet predmet in predmeti)
            {

                if (predmet.Obrisano == false && (svaOpremaSelektovana == true || predmet.Tip == tip))
                {
                    tekst.Add("ID: " + predmet.Id + " Naziv: " + predmet.Naziv + " Tip: " + predmet.Tip);
                }
            }
            listBox.ItemsSource = tekst;

        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Page adding = new AddOpremuPage();
            this.NavigationService.Navigate(adding);
        }
    }
}
