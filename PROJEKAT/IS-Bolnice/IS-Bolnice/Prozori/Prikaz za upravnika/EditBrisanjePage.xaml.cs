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

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for EditBrisanjePage.xaml
    /// </summary>
    public partial class EditBrisanjePage : Page
    {
        Lek izabraniLek;

        LekKontroler lekKontroler = new LekKontroler();

        public EditBrisanjePage(Lek lek)
        {
            InitializeComponent();
            izabraniLek = lek;
        }

        private void DeleteAddButton_Click(object sender, RoutedEventArgs e)
        {
            lekKontroler.ObrisiLek(izabraniLek);
            Page lekovi = new LekoviPage();
            this.NavigationService.Navigate(lekovi);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditLekPage1 editLek = new EditLekPage1(izabraniLek, true);
            this.NavigationService.Navigate(editLek);
        }
    }
}
