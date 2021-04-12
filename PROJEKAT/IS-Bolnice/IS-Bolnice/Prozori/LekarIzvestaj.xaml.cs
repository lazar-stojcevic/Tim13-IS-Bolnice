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
    /// Interaction logic for LekarIzvestaj.xaml
    /// </summary>
    public partial class LekarIzvestaj : Window
    {

        ObservableCollection<Terapija> terapije = new ObservableCollection<Terapija>();
        string jmbgPac;
        string jmbgLek;
        public LekarIzvestaj(string jmbgPacijenta, string jmbgLekara)
        {
            InitializeComponent();
            listaLekova.ItemsSource = terapije;
            jmbgPac = jmbgPacijenta;
            jmbgLek = jmbgLekara;
        }

        private void Button_DodajLek(object sender, RoutedEventArgs e)
        {
            LekarDodavanjeLeka prozor = new LekarDodavanjeLeka(terapije, jmbgPac);
            prozor.Show();

            
        }

        private void Button_ObrisiLek(object sender, RoutedEventArgs e)
        {
            terapije.Remove((Terapija)listaLekova.SelectedItem);
        }

        private void Button_ZavrsiPregled(object sender, RoutedEventArgs e)
        {
            //Zapisivanje izvestaja u txt datoteku lekar#pacijent#anamneza#datum#Lekovi sa terapijom
            string textIzvestaja = txtAnamneza.Text.Replace("\n", "%%%"); //Nisam siguran da li treba ova linija
            textIzvestaja =jmbgLek +"#"+ jmbgPac + "#" + textIzvestaja + "#" + DateTime.Now.Date + "#";
            foreach (Terapija ter in terapije)
            {
                textIzvestaja = textIzvestaja + ter.Lek.Sifra + "$$" + ter.Lek.Ime + "$$" + ter.Lek.Opis + "$$" 
                    + ter.UcestanostKonzumiranja.ToString() + "$$" + ter.VremePocetka.ToString() + "$$" + ter.VremeKraja.ToString()+ "$$" +ter.Detalji + "$$$"; 
            }
            BazaIzvestaja baza = new BazaIzvestaja();
            textIzvestaja = textIzvestaja + System.Environment.NewLine;
            baza.KreirajIzvestaj(textIzvestaja);
            this.Close();

        }
    }
}
