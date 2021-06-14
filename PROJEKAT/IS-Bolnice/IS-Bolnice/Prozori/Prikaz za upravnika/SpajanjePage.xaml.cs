using IS_Bolnice.Repozitorijumi;
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
using System.Text.RegularExpressions;
using IS_Bolnice.Kontroleri.Ustanova;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for SpajanjePage.xaml
    /// </summary>
    public partial class SpajanjePage : Page
    {
        List<Soba> selectovaneSobe;
        Soba novaSoba;

        RenovacijaKontroler kontroler = new RenovacijaKontroler();

        public SpajanjePage(List<Soba> sobeZaSpajanje)
        {
            InitializeComponent();
            selectovaneSobe = sobeZaSpajanje;
            novaSoba = new Soba();
            novaSoba.Kvadratura = 0;
        }

        private void Potvrdi_btn_Click(object sender, RoutedEventArgs e)
        {
            if (Validiraj())
            {
                novaSoba.Id = id_txt.Text;
                novaSoba.Tip = (RoomType)tip_sobe_txt.SelectedIndex;
                if (CheckDatum(MakeRenovacija()))
                {
                    if (kontroler.SpajanjeSobe(selectovaneSobe, MakeRenovacija()))
                    {
                        Page renoviranje = new RenoviranjeSpajanjePage();
                        this.NavigationService.Navigate(renoviranje);
                    }
                }
                else
                {
                    CustomMessageBox.ShowOK("Datumi se logički ne poklapaju", "Greška", "Potvrdi");
                }
            }
            else {
                CustomMessageBox.ShowOK("Ne sme se koristiti # ili /", "Greška", "Potvrdi");
            }
        }
        private bool Validiraj()
        {
            Regex regex = new Regex("^[#]+");
            if (regex.IsMatch(id_txt.Text) || id_txt.Text.Contains("/"))
            {
                return false;
            }

            return true;
        }


        private Renovacija MakeRenovacija()
        {
            Renovacija novaRenovacija = new Renovacija();
            DateTime izbraniDatumZaPocetak = DateTime.Parse(datePicker_pocetak.SelectedDate.ToString());
            DateTime izbraniDatumZaKraj = DateTime.Parse(datePicker_kraj.SelectedDate.ToString());
            novaRenovacija.DatumPocetka = new DateTime(izbraniDatumZaPocetak.Year, izbraniDatumZaPocetak.Month, izbraniDatumZaPocetak.Day);
            novaRenovacija.DatumKraja = new DateTime(izbraniDatumZaKraj.Year, izbraniDatumZaKraj.Month, izbraniDatumZaKraj.Day);
            novaRenovacija.ProstorijaZaRenoviranje = novaSoba;
            return novaRenovacija;
        }

        public bool CheckDatum(Renovacija novaRenovacija)
        {
            bool ispravno = true;
            if (novaRenovacija.DatumPocetka > novaRenovacija.DatumKraja)
            {
                ispravno = false;
            }
            return ispravno;
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
