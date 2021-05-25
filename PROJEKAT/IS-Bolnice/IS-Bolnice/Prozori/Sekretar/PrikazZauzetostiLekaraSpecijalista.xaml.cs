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
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Model;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for PrikazZauzetostiLekaraSpecijalista.xaml
    /// </summary>
    public partial class PrikazZauzetostiLekaraSpecijalista : Window
    {
        private PregledKontroler pregledKontroler = new PregledKontroler();
        private OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
        private Lekar selektovaniLekar;
        public ObservableCollection<Pregled> PreglediSelektovanogLekara
        {
            get;
            set;
        }

        public ObservableCollection<Operacija> OperacijeSelektovanogLekara
        {
            get;
            set;
        }
        public PrikazZauzetostiLekaraSpecijalista(Lekar selektovaniLekar)
        {
            InitializeComponent();
            this.DataContext = this;

            this.selektovaniLekar = selektovaniLekar;
            txtLekar.Text = selektovaniLekar.Ime + " " + selektovaniLekar.Prezime;

            PreglediSelektovanogLekara =
                new ObservableCollection<Pregled>(pregledKontroler.GetSviBuduciPreglediLekara(selektovaniLekar.Jmbg));
            OperacijeSelektovanogLekara =
                new ObservableCollection<Operacija>(
                    operacijaKontroler.GetSveSledeceOperacijeLekara(selektovaniLekar.Jmbg));
        }
        private bool ValidnoUnesenDatum(DateTime pocetak, DateTime kraj)
        {
            return pocetak <= kraj;
        }

        private void Button_Click_Generisanje_PDF_Izvestaja(object sender, RoutedEventArgs e)
        {
            DateTime pocetak = DateTime.Parse(datePicker_pocetak.SelectedDate.ToString());
            DateTime kraj = DateTime.Parse(datePicker_kraj.SelectedDate.ToString());
            if (!ValidnoUnesenDatum(pocetak, kraj))
            {
                MessageBox.Show("Nevalidno unesen period za formranje izveštaja");
                return;
            }

            try
            {
                string dest = @"..\..\..\..\izvestajZaLekaraSpecijalistu.pdf";
                PdfWriter writer = new PdfWriter(dest);
                PdfDocument pdfDoc = new PdfDocument(writer);
                pdfDoc.AddNewPage();
                Document document = new Document(pdfDoc);
                PdfFont utf8 = PdfFontFactory.CreateFont("C:/windows/fonts/arial.ttf", "Identity-H", PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
                document.SetFont(utf8);

                string naslov = "Izveštaj zauzetosti lekara " + selektovaniLekar.Ime + " " + selektovaniLekar.Prezime;
                document.Add(new iText.Layout.Element.Paragraph(naslov)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(20));

                string podnaslov = "Za period " + pocetak.ToString(String.Format("dd.MM.yyyy")) + " - "
                                   + kraj.ToString(String.Format("dd.MM.yyyy"));
                document.Add(new iText.Layout.Element.Paragraph(podnaslov)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(15));

                document.Add(new iText.Layout.Element.Paragraph());
                document.Add(new iText.Layout.Element.Paragraph("Pregledi lekara:"));
                float[] pointColumnWidths = { 150F, 150F, 150F, 150F };
                iText.Layout.Element.Table pdfTable = new iText.Layout.Element.Table(pointColumnWidths);
                pdfTable.AddCell("Ordinacija");
                pdfTable.AddCell("Vreme početka");
                pdfTable.AddCell("Vreme kraja");
                pdfTable.AddCell("Datum početka");
                foreach (Pregled pregled in SviPreglediLekaraUZadatomPeriodu())
                {
                    pdfTable.AddCell(pregled.Lekar.Ordinacija.Id);
                    pdfTable.AddCell(pregled.VremePocetkaPregleda.ToString(String.Format("HH:mm")));
                    pdfTable.AddCell(pregled.VremeKrajaPregleda.ToString(String.Format("HH:mm")));
                    pdfTable.AddCell(pregled.VremePocetkaPregleda.ToString(String.Format("dd.MM.yyyy")));
                }
                document.Add(pdfTable
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER));

                document.Add(new iText.Layout.Element.Paragraph());
                document.Add(new iText.Layout.Element.Paragraph("Operacije lekara:"));
                float[] pointColumnWidths2 = { 150F, 150F, 150F, 150F };
                iText.Layout.Element.Table pdfTableOperacije = new iText.Layout.Element.Table(pointColumnWidths2);
                pdfTableOperacije.AddCell("Sala za operaciju");
                pdfTableOperacije.AddCell("Vreme početka");
                pdfTableOperacije.AddCell("Vreme kraja");
                pdfTableOperacije.AddCell("Datum početka");
                foreach (Operacija operacija in SveOperacijeLekaraUZadatomPeriodu())
                {
                    pdfTableOperacije.AddCell(operacija.Soba.Id);
                    pdfTableOperacije.AddCell(operacija.VremePocetkaOperacije.ToString(String.Format("HH:mm")));
                    pdfTableOperacije.AddCell(operacija.VremeKrajaOperacije.ToString(String.Format("HH:mm")));
                    pdfTableOperacije.AddCell(operacija.VremePocetkaOperacije.ToString(String.Format("dd.MM.yyyy")));
                }
                document.Add(pdfTableOperacije
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER));
                document.Close();

                MessageBox.Show("Uspešno generisan izveštaj!");
            }
            catch (Exception exception)
            {
                MessageBox.Show("Zatvorite fajl \"izvestajZaLekaraSpecijalistu\" da bi mogao ponovo da se generiše.");
                return;
            }
        }

        private List<Pregled> SviPreglediLekaraUZadatomPeriodu()
        {
            DateTime pocetak = DateTime.Parse(datePicker_pocetak.SelectedDate.ToString());
            DateTime kraj = DateTime.Parse(datePicker_kraj.SelectedDate.ToString());
            kraj = kraj.AddHours(24);   // da bi upali i termini tog dana

            List<Pregled> sviPreglediLekaraUPeriodu = new List<Pregled>();
            if (!ValidnoUnesenDatum(pocetak, kraj))
            {
                return sviPreglediLekaraUPeriodu;
            }

            VremenskiInterval zadatInterval = new VremenskiInterval(pocetak, kraj);
            foreach (Pregled pregled in pregledKontroler.GetSviPreglediLekara(selektovaniLekar.Jmbg))
            {
                if (zadatInterval.Pocetak <= pregled.VremePocetkaPregleda &&
                    zadatInterval.Kraj >= pregled.VremeKrajaPregleda)
                {
                    sviPreglediLekaraUPeriodu.Add(pregled);
                }
            }

            return sviPreglediLekaraUPeriodu;
        }

        private List<Operacija> SveOperacijeLekaraUZadatomPeriodu()
        {
            DateTime pocetak = DateTime.Parse(datePicker_pocetak.SelectedDate.ToString());
            DateTime kraj = DateTime.Parse(datePicker_kraj.SelectedDate.ToString());
            kraj = kraj.AddHours(24);   // da bi upali i termini tog dana

            List<Operacija> sveOperacijeLekaraUPeriodu = new List<Operacija>();
            if (!ValidnoUnesenDatum(pocetak, kraj))
            {
                return sveOperacijeLekaraUPeriodu;
            }

            VremenskiInterval zadatInterval = new VremenskiInterval(pocetak, kraj);
            foreach (Operacija operacija in operacijaKontroler.GetSveOperacijeLekara(selektovaniLekar.Jmbg))
            {
                if (zadatInterval.Pocetak <= operacija.VremePocetkaOperacije &&
                    zadatInterval.Kraj >= operacija.VremeKrajaOperacije)
                {
                    sveOperacijeLekaraUPeriodu.Add(operacija);
                }
            }

            return sveOperacijeLekaraUPeriodu;
        }
    }
}
