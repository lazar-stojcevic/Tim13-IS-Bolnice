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
using IS_Bolnice.Kontroleri;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarDodavanjeLeka.xaml
    /// </summary>
    public partial class LekarDodavanjeLeka : Window
    {
        ObservableCollection<Terapija> sveZadateTerapije = new ObservableCollection<Terapija>();

        private PacijentKontroler pacijentKontroler = new PacijentKontroler();
        private LekKontroler lekKontroler = new LekKontroler();
        private List<Lek> sviLekovi = new List<Lek>();
        public LekarDodavanjeLeka(ObservableCollection<Terapija> terapija, string jmbgPacijenta)
        {
            sviLekovi = lekKontroler.GetSviLekovi();
            PrikazLekovaNaKojePacijentNijeAlergican(jmbgPacijenta, sviLekovi);
            InitializeComponent();
            sviLekovi.Clear();

            foreach (Lek lek in lekKontroler.GetSviLekovi())
            {
                bool isValid = true;
                Pacijent pacijentKojiJeNaPregledu = pacijentKontroler.GetPacijentSaOvimJMBG(jmbgPacijenta);
                foreach (Sastojak alergenPacijenta in pacijentKojiJeNaPregledu.Alergeni)
                {
                    foreach (Sastojak alergen in lek.Alergeni)
                    {
                        if (alergen.Ime.Equals(alergenPacijenta.Ime))
                        {
                            isValid = false;
                            break;
                        }
                    }
                }
                if (isValid)
                    sviLekovi.Add(lek);
            }
            listaSvihLekova.ItemsSource = sviLekovi;

            sveZadateTerapije = terapija;
        }

        private void PrikazLekovaNaKojePacijentNijeAlergican(string jmbgPacijenta, List<Lek> sviLekovi)
        {
            List<Lek> lekoviZaPrikaz = lekKontroler.GetSviLekovi();
            Pacijent pacijentKojiJeNaPregledu = pacijentKontroler.GetPacijentSaOvimJMBG(jmbgPacijenta);

            if (pacijentKojiJeNaPregledu.Alergeni.Count != 0)
            {
                /*
                foreach (Sastojak alergenKodPacijenta in pacijentKojiJeNaPregledu.Alergeni)
                {
                    int index = 0;
                    foreach (Lek lek in lekoviZaPrikaz)
                    {
                        foreach (Sastojak alergenLek in lek.Alergeni)
                        {
                            if (alergenLek.Ime.Equals(alergenKodPacijenta.Ime) && !alergenLek.Ime.Equals(""))
                            {
                                sviLekovi.RemoveAt(index);
                                --index;
                                break;
                            }
                        }
                        ++index;
                    }
                }
              
                sviLekovi.Clear();

                foreach (Lek lek in lekKontroler.GetSviLekovi())
                {
                    bool isValid = true;
                    foreach (Sastojak alergenPacijenta in pacijentKojiJeNaPregledu.Alergeni)
                    {
                        if (lek.Alergeni.Contains(alergenPacijenta))
                        {
                            isValid = false;
                            break;
                        }
                    }
                    if (isValid)
                        sviLekovi.Add(lek);
                }
                if (listaSvihLekova != null)
                    listaSvihLekova.ItemsSource = sviLekovi;
                  */

            }

        }

        private void Button_DodajClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da ste dobro uneli svi podatke terapije?", "Dodavanje terapije", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DodajNovuTerapiju();
            }
        }

        private void DodajNovuTerapiju()
        {

            try
            {
                Terapija t = new Terapija();
                Lek l = (Lek) listaSvihLekova.SelectedItem;
                t.Lek = l;
                t.UcestanostKonzumiranja = Double.Parse(txtBrojUzimanja.Text);
                t.VremePocetka = System.DateTime.Now;
                t.VremeKraja = DateTime.Now.AddDays(Int32.Parse(txtTrajanje.Text));
                t.RazlikaNaKolikoSeDanaUzimaLek = comboboxNaKolikoDana.SelectedIndex;
                t.Opis = txtDetalji.Text;
                sveZadateTerapije.Add(t);
            }
            catch (Exception e)
            {
                CustomMessageBox.ShowOK("Niste dobro uneli parametre za terapiju", "Dodavanje terapije", "Dobro", MessageBoxImage.Error);
            }

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_KrajClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da ste definisali sve terapije?", "Dodavanje terapije", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }


        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 1;
        }

        private void ToggleButton_OnUnchecked_UnChecked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 0;
        }

    }
}
