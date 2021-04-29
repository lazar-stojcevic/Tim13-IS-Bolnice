﻿using System;
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
    public partial class IzmenaObavestenjaWindow : Window
    {
        private BazaObavestenja bo;
        private Obavestenje odabranoObavestenje;
        private ObservableCollection<Obavestenje> ObavestenjaRef;

        public ObservableCollection<Pacijent> OdabraniPacijenti
        {
            get;
            set;
        }

        public IzmenaObavestenjaWindow(Obavestenje obavestenje, ObservableCollection<Obavestenje> Obavestenja)
        {
            InitializeComponent();
            this.DataContext = this;

            bo = new BazaObavestenja();
            odabranoObavestenje = obavestenje;
            ObavestenjaRef = Obavestenja;
            OdabraniPacijenti = new ObservableCollection<Pacijent>(obavestenje.OdredjeniPacijenti);

            txtNaslov.Text = obavestenje.Naslov;
            txtSadrzaj.Text = obavestenje.Sadrzaj;

            CheckRightCheckBoxes();
        }

        private bool IsNotificationForAll()
        {
            return odabranoObavestenje.Uloge.Count == Enum.GetValues(typeof(Uloge)).Cast<Uloge>().Count();
        }

        private void CheckRightCheckBoxes()
        {
            if (IsNotificationForAll())
            {
                ulogaSve.IsChecked = true;
                return;
            }

            foreach (Uloge uloga in odabranoObavestenje.Uloge)
            {
                if (uloga.Equals(Uloge.Lekari))
                {
                    ulogaLekari.IsChecked = true;
                }
                if (uloga.Equals(Uloge.Upravnici))
                {
                    ulogaUpravnici.IsChecked = true;
                }
                if (uloga.Equals(Uloge.Sekretari))
                {
                    ulogaSekretari.IsChecked = true;
                }
                if (uloga.Equals(Uloge.Pacijenti))
                {
                    ulogaPacijenti.IsChecked = true;
                }
            }
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

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            Obavestenje obavestenje = new Obavestenje
            {
                Sifra = odabranoObavestenje.Sifra,  // uvek ce imati pocetnu sifru
                Naslov = txtNaslov.Text,
                Sadrzaj = txtSadrzaj.Text,
                VremeKreiranja = odabranoObavestenje.VremeKreiranja, // uvek ce imati inicijalno vreme kreiranja
                Uloge = SelektovaneUloge(),
                OdredjeniPacijenti = OdabraniPacijenti.ToList()
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

        private void dodavanjePacijenata_Click(object sender, RoutedEventArgs e)
        {
            PrikazSvihPacijenata prikazSvihPacijenata = new PrikazSvihPacijenata(OdabraniPacijenti);
            prikazSvihPacijenata.Show();
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
