﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Drawing;


namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarIzvestaj.xaml
    /// </summary>
    public partial class LekarIzvestaj : Window
    {

        ObservableCollection<Terapija> terapije = new ObservableCollection<Terapija>();
        string jmbgPac;
        string jmbgLek;
        public LekarIzvestaj(string jmbgPacijenta, string jmbgLekara)
        {
            InitializeComponent();
            listaLekova.ItemsSource = terapije;
            jmbgPac = jmbgPacijenta;
            jmbgLek = jmbgLekara;
        }

        private void Button_DodajLek(object sender, RoutedEventArgs e)
        {
            LekarDodavanjeLeka prozor = new LekarDodavanjeLeka(terapije, jmbgPac);
            prozor.Show();
        }

        private void Button_ObrisiLek(object sender, RoutedEventArgs e)
        {
            terapije.Remove((Terapija)listaLekova.SelectedItem);
        }

        private void Button_ZavrsiPregled(object sender, RoutedEventArgs e)
        {
            //Zapisivanje izvestaja u txt datoteku lekar#pacijent#anamneza#datumRodjenjaPacijenta#Lekovi sa terapijom

            SacuvajIzvestaj();

            if (terapije.Count != 0)
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;

                Pacijent p = new BazaPacijenata().PacijentSaOvimJMBG(jmbgPac);

                string imeBolnice = "Super bolnica";
                string imeIPrezimePacijenta = p.Ime + " " + p.Prezime;
                string datumRodjenjaPacijenta = p.DatumRodjenja.ToString("dd MM yyyy");
                string danasniDatum = DateTime.Now.ToString("dd MM yyyy");
                int brojRecepta = 0;

                foreach (Terapija ter in terapije)
                {
                    if (!ter.Lek.PotrebanRecept) { continue; }
                    KreirajReceptZaStampanje(imeBolnice, stringFormat, imeIPrezimePacijenta, datumRodjenjaPacijenta, danasniDatum, ter, ref brojRecepta);
                }
            }
            this.Close();

        }

        private void SacuvajIzvestaj()
        {
            string textIzvestaja = KreirajTextIzvestaja();

            BazaIzvestaja baza = new BazaIzvestaja();
            baza.KreirajIzvestaj(textIzvestaja);
        }

        private string KreirajTextIzvestaja()
        {
            string textIzvestaja;
            textIzvestaja = txtAnamneza.Text.Replace("\n", "%%%"); //Nisam siguran da li treba ova linija
            textIzvestaja = jmbgLek + "#" + jmbgPac + "#" + textIzvestaja + "#" + DateTime.Now.Date + "#";
            foreach (Terapija jednaTerapija in terapije)
            {
                textIzvestaja += GenerisanjeTekstaJedneTerapije(jednaTerapija);
            }

            textIzvestaja = textIzvestaja + Environment.NewLine;
            return textIzvestaja;
        }

        private static string GenerisanjeTekstaJedneTerapije(Terapija ter)
        {
            return ter.Lek.Sifra + "$$" + ter.Lek.Ime + "$$" + ter.Lek.Opis + "$$" +
                   ter.RazlikaNaKolikoSeDanaUzimaLek + "$$"
                   + ter.UcestanostKonzumiranja + "$$" + ter.VremePocetka + "$$" +
                   ter.VremeKraja + "$$" + ter.Opis + "$$$";
        }

        private void KreirajReceptZaStampanje(string imeBolnice, StringFormat stringFormat, string imePacijenta, string datumRodjenjaPacijenta,
            string danasniDatum, Terapija ter, ref int broj)
        {
            System.Drawing.Image bitmap = new Bitmap(@"..\..\Slike\recept.bmp");
            Graphics graphicsImage = Graphics.FromImage(bitmap);
            //ISCRTAVANJAE NA SLIKU RECEPTA
            IscrtavanjeImenaBolnice(imeBolnice, stringFormat, graphicsImage);

            IscrtavanjeImenaIPrezimenaPacijenta(stringFormat, imePacijenta, graphicsImage);

            IscrtavanjeDatumaRodjenjaPacijenta(stringFormat, datumRodjenjaPacijenta, graphicsImage);

            IcrtavanjeDanasnjegDatuma(stringFormat, danasniDatum, graphicsImage);

            IscrtavanjaIDLekara(stringFormat, graphicsImage);

            IcrtavanjeSifreLeka(stringFormat, ter, graphicsImage);

            IscrtavanjeKolikoSePutaDnevnoLekUzima(stringFormat, ter, graphicsImage);

            IspisNaReceptuNaKolikoDanaSeLekUzima(stringFormat, ter, graphicsImage);

            broj++;
            bitmap.Save(@"..\..\Recepti\noviRecept" + jmbgPac + "_(" + broj + ").bmp");
        }

        private static void IscrtavanjeKolikoSePutaDnevnoLekUzima(StringFormat stringFormat, Terapija ter,
            Graphics graphicsImage)
        {
            graphicsImage.DrawString(ter.UcestanostKonzumiranja + "puta na dan", new Font("arail", 10),
                new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(80, 360), stringFormat);
        }

        private static void IcrtavanjeSifreLeka(StringFormat stringFormat, Terapija ter, Graphics graphicsImage)
        {
            graphicsImage.DrawString(ter.Lek.Sifra, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.Point(30, 310), stringFormat);
        }

        private void IscrtavanjaIDLekara(StringFormat stringFormat, Graphics graphicsImage)
        {
            graphicsImage.DrawString(jmbgLek, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.Point(100, 280), stringFormat);
        }

        private static void IcrtavanjeDanasnjegDatuma(StringFormat stringFormat, string danasniDatum, Graphics graphicsImage)
        {
            graphicsImage.DrawString(danasniDatum, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.Point(110, 210), stringFormat);
        }

        private static void IscrtavanjeDatumaRodjenjaPacijenta(StringFormat stringFormat, string datumRodjenjaPacijenta,
            Graphics graphicsImage)
        {
            graphicsImage.DrawString(datumRodjenjaPacijenta, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.Point(30, 140), stringFormat);
        }

        private static void IscrtavanjeImenaIPrezimenaPacijenta(StringFormat stringFormat, string imePacijenta,
            Graphics graphicsImage)
        {
            graphicsImage.DrawString(imePacijenta, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.Point(30, 110), stringFormat);
        }

        private static void IscrtavanjeImenaBolnice(string imeBolnice, StringFormat stringFormat, Graphics graphicsImage)
        {
            graphicsImage.DrawString(imeBolnice, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                new System.Drawing.Point(30, 70), stringFormat);
        }

        private static void IspisNaReceptuNaKolikoDanaSeLekUzima(StringFormat stringFormat, Terapija ter,
            Graphics graphicsImage)
        {
            switch (ter.RazlikaNaKolikoSeDanaUzimaLek)
            {
                case 0:
                    graphicsImage.DrawString("Svaki dan", new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black),
                        new System.Drawing.Point(80, 410), stringFormat);
                    break;
                case 1:
                    graphicsImage.DrawString("Svaki drugi dan", new Font("arail", 12),
                        new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(80, 410), stringFormat);
                    break;
                case 2:
                    graphicsImage.DrawString("Svaki treći dan", new Font("arail", 12),
                        new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(80, 410), stringFormat);
                    break;
                case 3:
                    graphicsImage.DrawString("Svaki čevrti dan", new Font("arail", 12),
                        new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(80, 410), stringFormat);
                    break;
                case 4:
                    graphicsImage.DrawString("Svaki peti dan", new Font("arail", 12),
                        new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(80, 410), stringFormat);
                    break;
                case 5:
                    graphicsImage.DrawString("Svaki šesti dan", new Font("arail", 12),
                        new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(80, 410), stringFormat);
                    break;
                default:
                    graphicsImage.DrawString("Na svakih nedelju dana", new Font("arail", 12),
                        new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(80, 410), stringFormat);
                    break;
            }
        }
    }
}
