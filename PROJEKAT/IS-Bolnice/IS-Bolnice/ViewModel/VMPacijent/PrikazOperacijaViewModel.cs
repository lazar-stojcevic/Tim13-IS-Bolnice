using IS_Bolnice.Kontroleri;
using IS_Bolnice.Prozori.Prozori_za_pacijenta;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using System.Collections.Generic;
using System.Windows;

namespace IS_Bolnice.ViewModel.VMPacijent
{
    class PrikazOperacijaViewModel
    {
        private PrikazTerminaOperacija prikazTerminaOperacijaProzor;
        private OperacijaKontroler operacijaKontroler = new OperacijaKontroler();

        public PrikazOperacijaViewModel(PrikazTerminaOperacija prikazTerminaOperacija, string jmbgPacijenta)
        {
            this.prikazTerminaOperacijaProzor = prikazTerminaOperacija;

            OperacijePacijenta = operacijaKontroler.GetSveBuduceOperacijePacijenta(jmbgPacijenta);

            Izadji = new RelayCommand(IzvrsiIzadjiKomandu);
            Izvestaj = new RelayCommand(IzvrisIzvestajKomandu);
        }

        public List<Operacija> OperacijePacijenta { get; set; }

        public RelayCommand Izadji { get; set; }

        public RelayCommand Izvestaj { get; set; }

        public void IzvrsiIzadjiKomandu(object obj)
        {
            prikazTerminaOperacijaProzor.Close();
        }

        public void IzvrisIzvestajKomandu(object obj)
        {
            KreirajPDFIzvestajOSvimOperacijama();
        }

        public void KreirajPDFIzvestajOSvimOperacijama()
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

                foreach (Operacija operacija in OperacijePacijenta)
                {
                    tabela.AddCell(operacija.Lekar.Ime);
                    tabela.AddCell(operacija.Lekar.Prezime);
                    tabela.AddCell(operacija.VremePocetkaOperacije.ToString());
                    tabela.AddCell(operacija.Soba.Id);
                }

                document.Add(tabela.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER));
                document.Close();
                string message = "PDF je kreiran";
                MessageBox.Show(message);
            }
            catch
            {
                string message = "PDF nije kreiran";
                MessageBox.Show(message);
                return;
            }
        }
    }
}
