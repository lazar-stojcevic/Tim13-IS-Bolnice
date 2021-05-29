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
    /// Interaction logic for EditSastojakWindow.xaml
    /// </summary>
    public partial class EditSastojakWindow : Window
    {
        string ime;
        public EditSastojakWindow(string name)
        {
            InitializeComponent();
            ime = name;
            id_txt.Text = name;
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Izmeni_btn_Click(object sender, RoutedEventArgs e)
        {
            BazaSastojaka bazaSastojaka = new BazaSastojaka();
            Sastojak stariSastojak = new Sastojak(ime);
            bazaSastojaka.Obrisi(stariSastojak.Ime);
            Sastojak noviSastojak = new Sastojak(id_txt.Text);
            bazaSastojaka.Sacuvaj(noviSastojak);
        }
    }
}
