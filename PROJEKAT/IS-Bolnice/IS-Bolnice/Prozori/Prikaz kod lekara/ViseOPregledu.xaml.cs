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
    /// Interaction logic for ViseOPregledu.xaml
    /// </summary>
    public partial class ViseOPregledu : Page
    {
        public ViseOPregledu()
        {
            InitializeComponent();
            jmbgTxt.Text = "1234567890123";
            imeTxt.Text = "Marko";
            przTxt.Text = "Markovic";
            vremePocetkaTxt.Text = "13/12/2020 10:30";
            vremeKrajaTxt.Text = "13/12/2020 11: 00";
            oblastLekaraTxt.Text = "Kardiolog";

            anamnezaTxt.Text =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque sagittis," +
                " sem vitae accumsan posuere, quam lectus convallis nunc, eget rhoncus erat lacus in elit.";

            List<Lek> lekovi = new List<Lek>();
            Lek lek1 = new Lek("Lek1", "500mg");
            Lek lek2 = new Lek("Lek2", "100mg");
            Lek lek3 = new Lek("Lek3", "200mg");

            lekovi.Add(lek1);
            lekovi.Add(lek2);
            lekovi.Add(lek3);

            lekoviList.ItemsSource = lekovi;


        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
