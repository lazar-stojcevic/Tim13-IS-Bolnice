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
    /// Interaction logic for CenovnikPage.xaml
    /// </summary>
    public partial class CenovnikPage : Page
    {
        List<string> sourse = new List<string>();
        public CenovnikPage()
        {
            InitializeComponent();
            string tekst = "Usluga: Pregled Cena: 1000";
            string tekst1 = "Usluga: Operacija Cena: 10000";
            string tekst2 = "Usluga: Krvna slika Cena: 800";
            sourse.Add(tekst);
            sourse.Add(tekst1);
            sourse.Add(tekst2);
            listBox.ItemsSource = sourse;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
