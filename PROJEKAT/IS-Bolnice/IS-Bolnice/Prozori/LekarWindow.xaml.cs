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
                }

            }
            if (!nasao) 
            { 
                MessageBox.Show("Ne postoji pacijent sa unetim jmbg-om", "Probaj ponovo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ButtonRaspored_Click(object sender, RoutedEventArgs e)
        {
            var raspored = new LekarRaspored();
            BazaOperacija operacije = new BazaOperacija();
            foreach (Operacija op in operacije.SveSledeceOperacije())
            if(op.Lekar.Jmbg.Equals(Sifra))
            {
                raspored.listaOperacija.Items.Add("Pacijent: "+op.Pacijent.Ime+" "+op.Pacijent.Prezime+" "+
                    op.Pacijent.Jmbg +" u prostoriji: "+op.Soba.Tip.ToString()+" "+ op.Soba.Id +" ( "+ op.VremePocetaOperacije.ToString("dd/MM/yyyy HH:mm")+
                    " - " + op.VremeKrajaOperacije.ToString("HH:mm") + " )");
            }
                BazaPregleda pregledi = new BazaPregleda();

            foreach (Pregled pr in pregledi.SviSledeciPregledi())
                if (pr.Lekar.Jmbg.Equals(Sifra))
                {
                raspored.listaPregleda.Items.Add("Pacijent: " + pr.Pacijent.Ime + " " + pr.Pacijent.Prezime + " " +
                    pr.Pacijent.Jmbg + " u prostoriji: " + RoomType.ordinacija.ToString() + " " + "FALI_ORDINACIJA_LEKARA" + " ( " + pr.VremePocetkaPregleda.ToString("dd/MM/yyyy HH:mm") +
                    " - " + pr.VremeKrajaPregleda.ToString("HH:mm") + " )");
            }

            raspored.ShowDialog();
        }

        private void odjavaClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow prijava = new MainWindow();
            prijava.ShowDialog();
            this.Close();

        }

        public string Sifra { get; set; }
    }




}
