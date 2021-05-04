using IS_Bolnice.Baze;
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

namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for ZakazivanjeHitnogTermina.xaml
    /// </summary>
    public partial class ZakazivanjeHitnogTermina : Window
    {
        private Pacijent pacijent = new Pacijent();
        private BazaPacijenata bazaPacijenata = new BazaPacijenata();
        private BazaOblastiLekara bazaOblastiLekara = new BazaOblastiLekara();

        public ZakazivanjeHitnogTermina()
        {
            InitializeComponent();

            PopunjavanjeOblastiLekara();
            PopunjavanjePonudjenihTrajanja();
        }

        private void PopunjavanjeOblastiLekara()
        {
            List<OblastLekara> sveOblastiIzBaze = bazaOblastiLekara.SveOblasti();
            List<string> sveOblastiZaPrikaz = new List<string>();

            foreach (OblastLekara oblast in sveOblastiIzBaze)
            {
                sveOblastiZaPrikaz.Add(oblast.Naziv);
            }
            comboOblastLekara.ItemsSource = sveOblastiZaPrikaz;
        }

        private void PopunjavanjePonudjenihTrajanja()
        {
            List<double> trajanja = new List<double>();
            for (double i = 0.5; i <= 24; i += 0.5)
            {
                trajanja.Add(i);
            }
            comboTrajanja.ItemsSource = trajanja;
        }

        private void UpdateTextBox()
        {
            if (pacijent != null)
            {
                odabraniPacijent.Text = pacijent.Ime + " " + pacijent.Prezime + " (JMBG: " + pacijent.Jmbg + ")";
            }
        }

        private void Button_Click_Postojeci(object sender, RoutedEventArgs e)
        {
            PrikazSvihPacijenata prikazSvihPacijenata = new PrikazSvihPacijenata(pacijent);
            prikazSvihPacijenata.ShowDialog();
            UpdateTextBox();
        }

        private void Button_Click_Gostujuci(object sender, RoutedEventArgs e)
        {
            DodavanjeGuestNalogaWindow dodavanjeGostujuceg = new DodavanjeGuestNalogaWindow();
            dodavanjeGostujuceg.ShowDialog();
            // u slucaju dodavanja gostujuceg uzima se poslednji iz baze
            pacijent = bazaPacijenata.poslednjiDodat();
            UpdateTextBox();
        }
    }
}
