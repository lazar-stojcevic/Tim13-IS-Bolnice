using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
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
using IS_Bolnice.ViewModel.Lekar;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarRaspored.xaml
    /// </summary>
    public partial class LekarRaspored : Page
    {
        private string sifra;
        public LekarRaspored(string id)
        {
            InitializeComponent();
            sifra = id;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            this.DataContext = new RasporedLekaraViewModel(this.NavigationService, sifra);
        }

    }

}
