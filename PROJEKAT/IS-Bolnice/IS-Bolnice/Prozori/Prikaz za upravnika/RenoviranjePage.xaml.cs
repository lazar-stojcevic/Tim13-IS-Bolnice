using IS_Bolnice.Baze;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using IS_Bolnice.Kontroleri;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for RenoviranjePage.xaml
    /// </summary>
    public partial class RenoviranjePage : Page
    {
        Soba selektovanaSoba;

        RenovacijaKontroler renovacijaKontroler = new RenovacijaKontroler();


        public RenoviranjePage(Soba soba)
        {
            InitializeComponent();
            selektovanaSoba = soba;
        }

        private void Potvrdi_btn_Click(object sender, RoutedEventArgs e)
        {
            bool uspesno = false;
            if (CheckDatum(MakeRenovacija()))
            {
                if (selektovanaSoba.Tip == RoomType.operacionaSala)
                {
                   uspesno = renovacijaKontroler.RenoviranjeOperacioneSale(MakeRenovacija());
                }
                else if (selektovanaSoba.Tip == RoomType.bolnickaSoba) {
                    uspesno = renovacijaKontroler.RenovirajBolnickuSobu(MakeRenovacija());
                }
                else {
                    uspesno = renovacijaKontroler.RenoviranjeProstorije(MakeRenovacija());
                }
                if (uspesno) {
                    renovacijaKontroler.RenovirajOpremu(comboBox_oprema.SelectedIndex, selektovanaSoba.Id);
                    this.NavigationService.GoBack();
                    CustomMessageBox.ShowOK("Renoviranje je uspešno zakazano!", "Upseh", "Potvrdi");
                }
                else
                {
                    CustomMessageBox.ShowOK("Zakazani termini postoje! Odaberite drugi datum!", "Greska", "Potvrdi");
                }
            }
            else {
                MessageBox.Show("Datumi se ne poklapaju logicki");
            }
        }

        public Renovacija MakeRenovacija() {
            Renovacija novaRenovacija = new Renovacija();
            DateTime izbraniDatumZaPocetak = DateTime.Parse(datePicker_pocetak.SelectedDate.ToString());
            DateTime izbraniDatumZaKraj = DateTime.Parse(datePicker_kraj.SelectedDate.ToString());
            novaRenovacija.DatumPocetka = new DateTime(izbraniDatumZaPocetak.Year, izbraniDatumZaPocetak.Month, izbraniDatumZaPocetak.Day);
            novaRenovacija.DatumKraja = new DateTime(izbraniDatumZaKraj.Year, izbraniDatumZaKraj.Month, izbraniDatumZaKraj.Day);
            novaRenovacija.ProstorijaZaRenoviranje = selektovanaSoba;
            return novaRenovacija;
        }

        public bool CheckDatum(Renovacija novaRenovacija) {
            bool ispravno = true;
            if (novaRenovacija.DatumPocetka > novaRenovacija.DatumKraja) {
                ispravno = false;
            }
            return ispravno;
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Podeli_btn_Click(object sender, RoutedEventArgs e)
        {
            Page razdvajanjePage = new RazdvajanjeSobePage(MakeRenovacija());
            this.NavigationService.Navigate(razdvajanjePage);
        }
    }
}
