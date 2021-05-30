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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IS_Bolnice.Prozori.UpravnikPages
{
    /// <summary>
    /// Interaction logic for AddSalePage.xaml
    /// </summary>
    public partial class AddSalePage : Page
    {
        BolnicaKontroler kontroler = new BolnicaKontroler();
        public AddSalePage()
        {
            InitializeComponent();
        }

        private void Dodaj_btn_Click(object sender, RoutedEventArgs e)
        {
            Soba novaSoba = new Soba();
            novaSoba.Id = id_txt.Text;
            novaSoba.Tip = (RoomType)tip_sobe_txt.SelectedIndex;
            novaSoba.Kvadratura = double.Parse(kvadratura_txt.Text);
            novaSoba.Sprat = int.Parse(sprat_txt.Text);
            kontroler.KreirajSobuUBolnici(novaSoba);
            
            
            Page sale = new SalePage();
            this.NavigationService.Navigate(sale);
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            Page sale = new SalePage();
            this.NavigationService.Navigate(sale);
        }
    }
}
