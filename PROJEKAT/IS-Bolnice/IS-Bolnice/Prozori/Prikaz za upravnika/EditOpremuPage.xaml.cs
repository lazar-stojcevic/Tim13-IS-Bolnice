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
    /// Interaction logic for EditOpremuPage.xaml
    /// </summary>
    public partial class EditOpremuPage : Page
    {
        public EditOpremuPage(string selectedID)
        {
            InitializeComponent();
            id_txt.Text = selectedID;
            BazaOpreme baza = new BazaOpreme();
            Predmet izmenjenPredmet = baza.DobaviPoId(selectedID);
            naziv_txt.Text = izmenjenPredmet.Naziv;
            if (izmenjenPredmet.Tip == TipOpreme.dinamicka)
            {
                tip_opreme_txt.SelectedIndex = 0;
            }
            else {
                tip_opreme_txt.SelectedIndex = 1;
            }

        }

        private void Izmeni_btn_Click(object sender, RoutedEventArgs e)
        {
            BazaOpreme baza = new BazaOpreme();
            List<Predmet> lista = baza.DobaviSve();
            Predmet izmenjenPredmet = baza.DobaviPoId(id_txt.Text);
            izmenjenPredmet.Naziv = naziv_txt.Text;
            if (tip_opreme_txt.SelectedIndex == 1)
            {
                izmenjenPredmet.Tip = TipOpreme.staticka;
            }
            else
            {
                izmenjenPredmet.Tip = TipOpreme.dinamicka;
            }
            baza.Izmeni(izmenjenPredmet);
            Page upravljanje = new UpravljanjeOpremomPage();
            this.NavigationService.Navigate(upravljanje);
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            Page upravljanje = new UpravljanjeOpremomPage();
            this.NavigationService.Navigate(upravljanje);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultat = MessageBox.Show("Da li ste sigurni da zelite da obrisete opremu?", "", MessageBoxButton.YesNo);
            if (resultat == MessageBoxResult.Yes)
            {
                BazaOpreme baza = new BazaOpreme();
                BazaSadrzaja bazaSadrzaja = new BazaSadrzaja();
                List<Predmet> lista = baza.DobaviSve();
                Predmet izmenjenPredmet = baza.DobaviPoId(id_txt.Text);
                if (!bazaSadrzaja.PostojiOpremaUBolnici(id_txt.Text))
                {
                    izmenjenPredmet.Obrisano = true;
                    baza.Izmeni(izmenjenPredmet);
 
                }
                else
                {
                    MessageBox.Show("Oprema postoji na stanju, ne može biti obrisana!");
                }
            }
            Page upravljanje = new UpravljanjeOpremomPage();
            this.NavigationService.Navigate(upravljanje);
        }
    }
}
