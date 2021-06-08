﻿using IS_Bolnice.Kontroleri;
using IS_Bolnice.Prozori;
using IS_Bolnice.Prozori.Prikaz_za_upravnika;
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
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice
{
    public partial class MainWindow : Window
    {
        SadrzajSobeKontroler kontroler = new SadrzajSobeKontroler();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click_1(object sender, RoutedEventArgs e)
        {
            kontroler.IzvrsiTransport();
            string korisnik = txtUserId.Text;
            string sifra = txtPassword.Password;
            IUpravnikRepozitorijum upravnikRepo = new UpravnikFajlRepozitorijum();
            List<Upravnik> upravnici = new List<Upravnik>();
            upravnici = upravnikRepo.DobaviSve();
            bool found = false;
            foreach (Upravnik u in upravnici)
            {
                if (u.KorisnickoIme.Equals(korisnik))
                {
                    if (u.Sifra.Equals(sifra))
                    {
                        found = true;
                        UpravnikWindow upravnik = new UpravnikWindow();
                        upravnik.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Korisnicko ime i sifra se ne poklapaju");
                        found = true;
                    }
                }
            }

            if (found == false)
            {
                ISekretarRepozitorijum sekretarRepo = new SekretarFajlRepozitorijum();
                List<Sekretar> sekretari = new List<Sekretar>();
                sekretari = sekretarRepo.DobaviSve();
                foreach (Sekretar s in sekretari)
                {
                    if (s.KorisnickoIme.Equals(korisnik))
                    {
                        if (s.Sifra.Equals(sifra))
                        {
                            found = true;
                            Prozori.Sekretar.SekretarWindow sekretar = new Prozori.Sekretar.SekretarWindow(s);
                            sekretar.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Korisnicko ime i sifra se ne poklapaju");
                            found = true;
                        }
                    }
                }
            }

            if (found == false)
            {
                LekarFajlRepozitorijum baza3 = new LekarFajlRepozitorijum();
                List<Lekar> lekari = new List<Lekar>();
                lekari = baza3.DobaviSve();
                foreach (Lekar l in lekari)
                {
                    if (l.KorisnickoIme.Equals(korisnik))
                    {
                        if (l.Sifra.Equals(sifra))
                        {
                            found = true;
                            Prozori.Prikaz_kod_lekara.LekarFrejm lekar = new Prozori.Prikaz_kod_lekara.LekarFrejm(l.Jmbg);
                            lekar.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Korisnicko ime i sifra se ne poklapaju");
                            found = true;
                        }
                    }
                }
            }

            if (found == false)
            {
                IPacijentRepozitorijum pacijentRepo = new PacijentFajlRepozitorijum();
                List<Pacijent> pacijenti = new List<Pacijent>();
                pacijenti = pacijentRepo.DobaviSve();
                foreach (Pacijent p in pacijenti)
                {
                    if (p.KorisnickoIme.Equals(korisnik))
                    {
                        if (p.Sifra.Equals(sifra))
                        {
                            found = true;
                            PacijentWindow pacijent = new PacijentWindow();
                            pacijent.imeKorisnika.Text = p.Ime + " " + p.Prezime + " " + p.Jmbg;
                            pacijent.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Korisnicko ime i sifra se ne poklapaju");
                            found = true;
                        }
                    }
                }
            }

            if (found == false)
            {
                MessageBox.Show("Korisnicko ime ne postoji!");
            }
        }

        private void btnClose_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
