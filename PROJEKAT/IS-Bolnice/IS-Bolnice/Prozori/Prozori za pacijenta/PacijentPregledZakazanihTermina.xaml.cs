using IS_Bolnice.Prozori.Prozori_za_pacijenta;
using System.Windows;
using System.Windows.Controls;
using IS_Bolnice.Kontroleri;

namespace IS_Bolnice.Prozori
{
    public partial class PacijentPregledZakazanihTermina : Window
    {
        private readonly string jmbgPac;

        private PregledKontroler pregledKontroler = new PregledKontroler();

        public PacijentPregledZakazanihTermina(string jmbgPacijenta)
        {
            InitializeComponent();
            jmbgPac = jmbgPacijenta;

            lvPregledi.ItemsSource = pregledKontroler.GetSviBuduciSortiraniPreglediPacijenta(jmbgPacijenta);
        }

        private void obrisiTermin_Click(object sender, RoutedEventArgs e)
        {
            Pacijent pacijent = new Pacijent();
            pacijent.Jmbg = jmbgPac;

            Pregled pregled = (Pregled)lvPregledi.SelectedItem;
            pregled.Pacijent = pacijent;

            pregledKontroler.OtkaziPregled(pregled);

            lvPregledi.ItemsSource = pregledKontroler.GetSviBuduciSortiraniPreglediPacijenta(jmbgPac);
        }

        private void izadji_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void izmeniTermin_Click(object sender, RoutedEventArgs e)
        {
            Pregled pregledZaIzmenu = (Pregled)lvPregledi.SelectedItem;
            PacijentIzmenaTermina pit = new PacijentIzmenaTermina(pregledZaIzmenu, lvPregledi);
            pit.ShowDialog();
        }

        private void lvPregledi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = lvPregledi.SelectedIndex;

            if (index == -1)
            {
                izmeniTermin.IsEnabled = false;
                obrisiTermin.IsEnabled = false;
            }
            else
            {
                obrisiTermin.IsEnabled = true;
                izmeniTermin.IsEnabled = true;
            }
        }

        private void Izvestaj_Click(object sender, RoutedEventArgs e)
        {
            PrikazIzvestaja pi = new PrikazIzvestaja(jmbgPac);
            pi.ShowDialog();
        }
    }
}
