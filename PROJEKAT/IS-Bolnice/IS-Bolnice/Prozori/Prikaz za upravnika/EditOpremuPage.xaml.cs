using IS_Bolnice.Kontroleri;
using IS_Bolnice.Servisi;
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

namespace IS_Bolnice.Prozori.UpravnikPages
{
    /// <summary>
    /// Interaction logic for EditOpremuPage.xaml
    /// </summary>
    public partial class EditOpremuPage : Page
    {

        OpremaKontroler opremaKontroler = new OpremaKontroler();

        public EditOpremuPage(string selectedID)
        {
            InitializeComponent();
            id_txt.Text = selectedID;
            OpremaFajlRepozitorijum baza = new OpremaFajlRepozitorijum();
            Predmet izmenjenPredmet = baza.GetPoId(selectedID);
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
            if (Validiraj())
            {
                Predmet izmenjenPredmet = opremaKontroler.GetPoId(id_txt.Text);
                izmenjenPredmet.Naziv = naziv_txt.Text;
                izmenjenPredmet.Tip = GetTip();
                opremaKontroler.IzmeniPredmet(izmenjenPredmet);
                Page upravljanje = new UpravljanjeOpremomPage();
                this.NavigationService.Navigate(upravljanje);
            }
        }

        private bool Validiraj()
        {
            Regex regex = new Regex("^[#]+");
            if (regex.IsMatch(naziv_txt.Text))
            {
                return false;
            }
            return true;
        }
        private TipOpreme GetTip() 
        {
            if (tip_opreme_txt.SelectedIndex == 1)
            {
                return TipOpreme.staticka;
            }
            else
            {
                return TipOpreme.dinamicka;
            }
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
                if(!opremaKontroler.ObrisiPredmet(id_txt.Text)){ MessageBox.Show("Oprema postoji na stanju, ne može biti obrisana!"); }
            }
            Page upravljanje = new UpravljanjeOpremomPage();
            this.NavigationService.Navigate(upravljanje);
        }
    }
}
