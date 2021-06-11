using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Model;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;

namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for PrikazZauzetostiLekaraOpstePrakse.xaml
    /// </summary>
    public partial class PrikazZauzetostiLekaraOpstePrakse : Window
    {
        private PregledKontroler pregledKontroler = new PregledKontroler();
        private Lekar selektovaniLekar;
        public ObservableCollection<Pregled> PreglediSelektovanogLekara
        {
            get;
            set;
        }
        public PrikazZauzetostiLekaraOpstePrakse(Lekar selektovaniLekar)
        {
            InitializeComponent();
            this.DataContext = this;

            this.selektovaniLekar = selektovaniLekar;
            txtLekar.Text = selektovaniLekar.Ime + " " + selektovaniLekar.Prezime;
            PreglediSelektovanogLekara =
                new ObservableCollection<Pregled>(pregledKontroler.GetSviBuduciPreglediLekara(selektovaniLekar.Jmbg));
        }
        private bool ValidnoUnesenDatum(DateTime pocetak, DateTime kraj)
        {
            return pocetak <= kraj;
        }

        private void Button_Click_Generisanje_PDF_Izvestaja(object sender, RoutedEventArgs e)
        {
            DateTime pocetak = DateTime.Parse(datePicker_pocetak.SelectedDate.ToString());
            DateTime kraj = DateTime.Parse(datePicker_kraj.SelectedDate.ToString());
            if (!ValidnoUnesenDatum(pocetak, kraj))
            {
                InformativniProzor ip = new InformativniProzor("Nevalidno unesen period za formranje izveštaja");
                ip.ShowDialog();
                return;
            }

            VremenskiInterval intervalZaIzvestaj = new VremenskiInterval(pocetak, kraj);
            GenerisanjeIzvestajaZaLekaraKontroler generisanje = new GenerisanjeIzvestajaZaLekaraKontroler();
            bool uspesno = generisanje.GenerisiIzvestajZauzetostiLekara(selektovaniLekar, intervalZaIzvestaj);

            if (uspesno)
            {
                InformativniProzor ip = new InformativniProzor("Uspešno generisan izveštaj!");
                ip.ShowDialog();
            }
            else
            {
                InformativniProzor ip = new InformativniProzor("Zatvorite fajl \"izvestajZaLekaraOpstePrakse\" da bi mogao ponovo da se generiše.");
                ip.ShowDialog();
            }
        }
    }
}
