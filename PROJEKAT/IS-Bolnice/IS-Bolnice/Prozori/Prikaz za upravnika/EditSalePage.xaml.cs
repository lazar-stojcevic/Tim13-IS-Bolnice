using IS_Bolnice.Kontroleri;
using IS_Bolnice.Prozori.Prikaz_za_upravnika;
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
    /// Interaction logic for EditSalePage.xaml
    /// </summary>
    public partial class EditSalePage : Page
    {
        BolnicaKontroler kontroler = new BolnicaKontroler();

        Soba selectedSoba;
        public EditSalePage(string selectedId)
        {
            InitializeComponent();
            
            selectedSoba = kontroler.GetSobaPoId(selectedId);
            this.id_txt.Text = selectedSoba.Id;
            this.kvadratura_txt.Text = selectedSoba.Kvadratura.ToString();
            this.sprat_txt.Text = selectedSoba.Sprat.ToString();
            PostaviTip();

        }

        private void PostaviTip()
        {
            switch (selectedSoba.Tip)
            {
                case RoomType.bolnickaSoba:
                    this.tip_sobe_txt.SelectedIndex = 1;
                    break;
                case RoomType.operacionaSala:
                    this.tip_sobe_txt.SelectedIndex = 0;
                    break;
                case RoomType.magacin:
                    this.tip_sobe_txt.SelectedIndex = 3;
                    break;
                default:
                    this.tip_sobe_txt.SelectedIndex = 2;
                    break;
            }
        }

        private RoomType DobaviTip() {

            switch (tip_sobe_txt.SelectedIndex) {
                case 0:
                    return RoomType.operacionaSala;
                case 1:
                    return RoomType.bolnickaSoba;
                case 2:
                    return RoomType.ordinacija;
                default:
                    return RoomType.magacin;
            }
        }

        private void Izmeni_btn_Click(object sender, RoutedEventArgs e)
        {
            Soba izmenjenaSoba = new Soba(id_txt.Text);
            izmenjenaSoba.Kvadratura =Int32.Parse(kvadratura_txt.Text);
            izmenjenaSoba.Sprat = Int32.Parse(sprat_txt.Text);
            izmenjenaSoba.Tip = DobaviTip();
            kontroler.IzmeniSobu(izmenjenaSoba);
            Page sale = new SalePage();
            this.NavigationService.Navigate(sale);
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            Page sale = new SalePage();
            this.NavigationService.Navigate(sale);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
                MessageBoxResult resultat = MessageBox.Show("Da li ste sigurni da zelite da obrisete sobu?", "", MessageBoxButton.YesNo);
                if (resultat == MessageBoxResult.Yes)
                {
                    selectedSoba.Obrisano = true;
                    kontroler.IzmeniSobu(selectedSoba);
                    Page sale = new SalePage();
                    this.NavigationService.Navigate(sale);
                }

            
        }

        private void RenoviranjeButton_Click(object sender, RoutedEventArgs e)
        {
            Page renoviranje = new RenoviranjePage(selectedSoba);
            this.NavigationService.Navigate(renoviranje);
        }
    }
}
