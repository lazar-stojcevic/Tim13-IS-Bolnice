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
using IS_Bolnice.ViewModel.Lekar;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarGlavniMeni.xaml
    /// </summary>
    public partial class LekarGlavniMeni : Page
    {
        public string Sifra { get; set; }
        public LekarGlavniMeni(string jmbgLekara)
        {
            Sifra = jmbgLekara;
            InitializeComponent();
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            this.DataContext = new GlavniMeniViewModel(this.NavigationService, Sifra);
        }

    }

}
