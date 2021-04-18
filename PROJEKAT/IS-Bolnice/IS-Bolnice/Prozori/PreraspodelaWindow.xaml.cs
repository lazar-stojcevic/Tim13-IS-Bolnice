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
    /// <summary>
    /// Interaction logic for PreraspodelaWindow.xaml
    /// </summary>
    public partial class PreraspodelaWindow : Window
    {
        public PreraspodelaWindow()
        {
            InitializeComponent();
            BazaBolnica baza = new BazaBolnica();
            List<Bolnica> bolnice = baza.SveBolnice();
            Bolnica b = bolnice[0];
            List<string> lista = new List<string>();
            lista.Add("");
            foreach (Soba s in b.Soba) {
                lista.Add(s.Id);
            }
            Soba1.ItemsSource = lista;
            Soba2.ItemsSource = lista;

        }

        private void Button_Plus(object sender, RoutedEventArgs e)
        {
            int i = Int32.Parse(kol_txt.Text);
            i++;
            kol_txt.Text = i.ToString();
        }

        private void Button_Minus(object sender, RoutedEventArgs e)
        {
            int i = Int32.Parse(kol_txt.Text);
            if (i > 0)
            {
                i--;
                kol_txt.Text = i.ToString();
            }
        }

        private void Soba2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Prikaz_Sobe(Soba2.SelectedItem.ToString(), 2);
        }

        private void Button_Up(object sender, RoutedEventArgs e)
        {
            BazaBolnica baza = new BazaBolnica();
            List<Bolnica> bolnice = baza.SveBolnice();
            Bolnica b = bolnice[0];
            bool postoji = false;
            if (Lista2.SelectedItem != null)
            {
                string[] predmet = Lista2.SelectedItem.ToString().Split(' ');
                string idp = predmet[1];
                bool tacno = false;
                foreach (Soba s in b.Soba)
                {
                    if (s.Id.Equals(Soba1.SelectedItem.ToString()))
                    {
                        foreach (Predmet p in s.Predmet)
                        {
                            if (p.Id.Equals(idp))
                            {
                                p.Kolicina += Int32.Parse(kol_txt.Text);
                                postoji = true;
                                break;
                            }
                        }
                        if (!postoji)
                        {
                            Predmet p = new Predmet();
                            p.Id = idp;
                            p.Kolicina = Int32.Parse(kol_txt.Text);
                            s.AddPredmet(p);
                        }
                    }
                    else if (s.Id.Equals(Soba2.SelectedItem.ToString()))
                    {

                        foreach (Predmet p in s.Predmet)
                        {
                            if (p.Id.Equals(idp))
                            {
                                if (p.Kolicina > Int32.Parse(kol_txt.Text))
                                {
                                    p.Kolicina -= Int32.Parse(kol_txt.Text);
                                    tacno = true;
                                    break;
                                }
                                else if (p.Kolicina == Int32.Parse(kol_txt.Text))
                                {
                                    s.RemovePredmet(p);
                                    tacno = true;
                                    break;
                                }
                                else
                                {
                                    tacno = false;
                                }
                            }
                        }

                    }
                }
                if (tacno)
                {
                    baza.KreirajBolnicu(b);
                }
                else
                {
                    MessageBox.Show("Nije odabrana odgovarajuća količina za prenos.");
                }
                this.Prikaz_Sobe(Soba1.SelectedItem.ToString(), 1);
                this.Prikaz_Sobe(Soba2.SelectedItem.ToString(), 2);
            }
            else {
                MessageBox.Show("Objekat nije selektovan");
            }
        }

        private void Button_Down(object sender, RoutedEventArgs e)
        {
            BazaBolnica baza = new BazaBolnica();
            List<Bolnica> bolnice = baza.SveBolnice();
            Bolnica b = bolnice[0];
            bool postoji = false;
            if (Lista1.SelectedItem != null)
            {
                string[] predmet = Lista1.SelectedItem.ToString().Split(' ');
                string idp = predmet[1];
                bool tacno = false;
                foreach (Soba s in b.Soba)
                {
                    if (s.Id.Equals(Soba2.SelectedItem.ToString()))
                    {
                        foreach (Predmet p in s.Predmet)
                        {
                            if (p.Id.Equals(idp))
                            {
                                p.Kolicina += Int32.Parse(kol_txt.Text);
                                postoji = true;
                                break;
                            }
                        }
                        if (!postoji)
                        {
                            Predmet p = new Predmet();
                            p.Id = idp;
                            p.Kolicina = Int32.Parse(kol_txt.Text);
                            s.AddPredmet(p);
                        }
                    }
                    else if (s.Id.Equals(Soba1.SelectedItem.ToString()))
                    {

                        foreach (Predmet p in s.Predmet)
                        {
                            if (p.Id.Equals(idp))
                            {
                                if (p.Kolicina > Int32.Parse(kol_txt.Text))
                                {
                                    p.Kolicina -= Int32.Parse(kol_txt.Text);
                                    tacno = true;
                                    break;
                                }
                                else if (p.Kolicina == Int32.Parse(kol_txt.Text))
                                {
                                    s.RemovePredmet(p);
                                    tacno = true;
                                    break;
                                }
                                else
                                {
                                    tacno = false;
                                }
                            }
                        }

                    }
                }
                if (tacno)
                {
                    baza.KreirajBolnicu(b);
                }
                else
                {
                    MessageBox.Show("Nije odabrana odgovarajuća količina za prenos.");
                }
                this.Prikaz_Sobe(Soba1.SelectedItem.ToString(), 1);
                this.Prikaz_Sobe(Soba2.SelectedItem.ToString(), 2);
            }
            else {
                MessageBox.Show("Objekat nije selektovan");
            }
        }

        private void Soba1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Prikaz_Sobe(Soba1.SelectedItem.ToString(), 1);
        }

        public void Prikaz_Sobe(string id, int brojSobe) {
            BazaBolnica bazaBolnica = new BazaBolnica();
            List<Bolnica> bolnice = bazaBolnica.SveBolnice();
            Bolnica b = bolnice[0];
            BazaOpreme bazaO = new BazaOpreme();
            List<Predmet> predmeti = bazaO.SvaOprema();
            List<string> sadrzaj = new List<string>();
            foreach (Soba s in b.Soba)
            {
                if (s.Id.Equals(id))
                {
                    foreach (Predmet p in s.Predmet)
                    {
                        foreach (Predmet p1 in predmeti)
                        {
                            if (p.Id.Equals(p1.Id) && !p1.Obrisano)
                            {
                                string linija = "Id: " + p1.Id + " Naziv: " + p1.Naziv + " Kolicina: " + p.Kolicina;
                                sadrzaj.Add(linija);
                                break;
                            }
                        }
                    }
                    break;
                }

            }
            if (brojSobe == 1)
            {
                Lista1.ItemsSource = sadrzaj;
            }
            else {
                Lista2.ItemsSource = sadrzaj;
            }
        }
    }
}
