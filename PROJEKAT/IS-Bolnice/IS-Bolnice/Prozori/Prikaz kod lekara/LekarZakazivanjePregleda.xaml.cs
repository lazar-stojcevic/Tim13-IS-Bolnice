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
using IS_Bolnice.Kontroleri;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarZakazivanjePregleda.xaml
    /// </summary>
    public partial class LekarZakazivanjePregleda : Window
    {
        private PregledKontroler pregledKontroler = new PregledKontroler();

        List<Pregled> pregledi = new List<Pregled>();
        public LekarZakazivanjePregleda()
        {
            InitializeComponent();
            LekarKontroler lekarKontroler = new LekarKontroler();
            listaLekara.ItemsSource = lekarKontroler.GetSviLekari();
          
        }

        private void Button_ClickZakazi(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da ste dobro selektovali sve podatke?", "Zakazivanje pregleda", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {

                Pregled noviPregled = KreirajNoviPregled();
                pregledKontroler.ZakaziPregled(noviPregled);
                MessageBox.Show("Pregled uspešno kreiran", "Kreiran pregled", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                this.Close();
            }
        }

        private Pregled KreirajNoviPregled()
        {
            Pregled noviPregled = new Pregled();
            noviPregled.Lekar = new Lekar();
            noviPregled.Pacijent = new Pacijent();

            Lekar lekar = (Lekar)listaLekara.SelectedItem;
            string idLekara = lekar.Jmbg;
            noviPregled.Lekar.Ordinacija = lekar.Ordinacija;
            
            Pregled pregled = pregledi.ElementAt(terminiList.SelectedIndex);

            DateTime pocetak = new DateTime(pregled.VremePocetkaPregleda.Year, pregled.VremePocetkaPregleda.Month,
                pregled.VremePocetkaPregleda.Day, pregled.VremePocetkaPregleda.Hour, pregled.VremePocetkaPregleda.Minute, 0);
            DateTime kraj = new DateTime(pregled.VremeKrajaPregleda.Year, pregled.VremeKrajaPregleda.Month,
                pregled.VremeKrajaPregleda.Day, pregled.VremeKrajaPregleda.Hour, pregled.VremeKrajaPregleda.Minute, 0);
            noviPregled.Lekar.Jmbg = idLekara;
            noviPregled.Pacijent.Jmbg = txtOperJmbg.Text;
            noviPregled.VremePocetkaPregleda = pocetak;
            noviPregled.VremeKrajaPregleda = kraj;
            noviPregled.Lekar.RadnoVreme = new RadnoVremeKontroler().DobaviRadnoVremeLekara(idLekara);
            return noviPregled;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li želida da odustanete od zakazivanje pregleda?", "Zakazivanje pregleda", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void lekariList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (listaLekara.SelectedIndex == -1)
                {
                    return;
                }

                if (terminiList.SelectedIndex == -1 || listaLekara.SelectedIndex == -1)
                {
                    potvrdi.IsEnabled = false;
                }
                else
                {
                    potvrdi.IsEnabled = true;
                }

                Lekar lekar = (Lekar) listaLekara.SelectedItem;
                pregledi = pregledKontroler.GetDostupniTerminiPregledaLekaraUNarednomPeriodu(lekar);

                terminiList.Items.Clear();

                foreach (Pregled p in pregledi)
                {
                    terminiList.Items.Add(p.VremePocetkaPregleda);
                }

                if (terminiList.Items.Count != 0)
                {
                    terminiList.SelectedIndex = 0;
                }

                if (terminiList.SelectedIndex == -1 || listaLekara.SelectedIndex == -1)
                {
                    potvrdi.IsEnabled = false;
                }
                else
                {
                    potvrdi.IsEnabled = true;
                }
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }

        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 1;
        }

        private void ToggleButton_OnUnchecked_UnChecked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 0;
        }

    }
}
