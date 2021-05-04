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
        }

        private void Izmeni_btn_Click(object sender, RoutedEventArgs e)
        {
            BazaOpreme baza = new BazaOpreme();
            List<Predmet> lista = baza.SvaOprema();
            foreach (Predmet p in lista)
            {
                if (p.Id == id_txt.Text)
                {
                    p.Naziv = naziv_txt.Text;
                    if (tip_opreme_txt.SelectedIndex == 0)
                    {
                        p.Tip = TipOpreme.staticka;
                    }
                    else
                    {
                        p.Tip = TipOpreme.dinamicka;
                    }
                }
            }
            baza.KreirajOpremu(lista);
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
            /*MessageBoxResult resultat = MessageBox.Show("Da li ste sigurni da zelite da obrisete opremu?", "", MessageBoxButton.YesNo);
            if (resultat == MessageBoxResult.Yes)
            {
                BazaOpreme baza = new BazaOpreme();
                List<Predmet> predmeti = new List<Predmet>();
                predmeti = baza.SvaOprema();
                string tekst = (string)lvDataBindin.SelectedItem;
                string[] niz = tekst.Split(' ');
                if (!this.CheckNumber(niz[1]))
                {
                    foreach (Predmet p in predmeti)
                    {
                        if (p.Id.Equals(niz[1]))
                        {
                            p.Obrisano = true;
                            break;
                        }

                    }
                    baza.KreirajOpremu(predmeti);
 
                }
                else
                {
                    MessageBox.Show("Oprema postoji na stanju, ne može biti obrisana!");
                }
            }*/
        }
    }
}
