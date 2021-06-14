using IS_Bolnice.Kontroleri;
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
using IS_Bolnice.Kontroleri.Lekovi;

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for AddSastojakWindow.xaml
    /// </summary>
    public partial class AddSastojakWindow : Window
    {
        SastojakKontroler sastojakKontroler = new SastojakKontroler();
        public AddSastojakWindow()
        {
            InitializeComponent();
        }

        private void Dodaj_btn_Click(object sender, RoutedEventArgs e)
        { 
            Sastojak sastojak = new Sastojak(id_txt.Text);
            sastojakKontroler.KreirajNoviSastojak(sastojak);
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
