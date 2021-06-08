using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for AddLekPage.xaml
    /// </summary>
    public partial class AddLekPage : Page
    {
        public AddLekPage()
        {
            InitializeComponent();

        }

        

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            Page lekovi = new LekoviPage();
            this.NavigationService.Navigate(lekovi);
        }

        private void Potvrdi_btn_Click(object sender, RoutedEventArgs e)
        {
            bool potrebanRecept;
            if (potreban_recept_txt.SelectedIndex == 0) {
                potrebanRecept = true;
            }
            else {
                potrebanRecept = false;
            }
            if (Validiraj())
            {
                Lek lek = new Lek(id_txt.Text, naziv_txt.Text, opis_txt.Text, potrebanRecept);
                Page addLekNext = new AddLekPage2(lek);
                this.NavigationService.Navigate(addLekNext);
            }
            else {
                CustomMessageBox.ShowOK("Podaci nisu validno uneti! Ne sme biti #!", "Greška", "Potvrdi");
            }

        }

        private bool Validiraj()
        {
            if (id_txt.Text.Contains("#")) {
                return false;
            }
            if (naziv_txt.Text.Contains("#")) {
                return false;
            }
            if (opis_txt.Text.Contains("#"))
            {
                return false;
            }
            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

    }
}
