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
using System.Windows.Shapes;

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for AddSastojakWindow.xaml
    /// </summary>
    public partial class AddSastojakWindow : Window
    {
        public AddSastojakWindow()
        {
            InitializeComponent();
        }

        private void Dodaj_btn_Click(object sender, RoutedEventArgs e)
        {
            BazaSastojaka bazaSastojaka = new BazaSastojaka();
            Sastojak sastojak = new Sastojak(id_txt.Text);
            bazaSastojaka.Sacuvaj(sastojak);
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
