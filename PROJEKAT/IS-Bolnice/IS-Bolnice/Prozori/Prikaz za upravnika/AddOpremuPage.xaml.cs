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
    /// Interaction logic for AddOpremuPage.xaml
    /// </summary>
    public partial class AddOpremuPage : Page
    {
        public AddOpremuPage()
        {
            InitializeComponent();
        }

        private void Dodaj_btn_Click(object sender, RoutedEventArgs e)
        {
            Predmet noviPredmet = new Predmet(id_txt.Text);
            noviPredmet.Naziv = naziv_txt.Text;
            if (tip_opreme_txt.SelectedIndex == 1)
            {
                noviPredmet.Tip = TipOpreme.staticka;
            }
            else
            {
                noviPredmet.Tip = TipOpreme.dinamicka;
            }
            BazaOpreme baza = new BazaOpreme();
            baza.Sacuvaj(noviPredmet);
            Page upravljanje = new UpravljanjeOpremomPage();
            this.NavigationService.Navigate(upravljanje);
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            Page upravljanje = new UpravljanjeOpremomPage();
            this.NavigationService.Navigate(upravljanje);
        }
    }
}
