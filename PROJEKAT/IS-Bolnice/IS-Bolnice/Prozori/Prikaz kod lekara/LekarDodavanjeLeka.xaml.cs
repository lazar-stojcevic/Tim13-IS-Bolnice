﻿using System;
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

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarDodavanjeLeka.xaml
    /// </summary>
    public partial class LekarDodavanjeLeka : Window
    {
        ObservableCollection<Terapija> pomocna = new ObservableCollection<Terapija>();
        public LekarDodavanjeLeka(ObservableCollection<Terapija> terapija, string jmbgPacijenta)
        {
            BazaLekova bazaLekova = new BazaLekova();
            BazaPacijenata bazaPacijenata = new BazaPacijenata();
            List<Lek> lekovi = bazaLekova.SviLekovi();
            List<Lek> lekoviZaPrikaz = bazaLekova.SviLekovi();
            Pacijent p = bazaPacijenata.PacijentSaOvimJMBG(jmbgPacijenta);

            if(p.Alergeni.Count != 0)
            {
                int i = 0;
                foreach (string alergen in p.Alergeni)
                {
                    foreach (Lek lek in lekoviZaPrikaz)
                    {
                        ++i;
                        foreach (Sastojak alergenLek in lek.Alergeni)
                        {
                            if (alergenLek.Ime.Equals(alergen) && !alergenLek.Ime.Equals(""))
                            { 
                                lekovi.RemoveAt(i);
                            }
                            --i;
                            break;
                        }
                    }
                }
            }

            InitializeComponent();
            listaSvihLekova.ItemsSource = lekovi;
            pomocna = terapija;

        }

        private void Button_DodajClick(object sender, RoutedEventArgs e)
        {
            Terapija t = new Terapija();
            Lek l = (Lek)listaSvihLekova.SelectedItem;
            t.Lek = l;
            t.UcestanostKonzumiranja = Double.Parse(txtBrojUzimanja.Text);
            t.VremePocetka = System.DateTime.Now;
            t.VremeKraja = DateTime.Now.AddDays(Int16.Parse(txtTrajanje.Text));
            t.RazlikaNaKolikoSeDanaUzimaLek = comboboxNaKolikoDana.SelectedIndex;
            t.Opis = txtDetalji.Text;
            pomocna.Add(t);

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_KrajClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}