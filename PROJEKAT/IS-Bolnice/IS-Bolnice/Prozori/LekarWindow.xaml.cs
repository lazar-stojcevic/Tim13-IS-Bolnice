﻿using System;
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
    
    public partial class LekarWindow : Window
    {
        public LekarWindow()
        {
            InitializeComponent();
        }

        private void btnKreirajOperaciju(object sender, RoutedEventArgs e)
        {
            var operacija = new ZakazivanjeOperacije();
            operacija.txtOperJmbg.Text = txtJMBG.Text;
            operacija.txtOperIme.Text = txtIme.Text;
            operacija.txtOperPrz.Text = txtPrz.Text;
            operacija.ShowDialog();

        }

        private void btnKreirajPregled(object sender, RoutedEventArgs e)
        {
            var pregled = new LekarZakazivanjePregleda();
            pregled.txtOperJmbg.Text = txtJMBG.Text;
            pregled.txtOperIme.Text = txtIme.Text;
            pregled.txtOperPrz.Text = txtPrz.Text;
            pregled.ShowDialog();

        }

        private void btnUcitajPacijenta(object sender, RoutedEventArgs e)
        {
            bool nasao = false;
            BazaPacijenata baza = new BazaPacijenata();
            foreach (Pacijent p in baza.SviPacijenti())
            {
                if (txtJMBG.Text.Equals(p.Jmbg))
                {
                    txtIme.Text = p.Ime;
                    txtPrz.Text = p.Prezime;
                    btnOperacija.IsEnabled = true;
                    btnPregled.IsEnabled = true;
                    btnIzvestaj.IsEnabled = true;
                    nasao = true;
                }
                else
                {
                    Console.WriteLine(txtJMBG);
                    Console.WriteLine(p.Jmbg);
                    Console.WriteLine("Ovaj nije isti");
                }

                if (!nasao)
                {
                    btnOperacija.IsEnabled = false;
                    btnPregled.IsEnabled = false;
                    btnIzvestaj.IsEnabled = false;
                }

            }
            if (!nasao) 
            { 
                MessageBox.Show("Ne postoji pacijent sa unetim jmbg-om", "Probaj ponovo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ButtonRaspored_Click(object sender, RoutedEventArgs e)
        {
            var raspored = new LekarRaspored(Sifra);
            
            BazaOperacija operacije = new BazaOperacija();
            BazaPregleda pregledi = new BazaPregleda();

            raspored.ShowDialog();
        }

        private void odjavaClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow prijava = new MainWindow();
            prijava.ShowDialog();
            this.Close();

        }

        private void ButtonIzvestaj_Click(object sender, RoutedEventArgs e)
        {
            string jmbgPacijenta = txtJMBG.Text;
            LekarIzvestaj izvestaj = new LekarIzvestaj(jmbgPacijenta, Sifra);
            izvestaj.Show();

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public string Sifra { get; set; }
    }




}
