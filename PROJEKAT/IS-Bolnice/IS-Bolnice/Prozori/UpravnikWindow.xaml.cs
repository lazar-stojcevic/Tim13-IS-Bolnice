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

namespace IS_Bolnice
{
    /// <summary>
    /// Interaction logic for UpravnikWindow.xaml
    /// </summary>
    public partial class UpravnikWindow : Window
    {
        public UpravnikWindow()
        {
            InitializeComponent();
            List<Soba> lista = new List<Soba>();
            List<Bolnica> bolnice = new List<Bolnica>();
            BazaBolnica baza = new BazaBolnica();
            bolnice = baza.SveBolnice();
            foreach (Bolnica b in bolnice)
            {
                foreach (Soba s in b.Soba)
                {

                    if (s.Obrisano == false)
                    {
                        lista.Add(s);

                    }
                }

            }

            List<string> tekst = new List<string>();
            foreach (Soba s in lista)
            {
                if (s.Obrisano == false)
                {
                    tekst.Add("ID: " + s.Id + " Sprat: " + s.Sprat.ToString() + " Tip: " + s.Tip);
                }
            }
            lvDataBinding.ItemsSource = tekst;
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {


            if (Tg_Btn.IsChecked == true)
            {
                tt_home.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_home.Visibility = Visibility.Visible;
            }
        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {

        }


        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        private void Button_Obrisi(object sender, RoutedEventArgs e)
        {
            if (lvDataBinding.SelectedIndex == -1)
            {
                MessageBox.Show("Soba nije selektovana za brisanje");
            }
            else
            {
                MessageBoxResult resultat = MessageBox.Show("Da li ste sigurni da zelite da obrisete sobu?", "", MessageBoxButton.YesNo);
                if (resultat == MessageBoxResult.Yes)
                {
                    List<Bolnica> bolnice = new List<Bolnica>();
                    BazaBolnica baza = new BazaBolnica();
                    bolnice = baza.SveBolnice();
                    string tekst = (string)lvDataBinding.SelectedItem;
                    string[] niz = tekst.Split(' ');
                    foreach (Bolnica b in bolnice)
                    {
                        foreach (Soba s in b.Soba)
                        {

                            if (s.Id == niz[1])
                            {
                                s.Obrisano = true;
                                baza.KreirajBolnicu(b);
                                break;
                            }
                        }

                    }
               
                    UpravnikWindow upravnik = new UpravnikWindow();
                    upravnik.Show();
                    this.Close();
                }

            }
        }

        private void Button_Dodaj(object sender, RoutedEventArgs e)
        {
            Prozori.DodajSobuWindow dodajSobuWindow = new Prozori.DodajSobuWindow();
            dodajSobuWindow.Show();
            this.Close();

        }

        private void Button_Izmeni(object sender, RoutedEventArgs e)
        {

            if (lvDataBinding.SelectedIndex == -1)
            {
                MessageBox.Show("Soba nije selektovana za izmenu");
            }
            else
            {
                Soba selected = new Soba();
                List<Bolnica> bolnice = new List<Bolnica>();
                BazaBolnica baza = new BazaBolnica();
                bolnice = baza.SveBolnice();
                string tekst = (string)lvDataBinding.SelectedItem;
                string[] niz = tekst.Split(' ');
                foreach (Bolnica b in bolnice)
                {
                    foreach (Soba s in b.Soba)
                    {

                        if (s.Id == niz[1])
                        {
                            selected = s;
                            break;
                        }
                    }

                }
                Prozori.IzmeniSobuWindow izmeniSobuWindow = new Prozori.IzmeniSobuWindow(selected.Id, selected.Tip, selected.Sprat, selected.Kvadratura);
                izmeniSobuWindow.Show();
                this.Close();
            }
        }
    }
}
