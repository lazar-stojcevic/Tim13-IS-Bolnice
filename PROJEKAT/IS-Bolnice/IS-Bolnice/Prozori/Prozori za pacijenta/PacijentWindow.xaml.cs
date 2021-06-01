using IS_Bolnice.Prozori;
using IS_Bolnice.Prozori.Prozori_za_pacijenta;
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
    public partial class PacijentWindow : Window
    {
        public PacijentWindow()
        {
            InitializeComponent();
        }

        private void zakaziBtn_Click(object sender, RoutedEventArgs e)
        {
            string[] items = imeKorisnika.Text.Split(); //items[2] jmbg

            OdabirZakazivanja oz = new OdabirZakazivanja(items[2]);
            oz.ShowDialog();
        }

        private void prikaziBtn_Click(object sender, RoutedEventArgs e)
        {
            string[] items = imeKorisnika.Text.Split();
            PacijentPregledZakazanihTermina pp = new PacijentPregledZakazanihTermina(items[2]);
            pp.ShowDialog();
        }

        private void odjavaBtn_Click(object sender, RoutedEventArgs e)
        {
            
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void anketaBtn_Click(object sender, RoutedEventArgs e)
        {
            string[] items = imeKorisnika.Text.Split();
            FormaZaPopunjavanjeAnkete fzpa = new FormaZaPopunjavanjeAnkete(items[2]);
            fzpa.ShowDialog();
        }

        private void obavestenjaBtn_Click(object sender, RoutedEventArgs e)
        {
            string[] items = imeKorisnika.Text.Split();
            Obavestenje o = new Obavestenje(items[2]);
            o.ShowDialog();
        }

        private void operacijeBtn_Click(object sender, RoutedEventArgs e)
        {
            string[] items = imeKorisnika.Text.Split();
            PrikazTerminaOperacija pto = new PrikazTerminaOperacija(items[2]);
            pto.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] items = imeKorisnika.Text.Split(); //items[2] jmbg

            OdabirZakazivanja oz = new OdabirZakazivanja(items[2]);
            oz.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string[] items = imeKorisnika.Text.Split();
            Obavestenje o = new Obavestenje(items[2]);
            o.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string[] items = imeKorisnika.Text.Split();
            PacijentPregledZakazanihTermina pp = new PacijentPregledZakazanihTermina(items[2]);
            pp.ShowDialog();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string[] items = imeKorisnika.Text.Split();
            PrikazTerminaOperacija pto = new PrikazTerminaOperacija(items[2]);
            pto.ShowDialog();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            string[] items = imeKorisnika.Text.Split();
            FormaZaPopunjavanjeAnkete fzpa = new FormaZaPopunjavanjeAnkete(items[2]);
            fzpa.ShowDialog();
        }
    }
}
