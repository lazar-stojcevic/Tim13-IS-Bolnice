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

namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for IzmenaObavestenjaWindow.xaml
    /// </summary>
    public partial class IzmenaObavestenjaWindow : Window
    {
        private BazaObavestenja bo;
        private Obavestenje odabranoObavestenje;
        private ObservableCollection<Obavestenje> ObavestenjaRef;

        public IzmenaObavestenjaWindow(Obavestenje obavestenje, ObservableCollection<Obavestenje> Obavestenja)
        {
            InitializeComponent();

            bo = new BazaObavestenja();
            odabranoObavestenje = obavestenje;
            ObavestenjaRef = Obavestenja;

            txtNaslov.Text = obavestenje.Naslov;
            txtSadrzaj.Text = obavestenje.Sadrzaj;
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            Obavestenje obavestenje = new Obavestenje
            {
                Sifra = odabranoObavestenje.Sifra,  // uvek ce imati pocetnu sifru
                Naslov = txtNaslov.Text,
                Sadrzaj = txtSadrzaj.Text,
                VremeKreiranja = odabranoObavestenje.VremeKreiranja // uvek ce imati inicijalno vreme kreiranja
            };

            bo.IzmeniObavestenje(obavestenje);
            // osvezavanje liste
            int i = ObavestenjaRef.IndexOf(odabranoObavestenje);
            if (i != -1)
            {
                ObavestenjaRef[i] = obavestenje;
            }

            this.Close();
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
