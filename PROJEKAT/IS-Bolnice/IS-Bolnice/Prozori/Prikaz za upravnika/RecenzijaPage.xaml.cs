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
using WPFCustomMessageBox;
using IS_Bolnice.Model;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Kontroleri.Informativni;

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for RecenzijaPage.xaml
    /// </summary>
    public partial class RecenzijaPage : Page
    {
        RecenzijaKontroler recenzijaKontroler = new RecenzijaKontroler();
        int ocena;
        public RecenzijaPage()
        {
            InitializeComponent();
            ocena = 0;
        }

        private void Potvrdi_btn_Click(object sender, RoutedEventArgs e)
        {
            if (ocena == 0)
            {
                CustomMessageBox.ShowOK("Ocena nije odabrana!", "Greška", "Potvrdi");
            }
            else {
                if (opis_txt.Text.Contains("#")) {
                    CustomMessageBox.ShowOK("U komentaru ne sme biti znaka #", "Greška", "Potvrdi");
                }
                else
                {
                    Recenzija novaRecenzija = new Recenzija();
                    novaRecenzija.Ocena = ocena;
                    novaRecenzija.Opis = opis_txt.Text;
                    recenzijaKontroler.KreirajRecenziju(novaRecenzija);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void ocena5_Checked(object sender, RoutedEventArgs e)
        {
            ocena = 5;
            ocena1.IsChecked = false;
            ocena2.IsChecked = false;
            ocena3.IsChecked = false;
            ocena4.IsChecked = false;
        }

        private void ocena4_Checked(object sender, RoutedEventArgs e)
        {
            ocena = 4;
            ocena1.IsChecked = false;
            ocena2.IsChecked = false;
            ocena3.IsChecked = false;
            ocena5.IsChecked = false;
        }

        private void ocena3_Checked(object sender, RoutedEventArgs e)
        {
            ocena = 3;
            ocena1.IsChecked = false;
            ocena2.IsChecked = false;
            ocena4.IsChecked = false;
            ocena5.IsChecked = false;
        }

        private void ocena2_Checked(object sender, RoutedEventArgs e)
        {
            ocena = 2;
            ocena1.IsChecked = false;
            ocena3.IsChecked = false;
            ocena4.IsChecked = false;
            ocena5.IsChecked = false;

        }

        private void ocena1_Checked(object sender, RoutedEventArgs e)
        {
            ocena = 1;
            ocena2.IsChecked = false;
            ocena3.IsChecked = false;
            ocena4.IsChecked = false;
            ocena5.IsChecked = false;
        }
    }
}
