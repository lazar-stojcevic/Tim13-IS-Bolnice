using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
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
    /// Interaction logic for LekarRaspored.xaml
    /// </summary>
    public partial class LekarRaspored : Page
    {
        private string sifra;

        private OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
        PregledKontroler pregledKontroler = new PregledKontroler();

        ObservableCollection<Operacija> opKolekcija = new ObservableCollection<Operacija>();

        ObservableCollection<Pregled> preglediKolekcija = new ObservableCollection<Pregled>();
        public LekarRaspored(string id)
        {
            InitializeComponent();
            sifra = id;
            List<Operacija> sveSledeceOperacije = operacijaKontroler.GetSveSledeceOperacijeLekara(id);
            foreach (Operacija operacija in sveSledeceOperacije)
            {
                opKolekcija.Add(operacija);
            }
            listaOperacija.ItemsSource = opKolekcija;


            List<Pregled> pr = pregledKontroler.GetSviBuduciPreglediLekara(id);
            foreach (Pregled pregled in pr)
            {
                preglediKolekcija.Add(pregled);
            }
            listaPregleda.ItemsSource = preglediKolekcija;
        }

        private void Button_IzmeniOperaciju(object sender, RoutedEventArgs e)
        {
            if (listaOperacija.SelectedIndex == -1)
            {
                MessageBox.Show("Nista nije selektovano", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Operacija selektovani = (Operacija)listaOperacija.SelectedItem;
            string ime = selektovani.Pacijent.Ime;
            string prz = selektovani.Pacijent.Prezime;
            string jmbg = selektovani.Pacijent.Jmbg;
            DateTime datum = selektovani.VremePocetkaOperacije.Date;
            Console.WriteLine(datum);
            DateTime vreme = selektovani.VremePocetkaOperacije;
            //TODO napravi lepi kostruktor
            IzmenaOperacije izmena = new IzmenaOperacije();
            izmena.txtOperIme.Text = ime;
            izmena.txtOperPrz.Text = prz;
            izmena.txtOperJmbg.Text = jmbg;
            izmena.terminiList.Items.Add(selektovani.VremePocetkaOperacije);
            izmena.boxHitno.IsChecked = selektovani.Hitna;
            izmena.comboBoxSale.SelectedIndex = 0;
            izmena.listaLekara.SelectedIndex = 0;
            izmena.StariSat = vreme.Hour.ToString();
            izmena.StariMinut = vreme.Minute.ToString();
            izmena.StariDatum = datum;

            izmena.ShowDialog();

            List<Operacija> op = operacijaKontroler.GetSveSledeceOperacijeLekara(sifra);
            opKolekcija.Clear();
            foreach (Operacija operacija in op)
            {
                opKolekcija.Add(operacija);
            }

        }

        private void Button_ClickObrisi(object sender, RoutedEventArgs e)
        {
            if (listaOperacija.SelectedIndex == -1)
            {
                MessageBox.Show("Nista nije selektovano", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da želite da otkažete ovu operaciju?", "Otkazivanje operacije", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Operacija selektovani = (Operacija) listaOperacija.SelectedItem;
                operacijaKontroler.OtkaziOperaciju(selektovani);
                opKolekcija.Remove(selektovani);
               
            }
        }

        private void Button_IzmeniPregled(object sender, RoutedEventArgs e)
        {
            if (listaPregleda.SelectedIndex == -1) { 
                MessageBox.Show("Nista nije selektovano","Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Pregled selektovani = (Pregled)listaPregleda.SelectedItem;
            string ime = selektovani.Pacijent.Ime;
            string prz = selektovani.Pacijent.Prezime;
            string jmbg = selektovani.Pacijent.Jmbg;
            DateTime datum = selektovani.VremePocetkaPregleda.Date;
            DateTime vreme = selektovani.VremePocetkaPregleda;

            LekarIzmenaPregleda izmena = new LekarIzmenaPregleda();

            izmena.txtOperIme.Text = ime;
            izmena.txtOperPrz.Text = prz;
            izmena.txtOperJmbg.Text = jmbg;
            izmena.terminiList.Items.Add(selektovani.VremePocetkaPregleda);
            //OVEJ DEO TREBA MODIFIKOVATI
            izmena.listaLekara.SelectedIndex = 0;
            izmena.StariSat = vreme.Hour.ToString();
            izmena.StariMinut = vreme.Minute.ToString();
            izmena.StariDatum = datum;

            izmena.ShowDialog();

            List<Pregled> pr = pregledKontroler.GetSviBuduciPreglediLekara(sifra);
            preglediKolekcija.Clear();
            foreach (Pregled pregled in pr)
            {
                preglediKolekcija.Add(pregled);
            }


        }

        private void Button_ClickObrisiPregled(object sender, RoutedEventArgs e)
        {
            if (listaPregleda.SelectedIndex == -1)
            {
                MessageBox.Show("Nista nije selektovano", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da želite da otkažete ovaj pregled?", "Otkazivanje pregleda", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Pregled selektovani = (Pregled) listaPregleda.SelectedItem;
                pregledKontroler.OtkaziPregled(selektovani);
                preglediKolekcija.Remove(selektovani);
            }
        }

        private void Button_VidiPacijenta(object sender, RoutedEventArgs e)
        {
            if (listaOperacija.SelectedIndex == -1)
            {
                MessageBox.Show("Nista nije selektovano", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Operacija selektovana = (Operacija)listaOperacija.SelectedItem;
            PodaciOPacijentu pod = new PodaciOPacijentu(selektovana.Pacijent.Jmbg);
            NavigationService.Navigate(pod);
        }

        private void Button_VidiPacijentaPr(object sender, RoutedEventArgs e)
        {
            if (listaPregleda.SelectedIndex == -1)
            {
                MessageBox.Show("Nista nije selektovano", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Pregled selektovana = (Pregled)listaPregleda.SelectedItem;
            PodaciOPacijentu pod = new PodaciOPacijentu(selektovana.Pacijent.Jmbg);
            NavigationService.Navigate(pod);

        }

        private void Button_OtvoriPregled(object sender, RoutedEventArgs e)
        {
            if (listaPregleda.SelectedIndex == -1)
            {
                MessageBox.Show("Nista nije selektovano", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Pregled selektovanPregled = (Pregled)listaPregleda.SelectedItem;
            LekarWindow prozorZaPregled = new LekarWindow(sifra);
            //TODO napravi lepi konstruktor

            prozorZaPregled.txtJMBG.Text = selektovanPregled.Pacijent.Jmbg;
            prozorZaPregled.txtIme.Text = selektovanPregled.Pacijent.Ime;
            prozorZaPregled.txtPrz.Text = selektovanPregled.Pacijent.Prezime;

            prozorZaPregled.btnPregled.IsEnabled = true;
            prozorZaPregled.btnOperacija.IsEnabled = true;
            prozorZaPregled.btnIzvestaj.IsEnabled = true;
            prozorZaPregled.btnHospitalizacija.IsEnabled = true;
            this.NavigationService.Navigate(prozorZaPregled);

        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 1;
        }

        private void ToggleButton_OnUnchecked_UnChecked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 0;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
