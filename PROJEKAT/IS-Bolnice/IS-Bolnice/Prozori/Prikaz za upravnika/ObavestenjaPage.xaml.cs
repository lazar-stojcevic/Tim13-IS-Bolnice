using IS_Bolnice.Model;
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
    /// Interaction logic for ObavestenjaPage.xaml
    /// </summary>
    public partial class ObavestenjaPage : Page
    {
        List<Obavestenje> obavestenja = new List<Obavestenje>();
        public ObavestenjaPage()
        {
            InitializeComponent();
            Obavestenje obavestenje = new Obavestenje();
            obavestenje.Naslov = "Praznik rada";
            obavestenje.Sadrzaj = "1. maja 2021. obavljaće se samo hitni pregledi";
            Obavestenje obavestenje1 = new Obavestenje();
            obavestenje1.Naslov = "Seminar";
            Obavestenje obavestenje2 = new Obavestenje();
            obavestenje2.Naslov = "Renoviranje sale";
            obavestenja.Add(obavestenje2);
            obavestenja.Add(obavestenje1);
            obavestenja.Add(obavestenje);
            listBox.ItemsSource = obavestenja;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PrikazObavestenjaWindow window = new PrikazObavestenjaWindow((Obavestenje)listBox.SelectedItem);
            window.Show();
        }
    }
}
