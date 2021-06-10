using IS_Bolnice.Kontroleri;
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
    public partial class PrikazIzvestaja : Window
    {
        private string jmbgPacijenta;

        private IzvestajKontroler izvestajKontroler = new IzvestajKontroler();

        public PrikazIzvestaja(string jmbg)
        {
            InitializeComponent();

            jmbgPacijenta = jmbg;

            listaIzvestaja.ItemsSource = izvestajKontroler.GetSviIzvestajiPacijenta(jmbgPacijenta);
            foreach (Izvestaj izvestaj in izvestajKontroler.GetSviIzvestajiPacijenta(jmbgPacijenta))
            {
                Console.WriteLine(izvestaj.Lekar.Ime);
                Console.WriteLine(izvestaj.Lekar.Prezime);

            }
        }

        private void listaIzvestaja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Izvestaj selektovaniIzvestaj = (Izvestaj)listaIzvestaja.SelectedItem;
            opisIzvestaja.Text = selektovaniIzvestaj.Opis;
            listaTerapija.ItemsSource = selektovaniIzvestaj.Terapija;
        }

        private void listaTerapija_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Terapija selektovanaTerapija = (Terapija)listaTerapija.SelectedItem;
            if (selektovanaTerapija != null)
            {
                opisTerapije.Text = selektovanaTerapija.Opis;
            }
            else
            {
                opisTerapije.Text = "";
            }

        }

        private void napraviBelezku_Click(object sender, RoutedEventArgs e)
        {
            KreiranjeBelezke kreiranjeBelezke = new KreiranjeBelezke(jmbgPacijenta);
            kreiranjeBelezke.ShowDialog();
        }

        private void izdadji_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
