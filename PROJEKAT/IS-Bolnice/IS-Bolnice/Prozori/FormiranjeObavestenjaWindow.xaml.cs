using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class FormiranjeObavestenjaWindow : Window
    {
        BazaObavestenja bo;

        public FormiranjeObavestenjaWindow()
        {
            InitializeComponent();

            bo = new BazaObavestenja();
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string tmpNaslov = txtNaslov.Text;
            string tmpsadrzaj = txtSadrzaj.Text;
            DateTime tmpVremeKreiranja = DateTime.Now;

            Obavestenje obavestenje = new Obavestenje
            {
                Sifra = tmpsadrzaj.GetHashCode().ToString(),    // kreira se sifra obavestenja na osnovu sadrzaja
                Naslov = tmpNaslov,
                Sadrzaj = tmpsadrzaj,
                VremeKreiranja = tmpVremeKreiranja
            };

            bo.KreirajObavestenje(obavestenje);
            this.Close();
        }
    }
}
