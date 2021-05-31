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
    /// Interaction logic for RenoviranjeSpajanjePage.xaml
    /// </summary>
    public partial class RenoviranjeSpajanjePage : Page
    {
        BolnicaKontroler bolnicaKontroler = new BolnicaKontroler();
        List<Soba> sveSobe = new List<Soba>();
        bool spajanje;
        public RenoviranjeSpajanjePage()
        {
            InitializeComponent();
            foreach (Soba iterSoba in bolnicaKontroler.GetSveSobe()) {
                if (iterSoba.Obrisano == false)
                {
                    sveSobe.Add(iterSoba);
                }
            }
            listBox.ItemsSource = sveSobe;
            spajanje = false; 
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!spajanje)
            {
                Page renoviranje = new RenoviranjePage((Soba)listBox.SelectedItem);
                this.NavigationService.Navigate(renoviranje);
            }
        }

        private void Spoji_btn_Click(object sender, RoutedEventArgs e)
        { 
            List<Soba> sobe = new List<Soba>();
            foreach (Soba redTabele in listBox.SelectedItems)
            {
                    sobe.Add(redTabele);
            }
            if (CheckSpratSobe(sobe))
            {
                Page spajanje = new SpajanjePage(sobe);
                this.NavigationService.Navigate(spajanje);
            }
            else {
                MessageBox.Show("Sve prostorije moraju biti na istom spratu!");
            }
        }

        private bool CheckSpratSobe(List<Soba> sobe)
        {
            int sprat = sobe[0].Sprat;
            foreach (Soba soba in sobe) {
                if (soba.Sprat != sprat)
                {
                    return false;
                }
            }
            return true;
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
