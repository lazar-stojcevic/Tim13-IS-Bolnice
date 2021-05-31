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
    /// Interaction logic for SalePage.xaml
    /// </summary>
    public partial class SalePage : Page
    {
        private List<Soba> sveSobeZaPrikaz = new List<Soba>();
        private BolnicaKontroler kontroler = new BolnicaKontroler();
        public SalePage()
        {
            InitializeComponent();
            List<Soba> sveSobe = kontroler.GetSveSobe();
            foreach (Soba iterSoba in sveSobe) {
                if (iterSoba.Obrisano == false) {
                    sveSobeZaPrikaz.Add(iterSoba);
                }
            }
            listBox.ItemsSource = sveSobeZaPrikaz;
        }

        private bool SveProstorijeSuSelektovanje() {
            switch (tip_sale_txt.SelectedIndex)
            {
                case 0:
                    return true;
               
                default:
                    return false;
                   
            }
        }

        private RoomType SelektovaniTipProstorije() {
            switch (tip_sale_txt.SelectedIndex)
            {
                case 1:
                    return RoomType.operacionaSala;
                case 2:
                    return RoomType.bolnickaSoba;
                case 3:
                    return RoomType.ordinacija;
                default:
                    return RoomType.magacin;
            }
        }

        private void tip_opreme_txt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Soba> selektovaneSobe = new List<Soba>();
            foreach (Soba s in sveSobeZaPrikaz)
            {

                if (s.Obrisano == false && (SveProstorijeSuSelektovanje() || s.Tip == SelektovaniTipProstorije()))
                {
                    selektovaneSobe.Add(s);
                    
                }
            }
            listBox.ItemsSource = selektovaneSobe;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Soba selectovanaSoba = (Soba)listBox.SelectedItem;
            Page editSale = new EditSalePage(selectovanaSoba.Id);
            this.NavigationService.Navigate(editSale);
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Page addSale = new AddSalePage();
            this.NavigationService.Navigate(addSale);
        }
    }
}
