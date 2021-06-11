using IS_Bolnice.Prozori.Sekretar;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi.GenerisanjeIzvestaja
{
    class GenerisanjePDFIzvestajaZaLekaraServis : GenerisanjeIzvestajaZaLekara
    {
        protected override bool GenerisiIzvestajZauzetostiLekara()
        {
            try
            {
                string dest = GetOdgovarajucaDestinacijaIzvestaja(lekar);
                PdfWriter writer = new PdfWriter(dest);
                PdfDocument pdfDoc = new PdfDocument(writer);
                pdfDoc.AddNewPage();
                Document document = new Document(pdfDoc);
                PdfFont utf8 = PdfFontFactory.CreateFont("C:/windows/fonts/arial.ttf", "Identity-H", PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
                document.SetFont(utf8);

                GenerisiNaslovIzvestaja(document);
                document.Add(new iText.Layout.Element.Paragraph());
                GenerisiDeoZaPreglede(document);
                document.Add(new iText.Layout.Element.Paragraph());
                if (!lekar.JelLekarOpstePrakse())
                {
                    GenerisiDeoZaOperacije(document);
                }
                document.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void GenerisiNaslovIzvestaja(Document document)
        {
            string naslov = "Izveštaj zauzetosti lekara " + lekar.Ime + " " + lekar.Prezime;
            document.Add(new iText.Layout.Element.Paragraph(naslov)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFontSize(20));

            string podnaslov = "Za period " + intervalIzvestaja.Pocetak.ToString(String.Format("dd.MM.yyyy")) + " - "
                               + intervalIzvestaja.Kraj.ToString(String.Format("dd.MM.yyyy"));
            document.Add(new iText.Layout.Element.Paragraph(podnaslov)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFontSize(15));
        }

        private void GenerisiDeoZaPreglede(Document document)
        {
            document.Add(new iText.Layout.Element.Paragraph("Pregledi lekara:"));
            float[] pointColumnWidths = { 150F, 150F, 150F, 150F };
            iText.Layout.Element.Table pdfTable = new iText.Layout.Element.Table(pointColumnWidths);
            pdfTable.AddCell("Ordinacija");
            pdfTable.AddCell("Vreme početka");
            pdfTable.AddCell("Vreme kraja");
            pdfTable.AddCell("Datum početka");
            foreach (Pregled pregled in preglediLekaraZaIzvestaj)
            {
                pdfTable.AddCell(pregled.Lekar.Ordinacija.Id);
                pdfTable.AddCell(pregled.VremePocetkaPregleda.ToString(String.Format("HH:mm")));
                pdfTable.AddCell(pregled.VremeKrajaPregleda.ToString(String.Format("HH:mm")));
                pdfTable.AddCell(pregled.VremePocetkaPregleda.ToString(String.Format("dd.MM.yyyy")));
            }
            document.Add(pdfTable
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER));
        }

        private void GenerisiDeoZaOperacije(Document document)
        {
            document.Add(new iText.Layout.Element.Paragraph("Operacije lekara:"));
            float[] pointColumnWidths2 = { 150F, 150F, 150F, 150F };
            iText.Layout.Element.Table pdfTableOperacije = new iText.Layout.Element.Table(pointColumnWidths2);
            pdfTableOperacije.AddCell("Sala za operaciju");
            pdfTableOperacije.AddCell("Vreme početka");
            pdfTableOperacije.AddCell("Vreme kraja");
            pdfTableOperacije.AddCell("Datum početka");
            foreach (Operacija operacija in operacijeLekaraZaIzvestaj)
            {
                pdfTableOperacije.AddCell(operacija.Soba.Id);
                pdfTableOperacije.AddCell(operacija.VremePocetkaOperacije.ToString(String.Format("HH:mm")));
                pdfTableOperacije.AddCell(operacija.VremeKrajaOperacije.ToString(String.Format("HH:mm")));
                pdfTableOperacije.AddCell(operacija.VremePocetkaOperacije.ToString(String.Format("dd.MM.yyyy")));
            }
            document.Add(pdfTableOperacije
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER));
        }

        private string GetOdgovarajucaDestinacijaIzvestaja(Lekar lekar)
        {
            if (lekar.JelLekarOpstePrakse())
                return @"..\..\..\..\izvestajZaLekaraOpstePrakse.pdf";
            else
                return @"..\..\..\..\izvestajZaLekaraSpecijalistu.pdf";
        }
    }
}
