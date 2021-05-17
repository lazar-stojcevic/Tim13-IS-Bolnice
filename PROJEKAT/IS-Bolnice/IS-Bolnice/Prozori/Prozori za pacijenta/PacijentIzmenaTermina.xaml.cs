﻿using IS_Bolnice.Baze;
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
    public partial class PacijentIzmenaTermina : Window
    {
        private List<Lekar> lekari;
        private ListView listView;

        private List<Pregled> pregledi = new List<Pregled>();
        private Pregled stariPregled = new Pregled();

        private BazaPregleda bazaPregleda = new BazaPregleda();
        private BazaIzmena bazaIzmena = new BazaIzmena();

        public PacijentIzmenaTermina(string jmbgPacijenta, string jmbgLekara, DateTime datum, ListView lv)
        {
            InitializeComponent();

            listView = lv;

            Lekar ll = new Lekar();
            Pacijent p = new Pacijent();

            ll.Jmbg = jmbgLekara;
            p.Jmbg = jmbgPacijenta;

            stariPregled.Lekar = ll;
            stariPregled.Pacijent = p;
            stariPregled.VremePocetkaPregleda = datum;

            BazaLekara bl = new BazaLekara();
            lekari = bl.LekariOpstePrakse();

            //UPISIVANJE DANA U comboBox
            for (int j = 1; j <= 5; j++)
            {
                if (j == 1)
                    comboDani.Items.Add(j + " dan");
                else
                    comboDani.Items.Add(j + " dana");
            }
            comboDani.SelectedIndex = 0;

            //UPISIVANJE LEKARA I SELEKTOVANJE LEKARA CIJI SE TERMIN MENJA
            int i = 0;
            foreach (Lekar l in lekari)
            {
                string imePrezime = l.Ime + " " + l.Prezime;
                comboLekari.Items.Add(imePrezime);

                if (l.Jmbg == jmbgLekara)
                {
                    comboLekari.SelectedIndex = i;
                }

                i += 1;
            }

        }

        //DODATI PROVERU
        private void btnPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            Pregled noviPregled = new Pregled();
            Pacijent pacijent = new Pacijent();

            noviPregled = pregledi.ElementAt(listTermina.SelectedIndex);
            pacijent.Jmbg = stariPregled.Pacijent.Jmbg;
            noviPregled.Pacijent = pacijent;
            noviPregled.Lekar = lekari.ElementAt(comboLekari.SelectedIndex);

            if (bazaIzmena.IsPatientMalicious(pacijent))
            {
                string message = "Izvinjavamo se, ali previše puta ste vršili izmene tokom protekle nedelje";
                MessageBox.Show(message);
            }
            else
            {
                if (bazaPregleda.PacijentImaZakazanPregled(noviPregled))
                {
                    string message = "Već imate zakazan pregled u tom terminu";
                    MessageBox.Show(message);
                }
                else
                {
                    bazaPregleda.IzmeniPregled(noviPregled, stariPregled);

                    string message = "Uspešno ste zakazali pregled";
                    MessageBox.Show(message);

                    listView.ItemsSource = bazaPregleda.SviBuduciPreglediKojePacijentIma(stariPregled.Pacijent.Jmbg);

                    this.Close();
                }
            }
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void comboDani_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboLekari.SelectedIndex == -1 || comboDani.SelectedIndex == -1 || listTermina.SelectedIndex == -1)
            {
                btnPotvrdi.IsEnabled = false;
            }
            else
            {
                btnPotvrdi.IsEnabled = true;
            }

            int dan = comboDani.SelectedIndex + 1;

            DateTime datum = stariPregled.VremePocetkaPregleda.AddDays(dan);

            //OGRANICENJE PRILIKOM PRVOG POKRETANJA
            if (comboLekari.SelectedIndex != -1)
            {
                Lekar lekar = lekari.ElementAt(comboLekari.SelectedIndex);
                pregledi = bazaPregleda.SlobodniTerminiZaIzmenu(lekar, datum);
            }
            else
            {
                pregledi = bazaPregleda.SlobodniTerminiZaIzmenu(stariPregled.Lekar, datum);
            }

            listTermina.Items.Clear();

            foreach (Pregled p in pregledi)
            {
                listTermina.Items.Add(p.VremePocetkaPregleda);
            }
        }

        private void comboLekari_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboLekari.SelectedIndex == -1 || comboDani.SelectedIndex == -1 || listTermina.SelectedIndex == -1)
            {
                btnPotvrdi.IsEnabled = false;
            }
            else
            {
                btnPotvrdi.IsEnabled = true;
            }

            int dan = comboDani.SelectedIndex + 1;

            DateTime datum = stariPregled.VremePocetkaPregleda.AddDays(dan);

            //OGRANICENJE PRILIKOM PRVOG POKRETANJA
            if (comboDani.SelectedIndex != -1)
            {
                Lekar lekar = lekari.ElementAt(comboLekari.SelectedIndex);
                pregledi = bazaPregleda.SlobodniTerminiZaIzmenu(lekar, datum);
            }
            else
            {
                pregledi = bazaPregleda.SlobodniTerminiZaIzmenu(stariPregled.Lekar, datum);
            }

            listTermina.Items.Clear();

            foreach (Pregled p in pregledi)
            {
                listTermina.Items.Add(p.VremePocetkaPregleda);
            }
        }

        private void listTermina_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboLekari.SelectedIndex == -1 || comboDani.SelectedIndex == -1 || listTermina.SelectedIndex == -1)
            {
                btnPotvrdi.IsEnabled = false;
            }
            else
            {
                btnPotvrdi.IsEnabled = true;
            }
        }
    }
}

