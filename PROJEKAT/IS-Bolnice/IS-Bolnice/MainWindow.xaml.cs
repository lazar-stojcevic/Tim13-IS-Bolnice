using IS_Bolnice.Kontroleri;
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
using IS_Bolnice.Repozitorijumi.Interfejsi;
using IS_Bolnice.DTOs;
using IS_Bolnice.Kontroleri.Ustanova;
using IS_Bolnice.Kontroleri.Korisnicki;

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

            LoggerKontroler kontrolerLogIn = new LoggerKontroler();
            PacijentKontroler pacijentKontroler = new PacijentKontroler();
            LogInDTO logIn = kontrolerLogIn.GetKorisnika(korisnik, sifra);
            if (logIn == null)
            {
                MessageBox.Show("Podaci nisu ispravno uneseni");
            }
            else
            {

                switch (logIn.TipKorisnika)
                {
                    case "P":
                        PacijentWindow pacijent = new PacijentWindow();
                        Pacijent p = pacijentKontroler.GetPacijentSaOvimJMBG(logIn.Jmbg);
                        pacijent.imeKorisnika.Text = p.Ime + " " + p.Prezime + " " + p.Jmbg;
                        pacijent.Show();
                        this.Close();
                        break;
                    case "U":
                        UpravnikWindow upravnik = new UpravnikWindow(logIn.Jmbg);
                        upravnik.Show();
                        this.Close();
                        break;
                    case "L":
                        Prozori.Prikaz_kod_lekara.LekarFrejm lekar =
                            new Prozori.Prikaz_kod_lekara.LekarFrejm(logIn.Jmbg);
                        lekar.Show();
                        this.Close();
                        break;
                    default:
                        SekretarKontroler sekretarKontroler = new SekretarKontroler();
                        Sekretar sekretar = sekretarKontroler.GetByJmbg(logIn.Jmbg);
                        Prozori.Sekretar.SekretarWindow sekretarWindow =
                            new Prozori.Sekretar.SekretarWindow(sekretar);
                        sekretarWindow.Show();
                        this.Close();
                        break;
                }
            }

        }

        private void btnClose_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
