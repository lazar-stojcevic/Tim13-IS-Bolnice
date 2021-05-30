using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Model;

namespace IS_Bolnice.Prozori.Sekretar
{
    public partial class FormiranjeObavestenjaWindow : Window
    {
        private ObavestenjeKontroler obavestenjeKontroler = new ObavestenjeKontroler();
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
                Id = Guid.NewGuid().ToString(),    // kreira se id obavestenja na osnovu sadrzaja
                Naslov = tmpNaslov,
                Sadrzaj = tmpsadrzaj,
                VremeKreiranja = tmpVremeKreiranja,
                Uloge = SelektovaneUloge(),
                OdredjeniPacijenti = OdabraniPacijenti.ToList()
            };

            obavestenjeKontroler.KreirajObavestenje(obavestenje);
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

        private void TxtNaslov_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string text = txtNaslov.Text;
            if (text.Contains("#"))
            {
                text = text.Replace("#", "");
                txtNaslov.Text = text;
                InformativniProzor ip = new InformativniProzor("Naslov ne može da sadrži '#'");
                ip.ShowDialog();
            }
        }

        private void TxtSadrzaj_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = txtSadrzaj.Text;
            if (text.Contains("#"))
            {
                text = text.Replace("#", "");
                txtSadrzaj.Text = text;
                InformativniProzor ip = new InformativniProzor("Sadržaj ne može da sadrži '#'");
                ip.ShowDialog();
            }
        }
    }
}
