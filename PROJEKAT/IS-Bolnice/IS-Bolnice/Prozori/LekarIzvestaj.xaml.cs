using System;
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


namespace IS_Bolnice.Prozori
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
            //Zapisivanje izvestaja u txt datoteku lekar#pacijent#anamneza#datum#Lekovi sa terapijom
            string textIzvestaja = txtAnamneza.Text.Replace("\n", "%%%"); //Nisam siguran da li treba ova linija
            textIzvestaja =jmbgLek +"#"+ jmbgPac + "#" + textIzvestaja + "#" + DateTime.Now.Date + "#";
            foreach (Terapija ter in terapije)
            {
                textIzvestaja = textIzvestaja + ter.Lek.Sifra + "$$" + ter.Lek.Ime + "$$" + ter.Lek.Opis + "$$" + ter.RazlikaNaKolikoSeDanaUzimaLek.ToString() + "$$"
                    +ter.UcestanostKonzumiranja.ToString() + "$$" + ter.VremePocetka.ToString() + "$$" + ter.VremeKraja.ToString() + "$$" + ter.Detalji + "$$$";
            }
            BazaIzvestaja baza = new BazaIzvestaja();
            textIzvestaja = textIzvestaja + System.Environment.NewLine;
            baza.KreirajIzvestaj(textIzvestaja);
            ///////////////////////////////////////////////////////////////
            // GENERISANJE RECEPTA, OVAJ PUT STVARNO
            if (terapije.Count != 0)
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;

                Pacijent p = new BazaPacijenata().PacijentSaOvimJMBG(jmbgPac);

                string text1 = "Super bolnica";
                string text2 = p.Ime + " " + p.Prezime;
                string datum = p.DatumRodjenja.ToString("dd MM yyyy");
                string danasniDatum = System.DateTime.Now.ToString("dd MM yyyy");
                int broj = 0;
                foreach (Terapija ter in terapije)
                {
                    if (!ter.Lek.PotrebanRecept) { continue; }
                    System.Drawing.Image bitmap = new Bitmap(@"..\..\Slike\recept.bmp");
                    Graphics graphicsImage = Graphics.FromImage(bitmap);
                    //ISCRTAVANJAE NA SLIKU RECEPTA
                    graphicsImage.DrawString(text1, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(30, 70), stringFormat);
                    graphicsImage.DrawString(text2, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(30, 110), stringFormat);
                    graphicsImage.DrawString(datum, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(30, 140), stringFormat);
                    graphicsImage.DrawString(danasniDatum, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(110, 210), stringFormat);
                    graphicsImage.DrawString(jmbgLek, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(100, 280), stringFormat);
                    graphicsImage.DrawString(ter.Lek.Sifra, new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(30, 310), stringFormat);
                    graphicsImage.DrawString(ter.UcestanostKonzumiranja.ToString() + "puta na dan", new Font("arail", 10),
                        new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(80, 360), stringFormat);
                    switch (ter.RazlikaNaKolikoSeDanaUzimaLek)
                    {
                        case 0: 
                            graphicsImage.DrawString("Svaki dan", new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(80, 410), stringFormat);
                            break;
                        case 1:
                            graphicsImage.DrawString("Svaki drugi dan", new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(80, 410), stringFormat);
                            break;
                        case 2:
                            graphicsImage.DrawString("Svaki treći dan", new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(80, 410), stringFormat);
                            break;
                        case 3:
                            graphicsImage.DrawString("Svaki čevrti dan", new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(80, 410), stringFormat);
                            break;
                        case 4:
                            graphicsImage.DrawString("Svaki peti dan", new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(80, 410), stringFormat);
                            break;
                        case 5:
                            graphicsImage.DrawString("Svaki šesti dan", new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(80, 410), stringFormat);
                            break;
                        default:
                            graphicsImage.DrawString("Na svakih nedelju dana", new Font("arail", 12), new SolidBrush(System.Drawing.Color.Black), new System.Drawing.Point(80, 410), stringFormat);
                            break;
                    }

                    broj++;
                    bitmap.Save(@"..\..\Recepti\noviRecept" +jmbgPac +"_("+broj+ ").bmp");
                }
            }
            this.Close();

        }
    }
}
