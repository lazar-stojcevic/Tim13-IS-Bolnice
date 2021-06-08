using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Model;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;

namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for PrikazZauzetostiLekaraOpstePrakse.xaml
    /// </summary>
    public partial class PrikazZauzetostiLekaraOpstePrakse : Window
    {
        private PregledKontroler pregledKontroler = new PregledKontroler();
        private Lekar selektovaniLekar;
        public ObservableCollection<Pregled> PreglediSelektovanogLekara
        {
            get;
            set;
        }
        public PrikazZauzetostiLekaraOpstePrakse(Lekar selektovaniLekar)
        {
            InitializeComponent();
            this.DataContext = this;

            this.selektovaniLekar = selektovaniLekar;
            txtLekar.Text = selektovaniLekar.Ime + " " + selektovaniLekar.Prezime;
            PreglediSelektovanogLekara =
                new ObservableCollection<Pregled>(pregledKontroler.GetSviBuduciPreglediLekara(selektovaniLekar.Jmbg));
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
                InformativniProzor ip = new InformativniProzor("Nevalidno unesen period za formranje izveštaja");
                ip.ShowDialog();
                return;
            }

            try
            {
                string dest = @"..\..\..\..\izvestajZaLekaraOpstePrakse.pdf";
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
                document.Close();

                InformativniProzor ip = new InformativniProzor("Uspešno generisan izveštaj!");
                ip.ShowDialog();
            }
            catch (Exception exception)
            {
                InformativniProzor ip = new InformativniProzor("Zatvorite fajl \"izvestajZaLekaraOpstePrakse\" da bi mogao ponovo da se generiše.");
                ip.ShowDialog();
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
    }
}
