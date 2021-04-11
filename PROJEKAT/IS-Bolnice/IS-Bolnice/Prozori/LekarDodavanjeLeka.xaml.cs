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

namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for LekarDodavanjeLeka.xaml
    /// </summary>
    public partial class LekarDodavanjeLeka : Window
    {
        ObservableCollection<Terapija> pomocna = new ObservableCollection<Terapija>();
        public LekarDodavanjeLeka(ObservableCollection<Terapija> terapija)
        {
            BazaLekova bazaLekova = new BazaLekova();
            List<Lek> lekovi = bazaLekova.SviLekovi();
            InitializeComponent();
            listaSvihLekova.ItemsSource = lekovi;
            pomocna = terapija;

        }

        private void Button_DodajClick(object sender, RoutedEventArgs e)
        {
            Terapija t = new Terapija();
            Lek l = (Lek)listaSvihLekova.SelectedItem;
            t.Lek = l;
            t.UcestanostKonzumiranja = Double.Parse(txtBrojUzimanja.Text);
            t.VremePocetka = System.DateTime.Now;
            t.VremeKraja = DateTime.Now.AddDays(Int16.Parse(txtTrajanje.Text));
            pomocna.Add(t);

        }
    }
}
