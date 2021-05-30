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

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for PreraspodelaStatickeOpremeWindow.xaml
    /// </summary>
    public partial class PreraspodelaStatickeOpremeWindow : Window
    {
        SadrzajSobe sadrzajNaCekanju;
        public PreraspodelaStatickeOpremeWindow(SadrzajSobe sadrzajSobe)
        {
            InitializeComponent();
            sadrzajNaCekanju = sadrzajSobe;
        }

        private void Potvrdi_btn_Click(object sender, RoutedEventArgs e)
        {
            DateTime izbraniDatum = DateTime.Parse(datePicker_pocetak.SelectedDate.ToString());
            sadrzajNaCekanju.DatumPremestanja = new DateTime(izbraniDatum.Year, izbraniDatum.Month, izbraniDatum.Day);
            SadrzajSobeFajlRepozitorijum sadrzajSobeFajlRepozitorijum = new SadrzajSobeFajlRepozitorijum();
            sadrzajSobeFajlRepozitorijum.Sacuvaj(sadrzajNaCekanju);
            this.Close();
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
