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
using System.Windows.Shapes;

namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for UpravljanjeWindow.xaml
    /// </summary>
    public partial class UpravljanjeWindow : Window
    {
        public UpravljanjeWindow()
        {
            InitializeComponent();
            BazaOpreme baza = new BazaOpreme();
            List<Predmet> predmeti = new List<Predmet>();
            predmeti = baza.SvaOprema();
            List<string> tekst = new List<string>();
            foreach (Predmet predmet in predmeti) {

                if (predmet.Obrisano == false)
                {
                    tekst.Add("ID: " + predmet.Id + " Naziv: " + predmet.Naziv + " Tip: " + predmet.Tip);
                }
            }
            lvDataBinding.ItemsSource = tekst;
        }

        private void Button_Izmeni(object sender, RoutedEventArgs e)
        {
            if (lvDataBinding.SelectedIndex == -1)
            {
                MessageBox.Show("Oprema nije selektovana za izmenu");
            }
            else
            {
                Predmet selected = new Predmet();
                BazaOpreme baza = new BazaOpreme();
                List<Predmet> predmeti = new List<Predmet>();
                predmeti = baza.SvaOprema();
                string tekst = (string)lvDataBinding.SelectedItem;
                string[] niz = tekst.Split(' ');
                foreach (Predmet p in predmeti)
                {
                        if (p.Id.Equals(niz[1]))
                        {
                            selected = p;
                            break;
                        }

                }
                IzmeniOpremuWindow izmeniOpremuWindow = new IzmeniOpremuWindow(selected.Id, selected.Naziv, selected.Tip);
                izmeniOpremuWindow.Show();
                this.Close();
            }

  
        }

        private void Button_Dodaj(object sender, RoutedEventArgs e)
        {
            DodajOpremuWindow dodajOpremuWindow = new DodajOpremuWindow();
            dodajOpremuWindow.Show();
            this.Close();
        }

        private void Button_Obrisi(object sender, RoutedEventArgs e)
        {
            if (lvDataBinding.SelectedIndex == -1)
            {
                MessageBox.Show("Oprema nije selektovana za brisanje");
            }
            else
            {
                MessageBoxResult resultat = MessageBox.Show("Da li ste sigurni da zelite da obrisete opremu?", "", MessageBoxButton.YesNo);
                if (resultat == MessageBoxResult.Yes)
                { 
                    BazaOpreme baza = new BazaOpreme();
                    List<Predmet> predmeti = new List<Predmet>();
                    predmeti = baza.SvaOprema();
                    string tekst = (string)lvDataBinding.SelectedItem;
                    string[] niz = tekst.Split(' ');
                    foreach (Predmet p in predmeti)
                    {
                        if (p.Id.Equals(niz[1]))
                        {
                            p.Obrisano = true;
                            break;
                        }

                    }
                    baza.KreirajOpremu(predmeti);

                    UpravljanjeWindow upravljanjeWindow = new UpravljanjeWindow();
                    upravljanjeWindow.Show();
                    this.Close();
                }

            }
        }
    }
}
