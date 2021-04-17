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
    /// Interaction logic for IzmeniOpremuWindow.xaml
    /// </summary>
    public partial class IzmeniOpremuWindow : Window
    {
        public IzmeniOpremuWindow(string id, string naziv, TipOpreme tipOpreme)
        {
            InitializeComponent();
            id_txt.Text = id;
            naziv_txt.Text = naziv;
            if (tipOpreme == TipOpreme.staticka)
            {
                tip_opreme_txt.SelectedIndex = 0;
            }
            else
            {
                tip_opreme_txt.SelectedIndex = 1;
            }

        }

        private void Izmeni_btn_Click(object sender, RoutedEventArgs e)
        {
            BazaOpreme baza = new BazaOpreme();
            List<Predmet> lista = baza.SvaOprema();
            foreach (Predmet p in lista) {
                if (p.Id == id_txt.Text) {
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
