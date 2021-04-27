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
    /// Interaction logic for LekarGlavniMeni.xaml
    /// </summary>
    public partial class LekarGlavniMeni : Window
    {
        public LekarGlavniMeni()
        {
            InitializeComponent();
        }

        private void ButtonRaspored_Click(object sender, RoutedEventArgs e)
        {
            var raspored = new LekarRaspored(Sifra);

            BazaOperacija operacije = new BazaOperacija();
            foreach (Operacija op in operacije.SveSledeceOperacije())
                if (op.Lekar.Jmbg.Equals(Sifra))
                {
                    /* raspored.listaOperacija.Items.Add("Pacijent: "+op.Pacijent.Ime+" "+op.Pacijent.Prezime+" "+
                         op.Pacijent.Jmbg +" u prostoriji: "+op.Soba.Tip.ToString()+" "+ op.Soba.Id +" ( "+ op.VremePocetkaOperacije.ToString("dd/MM/yyyy HH:mm")+
                         " - " + op.VremeKrajaOperacije.ToString("HH:mm") + " )"); */
                }
            BazaPregleda pregledi = new BazaPregleda();

            foreach (Pregled pr in pregledi.SviSledeciPregledi())
                if (pr.Lekar.Jmbg.Equals(Sifra))
                {
                    /*
                raspored.listaPregleda.Items.Add("Pacijent: " + pr.Pacijent.Ime + " " + pr.Pacijent.Prezime + " " +
                    pr.Pacijent.Jmbg + " u prostoriji: " + RoomType.ordinacija.ToString() + " " + "FALI_ORDINACIJA_LEKARA" + " ( " + pr.VremePocetkaPregleda.ToString("dd/MM/yyyy HH:mm") +
                    " - " + pr.VremeKrajaPregleda.ToString("HH:mm") + " )"); */
                }

            raspored.ShowDialog();
        }

        private void ButtonPregled_Click(object sender, RoutedEventArgs e)
        {
            var pregled = new LekarWindow();
            pregled.Sifra = Sifra;
            pregled.Show();

            
        }

        public string Sifra { get; set; }


    }

}
