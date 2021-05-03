using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace IS_Bolnice.Prozori.Sekretar
{
    public partial class FormiranjeObavestenjaWindow : Window
    {
        private BazaObavestenja bo;
        private ObservableCollection<Obavestenje> ObavestenjaRef;

        public ObservableCollection<Pacijent> OdabraniPacijenti 
        { 
            get;
            set;
        }

        public FormiranjeObavestenjaWindow(ObservableCollection<Obavestenje> Obavestenja)
        {
            InitializeComponent();

            this.DataContext = this;
            bo = new BazaObavestenja();
            OdabraniPacijenti = new ObservableCollection<Pacijent>();
            // sluzi da se azurira lista u prikazu
            ObavestenjaRef = Obavestenja;
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            string tmpNaslov = txtNaslov.Text;
            string tmpsadrzaj = txtSadrzaj.Text;
            DateTime tmpVremeKreiranja = DateTime.Now;

            Obavestenje obavestenje = new Obavestenje
            {
                Sifra = tmpsadrzaj.GetHashCode().ToString(),    // kreira se sifra obavestenja na osnovu sadrzaja
                Naslov = tmpNaslov,
                Sadrzaj = tmpsadrzaj,
                VremeKreiranja = tmpVremeKreiranja,
                Uloge = SelektovaneUloge(),
                OdredjeniPacijenti = OdabraniPacijenti.ToList()
            };

            bo.KreirajObavestenje(obavestenje);
            ObavestenjaRef.Add(obavestenje);
            this.Close();
        }

        private List<Uloge> SelektovaneUloge()
        {
            List<Uloge> obavestenjeZaUloge = new List<Uloge>();

            if (ulogaSve.IsChecked == true)
            {
                obavestenjeZaUloge.AddRange(Enum.GetValues(typeof(Uloge)).Cast<Uloge>());
                return obavestenjeZaUloge;
            }
            if (ulogaLekari.IsChecked == true)
            {
                obavestenjeZaUloge.Add(Uloge.Lekari);
            }
            if (ulogaPacijenti.IsChecked == true)
            {
                obavestenjeZaUloge.Add(Uloge.Pacijenti);
            }
            if (ulogaSekretari.IsChecked == true)
            {
                obavestenjeZaUloge.Add(Uloge.Sekretari);
            }
            if (ulogaUpravnici.IsChecked == true)
            {
                obavestenjeZaUloge.Add(Uloge.Upravnici);
            }

            return obavestenjeZaUloge;
        }

        private void dodavanjePacijenata_Click(object sender, RoutedEventArgs e)
        {
            PrikazSvihPacijenata prikazSvihPacijenata = new PrikazSvihPacijenata(OdabraniPacijenti);
            prikazSvihPacijenata.Show();
        }
    }
}
