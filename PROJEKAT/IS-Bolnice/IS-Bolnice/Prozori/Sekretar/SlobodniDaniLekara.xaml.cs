using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using IS_Bolnice.Baze;

namespace IS_Bolnice.Prozori.Sekretar
{
    public partial class SlobodniDaniLekara : Window
    {
        private Lekar selektovaniLekar;
        private BazaRadnogVremena bazaRadnogVremena = new BazaRadnogVremena();

        public ObservableCollection<DateTime> PrikazSlobodnihDanaLekara
        {
            get;
            set;
        }

        public SlobodniDaniLekara(Lekar lekar)
        {
            InitializeComponent();
            this.DataContext = this;

            selektovaniLekar = lekar;
            tbLekar.Text = lekar.Ime + " " + lekar.Prezime;

            PrikazSlobodnihDanaLekara = new ObservableCollection<DateTime>();
            OsvezavanjePrikazaSlobodnihDana();
        }

        private void OsvezavanjePrikazaSlobodnihDana()
        {
            List<DateTime> slobodniDani = selektovaniLekar.RadnoVreme.SlobodniDani;
            slobodniDani.Sort();
            PrikazSlobodnihDanaLekara.Clear();
            foreach (DateTime datum in slobodniDani)
            {
                PrikazSlobodnihDanaLekara.Add(datum);
            }
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool Validno(DateTime pocetak, DateTime kraj)
        {
            return (pocetak <= kraj) && pocetak > DateTime.Now;
        }

        private void Button_Click_Kreiraj_Godisnji(object sender, RoutedEventArgs e)
        {
            DateTime pocetak = DateTime.Parse(datePicker_pocetak.SelectedDate.ToString());
            DateTime kraj = DateTime.Parse(datePicker_kraj.SelectedDate.ToString());
            if (!Validno(pocetak, kraj))
            {
                MessageBox.Show("Nevalidan unos datuma godišnjeg odmora.");
                return;
            }
            // TODO: implementirati analizu za slobodne dane i belezenje ako je to moguce i vracanje poruke
            TimeSpan intervalGodisnjegOdmora = kraj.Subtract(pocetak);
            int daniGodisnjegOdmora = intervalGodisnjegOdmora.Days;
            for (int i = 0; i <= daniGodisnjegOdmora; i++)
            {
                selektovaniLekar.RadnoVreme.SlobodniDani.Add(pocetak.AddDays(i));
            }

            bazaRadnogVremena.IzmeniRadnoVreme(selektovaniLekar.RadnoVreme);
            MessageBox.Show("Uspešno kreiran godišnji odmor!");
            OsvezavanjePrikazaSlobodnihDana();
        }
    }
}
