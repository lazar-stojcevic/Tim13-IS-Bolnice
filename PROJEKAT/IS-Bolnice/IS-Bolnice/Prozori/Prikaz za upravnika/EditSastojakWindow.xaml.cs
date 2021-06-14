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
using IS_Bolnice.Repozitorijumi.Interfejsi;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Kontroleri.Lekovi;

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for EditSastojakWindow.xaml
    /// </summary>
    public partial class EditSastojakWindow : Window
    {
        string ime;
        private SastojakKontroler kontroler = new SastojakKontroler();

        public EditSastojakWindow(string name)
        {
            InitializeComponent();
            ime = name;
            id_txt.Text = name;
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Izmeni_btn_Click(object sender, RoutedEventArgs e)
        {
            kontroler.ObrisiSastojak(ime);
            Sastojak noviSastojak = new Sastojak(id_txt.Text);
            kontroler.KreirajNoviSastojak(noviSastojak);
        }
    }
}
