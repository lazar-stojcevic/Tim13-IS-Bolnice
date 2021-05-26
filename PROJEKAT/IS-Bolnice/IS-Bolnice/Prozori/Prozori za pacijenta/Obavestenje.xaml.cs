using IS_Bolnice.Kontroleri;
using IS_Bolnice.Model;
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

namespace IS_Bolnice.Prozori.Prozori_za_pacijenta
{
    /// <summary>
    /// Interaction logic for Obavestenje.xaml
    /// </summary>
    public partial class Obavestenje : Window
    {
        private string jmbgPacijenta;

        private BelezkeKontroler belezkaKontroler = new BelezkeKontroler();
        public Obavestenje(string jmbg)
        {
            InitializeComponent();
            jmbgPacijenta = jmbg;

            listaObavestenja.ItemsSource = belezkaKontroler.SveTrenutneBelezkePacijenta(jmbgPacijenta);
        }

        private void kreirajBelezku_Click(object sender, RoutedEventArgs e)
        {
            KreiranjeBelezke kb = new KreiranjeBelezke(jmbgPacijenta, listaObavestenja);
            kb.ShowDialog();
        }

        private void izadji_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void listaObavestenja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int rbrSelekcijeUListi = listaObavestenja.SelectedIndex;
            if (rbrSelekcijeUListi != -1)
            {
                obrisiBelezku.IsEnabled = true;
                izmeniBelezku.IsEnabled = true;

                Belezka selektovanaBelezka = (Belezka)listaObavestenja.SelectedItem;
                poljeZaIspis.Text = selektovanaBelezka.Komentar;
            }
            else
            {
                obrisiBelezku.IsEnabled = false;
                izmeniBelezku.IsEnabled = false;
            }
        }

        private void obrisiBelezku_Click(object sender, RoutedEventArgs e)
        {
            Belezka belezkaZaBrisanje = (Belezka)listaObavestenja.SelectedItem;
            belezkaKontroler.ObrisiBelezku(belezkaZaBrisanje);
            poljeZaIspis.Text = "";
            listaObavestenja.ItemsSource = belezkaKontroler.SveTrenutneBelezkePacijenta(jmbgPacijenta);
        }

        private void izmeniBelezku_Click(object sender, RoutedEventArgs e)
        {
            Belezka belezkaZaIzmenu = (Belezka)listaObavestenja.SelectedItem;
            IzmenaBelezke ib = new IzmenaBelezke(belezkaZaIzmenu, listaObavestenja);
            ib.ShowDialog();
        }
    }
}
