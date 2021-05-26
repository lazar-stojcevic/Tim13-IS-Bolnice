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
    public partial class IzmenaBelezke : Window
    {
        private Belezka staraBelezka;

        private BelezkeKontroler belezkeKontroleri = new BelezkeKontroler();

        private ListView listaZaOsvezavanje;
        public IzmenaBelezke(Belezka belezka, ListView listView)
        {
            InitializeComponent();
            staraBelezka = belezka;
            listaZaOsvezavanje = listView;

            nazivBelezke.Text = staraBelezka.Naziv;
            sadrzajBelezke.Text = staraBelezka.Komentar;
            brojDana.Text = staraBelezka.PeriodVazenja.ToString();
        }

        private void potvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (nazivBelezke.Text.Trim() != "" && sadrzajBelezke.Text.Trim() != "" && brojDana.Text.Trim() != "")
            {
                Pacijent pacijent = new Pacijent();
                pacijent.Jmbg = staraBelezka.Pacijent.Jmbg;

                Belezka novaBelezka = new Belezka(pacijent, sadrzajBelezke.Text, DateTime.Now, Int32.Parse(brojDana.Text), nazivBelezke.Text);

                belezkeKontroleri.IzmeniBelezku(staraBelezka, novaBelezka);
                listaZaOsvezavanje.ItemsSource = belezkeKontroleri.SveTrenutneBelezkePacijenta(staraBelezka.Pacijent.Jmbg);
                this.Close();
            }
        }

        private void odustani_Click(object sender, RoutedEventArgs e)
        {

        }

        private void sadrzajBelezke_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void brojDana_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void nazivBelezke_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
