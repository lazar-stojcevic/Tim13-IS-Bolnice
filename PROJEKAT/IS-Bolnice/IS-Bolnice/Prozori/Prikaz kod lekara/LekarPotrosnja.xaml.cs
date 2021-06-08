using IS_Bolnice.Kontroleri;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using WPFCustomMessageBox;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Paragraph = iTextSharp.text.Paragraph;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarPotrosnja.xaml
    /// </summary>
    public partial class LekarPotrosnja : Page
    {
        private List<SadrzajSobe> predmetiUSobi = new List<SadrzajSobe>();
        private BolnicaKontroler bolnicaKontroler = new BolnicaKontroler();
        private SadrzajSobeKontroler sadrzajSobeKontroler = new SadrzajSobeKontroler();
        public LekarPotrosnja()
        {
            InitializeComponent();

            List<Soba> sveOperacioneSale = bolnicaKontroler.GetSveSobe();
            prostorije.ItemsSource = sveOperacioneSale;


        }

        private void promenaSelekcije(object sender, SelectionChangedEventArgs e)
        {
            Soba selektovana = (Soba)prostorije.SelectedItem;
            predmetiUSobi = sadrzajSobeKontroler.GetDnamickiSadrzajSobe(selektovana.Id);

            oprema.ItemsSource = predmetiUSobi;


        }

        private void Button_ClickNazad(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (oprema.SelectedIndex != -1)
            {
                predmetiUSobi.ElementAt(oprema.SelectedIndex).Kolicina = predmetiUSobi.ElementAt(oprema.SelectedIndex).Kolicina + 1;
                List<SadrzajSobe> lista = new List<SadrzajSobe>();
                foreach (SadrzajSobe sadrzaj in predmetiUSobi)
                {
                    lista.Add(sadrzaj);   
                }
                oprema.ItemsSource = lista;
            }
            else
            {
                CustomMessageBox.ShowOK("Nije selektovana ni jedna oprema", "Greška", "Dobro",
                    MessageBoxImage.Error);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (oprema.SelectedIndex != -1)
            {
                if (predmetiUSobi.ElementAt(oprema.SelectedIndex).Kolicina > 0)
                {
                    predmetiUSobi.ElementAt(oprema.SelectedIndex).Kolicina =
                        predmetiUSobi.ElementAt(oprema.SelectedIndex).Kolicina - 1;
                    List<SadrzajSobe> lista = new List<SadrzajSobe>();
                    foreach (SadrzajSobe sadrzaj in predmetiUSobi)
                    {
                        lista.Add(sadrzaj);
                    }
                    oprema.ItemsSource = lista;
                }
                else
                {
                    CustomMessageBox.ShowOK("Ne postoji više ove opreme u prostoriji", "Greška", "Dobro",
                        MessageBoxImage.Error);
                }
            }
            else
            {
                CustomMessageBox.ShowOK("Nije selektovana ni jedna oprema", "Greška", "Dobro",
                    MessageBoxImage.Error);
            }
        }

        private void Button_Zavrsi(object sender, RoutedEventArgs e)
        {
            CustomMessageBox.ShowOK("Uspešno ste izmenili sadržaj prostojire", "Izmena sadržaja prostorije", "Dobro",
                MessageBoxImage.Information);
            NavigationService.GoBack();
        }

        private void Button_Izvestaj(object sender, RoutedEventArgs e)
        {
            try
            {

                Document pdoc = new Document(PageSize.A4, 20, 20, 30, 30);
                PdfWriter pWriter =
                    PdfWriter.GetInstance(pdoc, new FileStream(@"..\..\Datoteke\Izvestaj.pdf", FileMode.Create));
                pdoc.Open();

                var razmak = new Paragraph("")
                {
                    SpacingBefore = 10f,
                    SpacingAfter = 10f
                };

                var tableHeader = new PdfPTable(new[] {1.2f, 2f})
                {
                    HorizontalAlignment = (int)HorizontalAlignment.Left,
                    WidthPercentage = 75,
                    DefaultCell = { MinimumHeight = 22f}
                };

                tableHeader.AddCell("Datum kreiranja");
                tableHeader.AddCell(DateTime.Now.ToString());

                tableHeader.AddCell("Bolnica:");
                tableHeader.AddCell("Bolnica Zdravo");

                tableHeader.AddCell("Vremenski period za koji je kreiran izvestaj");
                if ((bool)mesecDana.IsChecked)
                    tableHeader.AddCell(DateTime.Now.AddMonths(-1).ToString() + " - " + DateTime.Now.ToString());
                if ((bool)nedeljuDana.IsChecked)
                    tableHeader.AddCell(DateTime.Now.AddDays(-7).ToString() + " - " + DateTime.Now.ToString());
                if ((bool)godinuDana.IsChecked)
                    tableHeader.AddCell(DateTime.Now.AddYears(-1).ToString() + " - " + DateTime.Now.ToString());
                pdoc.Add(tableHeader);
                pdoc.Add(razmak);

                IzvestajKontroler izvestajKontroler = new IzvestajKontroler();
                List<Izvestaj> sviIzvetaji = new List<Izvestaj>();
                if ((bool)mesecDana.IsChecked)
                    sviIzvetaji = izvestajKontroler.DobaviSveIzvestajeizPoslednjihMesecDana();
                if ((bool)nedeljuDana.IsChecked)
                    sviIzvetaji = izvestajKontroler.DobaviSveIzvestajeizPoslednjihNedeljuDana();
                if ((bool)godinuDana.IsChecked)
                    sviIzvetaji = izvestajKontroler.DobaviSveIzvestajeizPoslednjihGodinuDana();


                LekKontroler lekKontroler = new LekKontroler();
                List<Lek> sviLekovi = lekKontroler.GetSviLekovi();

                IDictionary<String, int> lekovi = new Dictionary<string, int>();
                foreach (Lek lek in sviLekovi)
                {
                    lekovi.Add(lek.Id, 0);
                }

                foreach (Izvestaj izvestaj in sviIzvetaji)
                {
                    foreach (Terapija terapija in izvestaj.Terapija)
                    {
                        if (lekovi.ContainsKey(terapija.Lek.Id))
                        {
                            lekovi[terapija.Lek.Id] = lekovi[terapija.Lek.Id] + 1;
                        }
                    }
                    
                }


                var tabelaLekova = new PdfPTable(new[] { 1.75f, 2f, 0.5f })
                {
                    HorizontalAlignment = (int)HorizontalAlignment.Left,
                    WidthPercentage = 100,
                    DefaultCell = { MinimumHeight = 22f }
                };

                foreach (KeyValuePair<string, int> iterLek in lekovi)
                {
                    Lek lek = lekKontroler.DobaviLekPoId(iterLek.Key);
                    tabelaLekova.AddCell(lek.Ime);
                    tabelaLekova.AddCell(lek.Opis);
                    tabelaLekova.AddCell(iterLek.Value.ToString());
                }

                pdoc.Add(tabelaLekova);
                pdoc.Add(razmak);

                pdoc.Close();

                CustomMessageBox.Show("Uspesno je generisan izvestaj", "Generisan izvestaj", MessageBoxButton.OK,
                    MessageBoxImage.Information);

            }
            catch
            {

            }

        }
    }
}
