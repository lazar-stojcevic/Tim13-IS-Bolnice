using IS_Bolnice.Kontroleri;
using System;
using System.Collections.Generic;
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
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.UpravnikPages
{
    /// <summary>
    /// Interaction logic for AddOpremuPage.xaml
    /// </summary>
    public partial class AddOpremuPage : Page
    {

        OpremaKontroler kontroler = new OpremaKontroler();

        public AddOpremuPage()
        {
            InitializeComponent();
        }

        private void Dodaj_btn_Click(object sender, RoutedEventArgs e)
        {
            if (Validiraj())
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
                kontroler.KreirajNoviPredmet(noviPredmet);
                Page upravljanje = new UpravljanjeOpremomPage();
                this.NavigationService.Navigate(upravljanje);
            }
            else
            {
                CustomMessageBox.ShowOK("Podaci nisu validno uneti! Ne sme biti #!", "Greška", "Potvrdi");
            }
        }

        private bool Validiraj()
        {
            if (id_txt.Text.Contains("#"))
            {
                return false;
            }
            if (naziv_txt.Text.Contains("#"))
            {
                return false;
            }
            return true;
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            Page upravljanje = new UpravljanjeOpremomPage();
            this.NavigationService.Navigate(upravljanje);
        }

    }
}
