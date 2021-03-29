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
    public partial class PacijentPregledZakazanihTermina : Window
    {
        private string jmbgPac;

        public PacijentPregledZakazanihTermina(string jmbgPacijenta)
        {
            InitializeComponent();
            jmbgPac = jmbgPacijenta;

            BazaPregleda bp = new BazaPregleda();
            List<Pregled> pregledi = bp.SviSledeciPregledi();
            foreach(Pregled p in pregledi)
            {
                Console.WriteLine(p.VremePocetkaPregleda.ToString());
            }
            
            lvPregledi.ItemsSource = bp.PreglediOdredjenogPacijenta(jmbgPacijenta);
        }

        private void obrisiTermin_Click(object sender, RoutedEventArgs e)
        {
            // dodati da li si siguran dijalog
            BazaPregleda bp = new BazaPregleda();
            Pacijent pac = new Pacijent();
            pac.Jmbg = jmbgPac;

            Pregled p = (Pregled)lvPregledi.SelectedItem;
            p.Pacijent = pac;

            bp.OtkaziPregled(p);
            this.Close();
        }

        private void izadji_Click(object sender, RoutedEventArgs e)
        {
            // dodati da li si siguran?
            this.Close();
        }

        private void izmeniTermin_Click(object sender, RoutedEventArgs e)
        {
            Pregled p = (Pregled)lvPregledi.SelectedItem;
            PacijentIzmenaTermina pit = new PacijentIzmenaTermina(jmbgPac, p.Lekar.Jmbg, p.VremePocetkaPregleda.ToString("HH:mm"), p.VremePocetkaPregleda);
            pit.ShowDialog();
        }

        private void lvPregledi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pregled p = new Pregled();
            p = (Pregled)lvPregledi.SelectedItem;
            if (p.Equals(null))
            {
                izmeniTermin.IsEnabled = false;
            }
            else
            {
                izmeniTermin.IsEnabled = true;
            }
        }
    }
}
