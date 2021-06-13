using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using IS_Bolnice.Kontroleri.GenerisanjeIzvestaja;
using IS_Bolnice.Kontroleri.Termini;
using IS_Bolnice.Model;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for PrikazZauzetostiLekaraSpecijalista.xaml
    /// </summary>
    public partial class PrikazZauzetostiLekaraSpecijalista : Window
    {
        private PregledKontroler pregledKontroler = new PregledKontroler();
        private OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
        private Lekar selektovaniLekar;
        public ObservableCollection<Pregled> PreglediSelektovanogLekara
        {
            get;
            set;
        }

        public ObservableCollection<Operacija> OperacijeSelektovanogLekara
        {
            get;
            set;
        }
        public PrikazZauzetostiLekaraSpecijalista(Lekar selektovaniLekar)
        {
            InitializeComponent();
            this.DataContext = this;

            this.selektovaniLekar = selektovaniLekar;
            txtLekar.Text = selektovaniLekar.Ime + " " + selektovaniLekar.Prezime;

            PreglediSelektovanogLekara =
                new ObservableCollection<Pregled>(pregledKontroler.GetSviBuduciPreglediLekara(selektovaniLekar.Jmbg));
            OperacijeSelektovanogLekara =
                new ObservableCollection<Operacija>(
                    operacijaKontroler.GetSveBuduceOperacijeLekara(selektovaniLekar.Jmbg));
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
                InformativniProzor ip = new InformativniProzor("Zatvorite fajl \"izvestajZaLekaraSpecijalistu\" da bi mogao ponovo da se generiše.");
                ip.ShowDialog();
            }
        }
    }
}
