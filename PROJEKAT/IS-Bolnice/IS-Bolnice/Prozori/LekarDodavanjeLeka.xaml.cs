using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for LekarDodavanjeLeka.xaml
    /// </summary>
    public partial class LekarDodavanjeLeka : Window
    {
        ObservableCollection<Terapija> pomocna = new ObservableCollection<Terapija>();
        public LekarDodavanjeLeka(ObservableCollection<Terapija> terapija, string jmbgPacijenta)
        {
            BazaLekova bazaLekova = new BazaLekova();
            BazaPacijenata bazaPacijenata = new BazaPacijenata();
            List<Lek> lekovi = bazaLekova.SviLekovi();
            List<Lek> lekoviZaPrikaz = bazaLekova.SviLekovi();
            Pacijent p = bazaPacijenata.PacijentSaOvimJMBG(jmbgPacijenta);

            if(p.Alergeni.Count != 0)
            {
                int i = 0;
                foreach (string alergen in p.Alergeni)
                {
                    foreach (Lek lek in lekoviZaPrikaz)
                    {
                        ++i;
                        foreach (string alergenLek in lek.Alergeni)
                        {
                            Console.WriteLine("AAA");
                            Console.WriteLine(alergen);
                            Console.WriteLine("AAA");
                            if (alergenLek.Equals(alergen) && !alergenLek.Equals(""))
                            {
                                Console.WriteLine(alergenLek + "            " + alergen);
                                lekovi.RemoveAt(i);
                            }
                            --i;
                            break;
                        }
                    }
                    Console.WriteLine(alergen);
                }
            }

            InitializeComponent();
            listaSvihLekova.ItemsSource = lekovi;
            pomocna = terapija;

        }

        private void Button_DodajClick(object sender, RoutedEventArgs e)
        {
            Terapija t = new Terapija();
            Lek l = (Lek)listaSvihLekova.SelectedItem;
            t.Lek = l;
            t.UcestanostKonzumiranja = Double.Parse(txtBrojUzimanja.Text);
            t.VremePocetka = System.DateTime.Now;
            t.VremeKraja = DateTime.Now.AddDays(Int16.Parse(txtTrajanje.Text));
            t.Detalji = txtDetalji.Text;
            pomocna.Add(t);

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
