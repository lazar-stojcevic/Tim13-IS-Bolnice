using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for PrikazUsluga.xaml
    /// </summary>
    public partial class PrikazUsluga : Window
    {
        public ObservableCollection<string> Usluge
        {
            get;
            set;
        }

        public ObservableCollection<string> DodateUsluge
        {
            get;
            set;
        }

        private List<string> odabraneUsluge;

        public PrikazUsluga(List<string> odabraneUsluge)
        {
            InitializeComponent();

            this.DataContext = this;
            this.odabraneUsluge = odabraneUsluge;
            Usluge = new ObservableCollection<string>();
            DodateUsluge = new ObservableCollection<string>();
            Usluge.Add("Vađenje krvi     -   1000 dinara");
            Usluge.Add("Redovan pregled  -   1000 dinara");
            Usluge.Add("Operacija mozga  -   100000 dinara");
            Usluge.Add("Ultra zvuk       -   5000 dinara");
            Usluge.Add("Previjanje rane  -   2000 dinara");
            Usluge.Add("Davanje infuzije -   3000 dinara");
        }

        private void Button_Click_Dodaj(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < lvUsluge.SelectedItems.Count; i++)
            {
                string usluga = (string)lvUsluge.SelectedItems[i];
                DodateUsluge.Add(usluga);
            }
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            foreach (var usluga in DodateUsluge)
            {
                odabraneUsluge.Add(usluga);
            }
            Close();
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
