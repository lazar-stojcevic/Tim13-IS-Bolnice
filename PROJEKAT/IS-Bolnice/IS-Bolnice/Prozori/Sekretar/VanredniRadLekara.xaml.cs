using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using IS_Bolnice.Repozitorijumi;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Kontroleri.Korisnicki;
using IS_Bolnice.Model;

namespace IS_Bolnice.Prozori.Sekretar
{
    public partial class VanredniRadLekara : Window
    {
        private Lekar selektovaniLekar;
        private RadnoVremeKontroler radnoVremeKontroler = new RadnoVremeKontroler();

        public ObservableCollection<VremenskiInterval> VanrednaRadnaVremenaLekara
        {
            get;
            set;
        }

        public VanredniRadLekara(Lekar lekar)
        {
            InitializeComponent();
            this.DataContext = this;

            selektovaniLekar = lekar;
            tbLekar.Text = lekar.Ime + " " + lekar.Prezime;

            PopuniComboBoxSati();
            PopuniComboBoxMinuta();
            VanrednaRadnaVremenaLekara = new ObservableCollection<VremenskiInterval>();
            OsvezavanjePrikazaVanrednihRadnihDana();
        }

        private void OsvezavanjePrikazaVanrednihRadnihDana()
        {
            List<VremenskiInterval> vanrednaRadnaVremena = selektovaniLekar.RadnoVreme.VanrednaRadnaVremena;
            vanrednaRadnaVremena.Sort((u1, u2) => u1.Pocetak.CompareTo(u2.Pocetak));
            VanrednaRadnaVremenaLekara.Clear();
            foreach (VremenskiInterval datum in vanrednaRadnaVremena)
            {
                VanrednaRadnaVremenaLekara.Add(datum);
            }
        }

        private void PopuniComboBoxSati()
        {
            List<int> trajanja = new List<int>();
            for (int i = 0; i <= 24; i += 1)
            {
                trajanja.Add(i);
            }

            cbSatiPocetak.ItemsSource = trajanja.GetRange(0, 24);
            cbSatiTrajanje.ItemsSource = trajanja.GetRange(1, 24);

            cbSatiPocetak.SelectedIndex = 0;
            cbSatiTrajanje.SelectedIndex = 7;
        }

        private void PopuniComboBoxMinuta()
        {
            List<int> trajanja = new List<int>();
            for (int i = 0; i <= 60; i += 10)
            {
                trajanja.Add(i);
            }

            cbMinutiPocetak.ItemsSource = trajanja;
            cbMinutiPocetak.SelectedIndex = 0;
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool LekarNaGodisnjemOdmoru(VremenskiInterval vremenskiInterval)
        {
            List<DateTime> daniVanrednogRadnogVremena = new List<DateTime>();
            daniVanrednogRadnogVremena.Add(vremenskiInterval.Pocetak);
            daniVanrednogRadnogVremena.Add(vremenskiInterval.Kraj);

            return radnoVremeKontroler.PreklapanjeIntervalaGodisnjegOdmoraLekara(daniVanrednogRadnogVremena,
                selektovaniLekar.Jmbg);
        }

        private VremenskiInterval UcitajUneteParametre()
        {
            int satiPocetak = (int)cbSatiPocetak.SelectedValue;
            int minutiPocetka = (int)cbMinutiPocetak.SelectedValue;
            DateTime pocetakTemp = DateTime.Parse(datePicker_pocetak.SelectedDate.ToString());
            DateTime pocetak = new DateTime(pocetakTemp.Year, pocetakTemp.Month, pocetakTemp.Day, satiPocetak,
                minutiPocetka, 0, 0);

            int satiTrajanja = (int)cbSatiTrajanje.SelectedValue;
            DateTime kraj = pocetak.AddHours(satiTrajanja);

            return new VremenskiInterval(pocetak, kraj);
        }

        private void Button_Click_Dodaj(object sender, RoutedEventArgs e)
        {
            VremenskiInterval vanrednoRadnoVreme = UcitajUneteParametre();

            if (vanrednoRadnoVreme.Pocetak < DateTime.Now)
            {
                InformativniProzor ip = new InformativniProzor("Uneli ste datum u prošlosti.");
                ip.ShowDialog();
                return;
            }

            if (LekarNaGodisnjemOdmoru(vanrednoRadnoVreme))
            {
                InformativniProzor ip = new InformativniProzor("Lekar je na godišnjem odmoru za uneto vanredno radno vreme.");
                ip.ShowDialog();
                return;
            }

            if (selektovaniLekar.RadnoVreme.VecDodeljenoVanrednoRadnoVremeLekaru(vanrednoRadnoVreme))
            {
                InformativniProzor ip = new InformativniProzor("Lekaru je već dodeljeno vanredno radno vreme za uneti dan.");
                ip.ShowDialog();
                return;
            }

            selektovaniLekar.RadnoVreme.VanrednaRadnaVremena.Add(vanrednoRadnoVreme);
            radnoVremeKontroler.IzmeniRadnoVreme(selektovaniLekar.RadnoVreme);
            radnoVremeKontroler.OdloziSaObavestenjemTermineLekaraKojiSeNeUklapaju(vanrednoRadnoVreme, selektovaniLekar.Jmbg);

            OsvezavanjePrikazaVanrednihRadnihDana();
        }
    }
}
