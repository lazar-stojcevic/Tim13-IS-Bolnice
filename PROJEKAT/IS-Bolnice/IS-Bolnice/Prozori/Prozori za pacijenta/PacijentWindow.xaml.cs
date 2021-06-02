using IS_Bolnice.Prozori;
using IS_Bolnice.Prozori.Prozori_za_pacijenta;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void izvestaj_Click(object sender, RoutedEventArgs e)
        {
            string[] items = imeKorisnika.Text.Split();
            PrikazIzvestaja pi = new PrikazIzvestaja(items[2]);
            pi.ShowDialog();
        }

        private void zakazivanjeKodLekara_Click(object sender, RoutedEventArgs e)
        {
            string[] items = imeKorisnika.Text.Split();
            ZakazivanjeKodOdredjenogLekara zkol = new ZakazivanjeKodOdredjenogLekara(items[2]);
            zkol.ShowDialog();
        }
        private void zakazivanjeUTerminu_Click(object sender, RoutedEventArgs e)
        {
            string[] items = imeKorisnika.Text.Split();
            ZakazivanjeUOdredjenomTerminu ziot = new ZakazivanjeUOdredjenomTerminu(items[2]);
            ziot.ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            PromeniTemu(sender);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void PromeniTemu(object sender)
        {
            var app = (App)Application.Current;

            MenuItem menuItem = (MenuItem)sender;
            if(menuItem.Header.Equals("Tamna tema"))
            {
                app.PromeniTemu(new Uri("Skinovi/TamnaTema.xaml", UriKind.Relative));
            }
            else
            {
                app.OcistiTemu();
            }
        }
    }
}
