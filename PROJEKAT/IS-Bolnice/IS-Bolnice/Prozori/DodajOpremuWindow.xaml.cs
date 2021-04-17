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
    /// Interaction logic for DodajOpremuWindow.xaml
    /// </summary>
    public partial class DodajOpremuWindow : Window
    {
        public DodajOpremuWindow()
        {
            InitializeComponent();
        }

        private void Dodaj_btn_Click(object sender, RoutedEventArgs e)
        {
            Predmet noviPredmet = new Predmet();
            noviPredmet.Id = id_txt.Text;
            noviPredmet.Naziv = naziv_txt.Text;
            if (tip_opreme_txt.SelectedIndex == 0) {
                noviPredmet.Tip = TipOpreme.staticka;
            }
            else {
                noviPredmet.Tip = TipOpreme.dinamicka;
            }
            BazaOpreme baza = new BazaOpreme();
            List<Predmet> lista = baza.SvaOprema();
            lista.Add(noviPredmet);
            baza.KreirajOpremu(lista);
            UpravljanjeWindow upravljanjeWindow = new UpravljanjeWindow();
            upravljanjeWindow.Show();
            this.Close();
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            UpravljanjeWindow upravljanjeWindow = new UpravljanjeWindow();
            upravljanjeWindow.Show();
            this.Close();
        }
    }
}
