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

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for ViseOOperaciji.xaml
    /// </summary>
    public partial class ViseOOperaciji : Page
    {
        public ViseOOperaciji()
        {
            InitializeComponent();
            jmbgTxt.Text = "1234567890123";
            imeTxt.Text = "Marko";
            przTxt.Text = "Markovic";
            vremePocetkaTxt.Text = "13/12/2020 10:30";
            vremeKrajaTxt.Text = "13/12/2020 11: 00";
            oblastLekaraTxt.Text = "Kardiolog";
            salaTxt.Text = "Operaciona sala: 333";
            izvestajTxt.Text =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent rutrum ipsum scelerisque," +
                " ultricies justo sit amet, imperdiet dolor." +
                " Pellentesque imperdiet erat orci, id volutpat elit feugiat sit amet. Nam ultricies dolor ac finibus gravida.";
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
