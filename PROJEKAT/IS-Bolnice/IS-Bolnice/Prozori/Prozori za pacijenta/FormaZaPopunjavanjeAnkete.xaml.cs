using IS_Bolnice.Kontroleri;
using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using IS_Bolnice.Kontroleri.Informativni;

namespace IS_Bolnice.Prozori.Prozori_za_pacijenta
{
    public partial class FormaZaPopunjavanjeAnkete : Window
    {
        private string jmbgPacijenta;

        private List<Pregled> preglediZaAnketu = new List<Pregled>();

        private AnketaKontroler anketaKontroler = new AnketaKontroler();

        private Anketa anketa = new Anketa();

        public FormaZaPopunjavanjeAnkete(string jmbgPac)
        {
            InitializeComponent();

            jmbgPacijenta = jmbgPac;
            preglediZaAnketu = anketaKontroler.GetSviPreglediZaAnketu(jmbgPacijenta);

            PostaviAnkete();

        }

        private void PostaviAnkete()
        {
            foreach (Pregled pregled in preglediZaAnketu)
            {
                surveryName.Items.Add(pregled.Lekar.Ime + " " + pregled.Lekar.Prezime);
            }
        }

        private void nazivAnkete_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (surveryName.SelectedIndex == -1)
            {
                potvrdi.IsEnabled = false;
            }
            else
            {
                potvrdi.IsEnabled = true;
            }
        }
        private void ocenjivanjeDoktora()
        {
            Pregled pregled = preglediZaAnketu.ElementAt(surveryName.SelectedIndex);

            anketa.Lekar = pregled.Lekar;
            anketa.Pacijent = pregled.Pacijent;
            anketa.Ocena = KojaOcenaJeSelektovana();
            anketa.Trajanje = pregled.VremePocetkaPregleda;
            anketa.Opis = komentar.Text;
            anketa.KojaAnketa = 0;
            anketaKontroler.SacuvajAnketu(anketa);
        }

        private void ocenjivanjeBolnice()
        {
            Pacijent patient = new Pacijent();
            patient.Jmbg = jmbgPacijenta;

            Bolnica hospital = new Bolnica();
            hospital.Ime = "Naziv kompanije";

            anketa.Pacijent = patient;
            anketa.Trajanje = DateTime.Now;
            anketa.Opis = komentar.Text;
            anketa.Ocena = KojaOcenaJeSelektovana();
            anketa.Bolnica = hospital;
            anketa.KojaAnketa = 1;
            anketaKontroler.SacuvajAnketu(anketa);
        }

        private void potvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (radBtnLekari.IsChecked.Value)
            {
                ocenjivanjeDoktora();
                this.Close();
            }
            else
            {
                if (anketaKontroler.DaLiJeVremeZaAnketuBolnice(jmbgPacijenta))
                {
                    ocenjivanjeBolnice();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Jos uvek nemate pravo da ocenite kompaniju");
                    radBtnLekari.IsChecked = true;
                    radBtnKompanija.IsEnabled = false;
                }
            }
        }

        private void odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private int KojaOcenaJeSelektovana()
        {
            int grade;

            if (btn1.IsChecked.Value)
            {
                grade = 1;
            }
            else if (btn2.IsChecked.Value)
            {
                grade = 2;
            }
            else if (btn3.IsChecked.Value)
            {
                grade = 3;
            }
            else if (btn4.IsChecked.Value)
            {
                grade = 4;
            }
            else
            {
                grade = 5;
            }

            return grade;
        }

        private void radBtnLekari_Checked(object sender, RoutedEventArgs e)
        {
            if (surveryName != null)
            {
                surveryName.IsEnabled = true;

                if (surveryName.SelectedIndex == -1)
                {
                    potvrdi.IsEnabled = false;
                }
            }
        }

        private void radBtnKompanija_Checked(object sender, RoutedEventArgs e)
        {
            surveryName.IsEnabled = false;
            surveryName.SelectedIndex = -1;

            potvrdi.IsEnabled = true;
        }
    }
}