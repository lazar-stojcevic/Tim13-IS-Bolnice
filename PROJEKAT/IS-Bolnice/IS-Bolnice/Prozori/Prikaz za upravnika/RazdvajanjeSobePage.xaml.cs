﻿using IS_Bolnice.Kontroleri;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using IS_Bolnice.Kontroleri.Ustanova;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for RazdvajanjeSobePage.xaml
    /// </summary>
    public partial class RazdvajanjeSobePage : Page
    {
        Renovacija renovacija;

        RenovacijaKontroler renovacijaKontroler = new RenovacijaKontroler();
        public RazdvajanjeSobePage(Renovacija renovacijeStareSobe)
        {
            InitializeComponent();
            renovacija = renovacijeStareSobe;
        }

        private void kvadratura1_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Double.Parse(kvadratura1_txt.Text)<renovacija.ProstorijaZaRenoviranje.Kvadratura) {
                if (kvadratura1_txt.Text != "")
                {
                    double razlika = renovacija.ProstorijaZaRenoviranje.Kvadratura - Double.Parse(kvadratura1_txt.Text);
                    kvadratura2_txt.Text = razlika.ToString();
                }
                else {
                    kvadratura2_txt.Text = renovacija.ProstorijaZaRenoviranje.Kvadratura.ToString();
                }
            }
        }

        private void kvadratura2_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Double.Parse(kvadratura2_txt.Text) < renovacija.ProstorijaZaRenoviranje.Kvadratura)
            {
                if (kvadratura2_txt.Text != "")
                {
                    double razlika = renovacija.ProstorijaZaRenoviranje.Kvadratura - Double.Parse(kvadratura2_txt.Text);
                    kvadratura1_txt.Text = razlika.ToString();
                }
                else
                {
                    kvadratura1_txt.Text = renovacija.ProstorijaZaRenoviranje.Kvadratura.ToString();
                }
            }
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Potvrdi_btn_Click(object sender, RoutedEventArgs e)
        {
            if (Validiraj())
            {
                List<Soba> noveSobe = KreirajNoveSobe();
                renovacijaKontroler.RazdvajanjeSobe(renovacija, noveSobe);
                Page page = new RenoviranjeSpajanjePage();
                this.NavigationService.Navigate(page);
                CustomMessageBox.ShowOK("Renoviranje je uspešno kreirano!", "Uspeh", "Potvrdi");
            }
            else {
                CustomMessageBox.ShowOK("Ne sme se koristiti # ili /", "Greška", "Potvrdi");
            }
        }

        private bool Validiraj()
        {
            if (id1_txt.Text.Contains("#") || id1_txt.Text.Contains("/"))
            {
                return false;
            }
            if (id2_txt.Text.Contains("#") || id2_txt.Text.Contains("/"))
            {
                return false;
            }

            return true;
        }

        private List<Soba> KreirajNoveSobe() 
        {
            Soba soba1 = new Soba(id1_txt.Text, (RoomType)tip_sobe1_txt.SelectedIndex, false, renovacija.ProstorijaZaRenoviranje.Sprat, Double.Parse(kvadratura1_txt.Text));
            Soba soba2 = new Soba(id2_txt.Text, (RoomType)tip_sobe2_txt.SelectedIndex, false, renovacija.ProstorijaZaRenoviranje.Sprat, Double.Parse(kvadratura2_txt.Text));
            List<Soba> sobe = new List<Soba>();
            sobe.Add(soba1);
            sobe.Add(soba2);
            return sobe;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
