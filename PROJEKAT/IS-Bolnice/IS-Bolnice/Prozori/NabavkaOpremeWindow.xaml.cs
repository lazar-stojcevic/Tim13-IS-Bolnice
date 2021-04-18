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
    /// Interaction logic for NabavkaOpremeWindow.xaml
    /// </summary>
    public partial class NabavkaOpremeWindow : Window
    {
        public NabavkaOpremeWindow()
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
            lvDataBinding.ItemsSource = tekst;
        }

        private void Button_Plus(object sender, RoutedEventArgs e)
        {
            int i = Int32.Parse(kol_txt.Text);
            i++;
            kol_txt.Text = i.ToString();
        }

        private void Button_Minus(object sender, RoutedEventArgs e)
        {
            int i = Int32.Parse(kol_txt.Text);
            if (i > 0)
            {
                i--;
                kol_txt.Text = i.ToString();
            }
        }

        private void Button_Nazad(object sender, RoutedEventArgs e)
        {
            OpremaWindow opremaWindow = new OpremaWindow();
            opremaWindow.Show();
            this.Close();
        }

        private void Button_Dodaj(object sender, RoutedEventArgs e)
        {
            BazaBolnica baza = new BazaBolnica();
            List<Bolnica> bolnice = baza.SveBolnice();
            Bolnica b = bolnice[0];
            bool postoji= false;
            string[] predmet =lvDataBinding.SelectedItem.ToString().Split(' ');
            string id = predmet[1];
            foreach (Soba s in b.Soba) {
                if (s.Tip == RoomType.magacin) {
                    foreach (Predmet p in s.Predmet) {
                        if (p.Id.Equals(id)) {
                            p.Kolicina += Int32.Parse(kol_txt.Text);
                            postoji = true;
                            break;
                        }
                    }
                    if (!postoji) {
                        Predmet p = new Predmet();
                        p.Id = id;
                        p.Kolicina = Int32.Parse(kol_txt.Text);
                        s.AddPredmet(p);
                    }
                    break;
                }
            }
            baza.KreirajBolnicu(b);
            NabavkaOpremeWindow nabavkaOpremeWindow = new NabavkaOpremeWindow();
            nabavkaOpremeWindow.Show();
            this.Close();
        }

        private void lvDataBinding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
