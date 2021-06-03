using IS_Bolnice.Kontroleri;
using IS_Bolnice.ViewModel.Upravnik;
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

namespace IS_Bolnice.Prozori.UpravnikPages
{
    /// <summary>
    /// Interaction logic for AddSalePage.xaml
    /// </summary>
    public partial class AddSalePage : Page
    {
        public AddSalePage()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            this.DataContext = new AddSaluPageViewModel(this.NavigationService);
        }


    }
}
