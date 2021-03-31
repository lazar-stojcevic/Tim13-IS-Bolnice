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

namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for IzmenaObavestenjaWindow.xaml
    /// </summary>
    public partial class IzmenaObavestenjaWindow : Window
    {
        private BazaObavestenja bo;
        private Obavestenje odabranoObavestenje;
        private string sifraObavestenja;

        public IzmenaObavestenjaWindow(Obavestenje obavestenje)
        {
            InitializeComponent();

            bo = new BazaObavestenja();
            odabranoObavestenje = obavestenje;

            txtNaslov.Text = obavestenje.Naslov;
            txtSadrzaj.Text = obavestenje.Sadrzaj;
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            Obavestenje obavestenje = new Obavestenje
            {
                Sifra = odabranoObavestenje.Sifra,
                Naslov = txtNaslov.Text,
                Sadrzaj = txtSadrzaj.Text,
                VremeKreiranja = odabranoObavestenje.VremeKreiranja
            };

            bo.IzmeniObavestenje(obavestenje);
            this.Close();
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
