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
    public partial class KreiranjeBelezke : Window
    {
        private string jmbgPacijenta;

        private BelezkeKontroler belezkeKontroler = new BelezkeKontroler();

        ListView listaZaOsvezavanje;
        public KreiranjeBelezke(string jmbg, ListView listaObavestenja = null)
        {
            InitializeComponent();
            jmbgPacijenta = jmbg;

            listaZaOsvezavanje = listaObavestenja;
        }

        private void odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void potvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (nazivBelezke.Text.Trim() == "" || brojDana.Text.Trim() == "")
            {
                potvrdi.IsEnabled = false;
            }
            else
            {
                belezkeKontroler.SacuvajBelezku(napraviBelezku());
                MessageBox.Show("Uspešno ste kreirali beležku");

                if(listaZaOsvezavanje != null)
                    listaZaOsvezavanje.ItemsSource = belezkeKontroler.SveTrenutneBelezkePacijenta(jmbgPacijenta);

                this.Close();
            }
        }
        private Beleska napraviBelezku()
        {
            Pacijent pacijent = new Pacijent();
            pacijent.Jmbg = jmbgPacijenta;

            return new Beleska(pacijent, sadrzajBelezke.Text, DateTime.Now, Int32.Parse(brojDana.Text), nazivBelezke.Text);
        }
        private void nazivBelezke_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (brojDana.Text.Trim() == "")
            {
                potvrdi.IsEnabled = false;
            }
            else
            {
                potvrdi.IsEnabled = true;
            }
        }

        private void brojDana_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (nazivBelezke.Text.Trim() == "")
            {
                potvrdi.IsEnabled = false;
            }
            else
            {
                potvrdi.IsEnabled = true;
            }
        }

        private void sadrzajBelezke_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
