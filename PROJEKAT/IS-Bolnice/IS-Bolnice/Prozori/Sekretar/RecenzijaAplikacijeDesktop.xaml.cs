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
using IS_Bolnice.Model;
using IS_Bolnice.Servisi;
using IS_Bolnice.ViewModel.Sekretar;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for RecenzijaAplikacijeDesktop.xaml
    /// </summary>
    public partial class RecenzijaAplikacijeDesktop : Window
    {
        public RecenzijaAplikacijeDesktop()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            this.DataContext = new RecenzijaAplikacijeDesktopViewModel(this);
        }
    }
}
