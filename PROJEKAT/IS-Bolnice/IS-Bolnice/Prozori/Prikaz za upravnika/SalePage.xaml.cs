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
        public SalePage()
        {
            InitializeComponent();
            BolnicaFajlRepozitorijum bolnicaFajlRepozitorijum = new BolnicaFajlRepozitorijum();
            listBox.ItemsSource = ParseSobaToString(bolnicaFajlRepozitorijum.GetSobe());
        }

        private List<string> ParseSobaToString(List<Soba> sobe) {

            List<string> tekst = new List<string>();
            foreach (Soba s in sobe)
            {

                if (s.Obrisano == false && (SveProstorijeSuSelektovanje() || s.Tip == SelektovaniTipProstorije()))
                {
                    tekst.Add("ID: " + s.Id + " Sprat: " + s.Sprat.ToString() + " Tip: " + s.Tip);

                }
            }
            return tekst;

           
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
            BolnicaFajlRepozitorijum baza = new BolnicaFajlRepozitorijum();
            listBox.ItemsSource = ParseSobaToString(baza.GetSobe());
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] niz = listBox.SelectedItem.ToString().Split(' ');
            Page editSale = new EditSalePage(niz[1]);
            this.NavigationService.Navigate(editSale);
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Page addSale = new AddSalePage();
            this.NavigationService.Navigate(addSale);
        }
    }
}
