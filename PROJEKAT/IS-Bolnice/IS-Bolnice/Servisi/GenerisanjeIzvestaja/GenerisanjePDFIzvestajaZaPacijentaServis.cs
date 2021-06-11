using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;

namespace IS_Bolnice.Servisi.GenerisanjeIzvestaja
{
    class GenerisanjePDFIzvestajaZaPacijentaServis : GenerisanjeIzvestajaZaPacijenta
    {
        protected override bool GenerisiIzvestajBuducihOperacijaPacijenta()
        {
            try
            {
                string odrediste = @"..\..\PDF\izvestaj.pdf";
                PdfWriter pdfWriter = new PdfWriter(odrediste);
                PdfDocument pdfDocument = new PdfDocument(pdfWriter);
                pdfDocument.AddNewPage();

                Document document = new Document(pdfDocument);
                PdfFont pdfFont = PdfFontFactory.CreateFont("C:/windows/fonts/arial.ttf", "Identity-H", PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
                document.SetFont(pdfFont);

                string naslov = "Lista svih operacije";
                document.Add(new iText.Layout.Element.Paragraph(naslov));
                document.Add(new iText.Layout.Element.Paragraph());

                float[] sirinaKolone = { 150F, 150F, 150F, 150F };
                iText.Layout.Element.Table tabela = new iText.Layout.Element.Table(sirinaKolone);
                tabela.AddCell("Ime lekara");
                tabela.AddCell("Prezime lekara");
                tabela.AddCell("Vreme pocetka");
                tabela.AddCell("Ordinacija");

                foreach (Operacija operacija in operacijePacijenta)
                {
                    tabela.AddCell(operacija.Lekar.Ime);
                    tabela.AddCell(operacija.Lekar.Prezime);
                    tabela.AddCell(operacija.VremePocetkaOperacije.ToString());
                    tabela.AddCell(operacija.Soba.Id);
                }

                document.Add(tabela.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER));
                document.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
