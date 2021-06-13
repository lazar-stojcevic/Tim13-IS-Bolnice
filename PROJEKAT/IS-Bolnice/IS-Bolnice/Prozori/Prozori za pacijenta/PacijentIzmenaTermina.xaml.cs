using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Kontroleri.Korisnicki;
using IS_Bolnice.Kontroleri.Termini;

namespace IS_Bolnice.Prozori
{
    public partial class PacijentIzmenaTermina : Window
    {
        
        private ListView listView;
        private Pregled stariPregled;

        private readonly List<Lekar> lekariOpstePrakse = new LekarKontroler().GetSviLekariOpstePrakse();
        private List<Pregled> pregledi = new List<Pregled>();
        

        private PregledKontroler pregledKontroler = new PregledKontroler();
        private IzmenaTerminaKontroler izmenaTerminaKontroler = new IzmenaTerminaKontroler();

        public PacijentIzmenaTermina(Pregled pregledZaIzmenu, ListView lv)
        {
            InitializeComponent();

            listView = lv;
            stariPregled = pregledZaIzmenu;

            InicijalizacijaDana();
            InicijalizacijaLekara();

        }

        private void InicijalizacijaDana()
        {
            for (int j = 1; j <= 5; j++)
            {
                if (j == 1)
                    comboDani.Items.Add(j + " dan");
                else
                    comboDani.Items.Add(j + " dana");
            }
            comboDani.SelectedIndex = 0;
        }

        private void InicijalizacijaLekara()
        {
            int i = 0;
            foreach (Lekar lekar in lekariOpstePrakse)
            {
                string imePrezime = lekar.Ime + " " + lekar.Prezime;
                comboLekari.Items.Add(imePrezime);

                if (lekar.Jmbg == stariPregled.Lekar.Jmbg)
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
            noviPregled.Lekar = lekariOpstePrakse.ElementAt(comboLekari.SelectedIndex);
            noviPregled.Id = stariPregled.Id;

            if (izmenaTerminaKontroler.DaLiJePacijentMaliciozan(stariPregled.Pacijent))
            {
                string message = "Izvinjavamo se, ali previše puta ste vršili izmene tokom protekle nedelje";
                MessageBox.Show(message);
            }
            else
            {
                if (pregledKontroler.PacijentImaZakazanPregled(noviPregled))
                {
                    string message = "Već imate zakazan pregled u tom terminu";
                    MessageBox.Show(message);
                }
                else
                {
                    pregledKontroler.IzmeniPregled(noviPregled, stariPregled);

                    string message = "Uspešno ste zakazali pregled";
                    MessageBox.Show(message);

                    listView.ItemsSource = pregledKontroler.GetSviBuduciSortiraniPreglediPacijenta(stariPregled.Pacijent.Jmbg);

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
                Lekar lekar = lekariOpstePrakse.ElementAt(comboLekari.SelectedIndex);
                pregledi = pregledKontroler.GetSlobodniTerminiZaIzmenu(lekar, datum);
            }
            else
            {
                pregledi = pregledKontroler.GetSlobodniTerminiZaIzmenu(stariPregled.Lekar, datum);
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
                Lekar lekar = lekariOpstePrakse.ElementAt(comboLekari.SelectedIndex);
                pregledi = pregledKontroler.GetSlobodniTerminiZaIzmenu(lekar, datum);
            }
            else
            {
                pregledi = pregledKontroler.GetSlobodniTerminiZaIzmenu(stariPregled.Lekar, datum);
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

