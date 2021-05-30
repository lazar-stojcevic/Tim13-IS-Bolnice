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
    /// Interaction logic for RenoviranjeSpajanjePage.xaml
    /// </summary>
    public partial class RenoviranjeSpajanjePage : Page
    {

        bool spajanje;
        public RenoviranjeSpajanjePage()
        {
            InitializeComponent();
            BolnicaFajlRepozitorijum bolnicaFajlRepozitorijum = new BolnicaFajlRepozitorijum();
            listBox.ItemsSource = ParseSobaToString(bolnicaFajlRepozitorijum.GetSobe());
            spajanje = false; 
        }

        private List<string> ParseSobaToString(List<Soba> sobe)
        {

            List<string> tekst = new List<string>();
            foreach (Soba s in sobe)
            {

                if (s.Obrisano == false)
                {
                    tekst.Add("ID: " + s.Id + " Sprat: " + s.Sprat.ToString() + " Tip: " + s.Tip);

                }
            }
            return tekst;


        }

        private Soba ParseStringToSoba(string redTebele) {
            string[] niz = redTebele.Split(' ');
            BolnicaFajlRepozitorijum bolnicaFajlRepozitorijum = new BolnicaFajlRepozitorijum();
            return bolnicaFajlRepozitorijum.GetSobaById(niz[1]);
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!spajanje)
            {
                Page renoviranje = new RenoviranjePage(ParseStringToSoba(listBox.SelectedItem.ToString()));
                this.NavigationService.Navigate(renoviranje);
            }
        }

        private void Spoji_btn_Click(object sender, RoutedEventArgs e)
        { 

            List<Soba> sobe = new List<Soba>();
            foreach (string redTabele in listBox.SelectedItems)
            {
                sobe.Add(ParseStringToSoba(redTabele));
            }
            Page spajanje = new SpajanjePage(sobe);
            this.NavigationService.Navigate(spajanje);
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            spajanje = !spajanje;
            if (spajanje)
            {
                listBox.SelectionMode = SelectionMode.Multiple;
            }
            else {
                listBox.UnselectAll();
                listBox.SelectionMode = SelectionMode.Single;
            }
        }
    }
}
