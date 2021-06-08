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
using IS_Bolnice.Model;
using IS_Bolnice.Servisi;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for RecenzijaAplikacije.xaml
    /// </summary>
    public partial class RecenzijaAplikacije : Page
    {
        public RecenzijaAplikacije()
        {
            InitializeComponent();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Click_PosaljiRecenziju(object sender, RoutedEventArgs e)
        {
            try
            {
                Recenzija recenzija = new Recenzija();
                recenzija.Ocena = Int16.Parse(ocena.Text);
                recenzija.Opis = opis.Text;
                new RecenzijaServis().KreirajRecenziju(recenzija);
                CustomMessageBox.ShowOK("Uspešno ste poslali recenziju!", "Poslata recenzija", "Dobro", MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch
            {
                CustomMessageBox.ShowOK("Niste selektovali ocenu!", "Greška", "Dobro", MessageBoxImage.Error);
            }
        }
    }
}
