using IS_Bolnice.Kontroleri;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
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

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for IzvestajPage2.xaml
    /// </summary>
    public partial class IzvestajPage2 : Page
    {
        List<Lekar> selektovaniLekari;
        PregledKontroler pregledKontroler = new PregledKontroler();
        OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
        LekarKontroler lekarKontroler = new LekarKontroler();
        public IzvestajPage2(List<Lekar> lekari)
        {
            InitializeComponent();
            selektovaniLekari = lekari;
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private bool ValidnoUnesenDatum(DateTime pocetak, DateTime kraj)
        {
            return pocetak <= kraj;
        }

        private void Potvrdi_btn_Click(object sender, RoutedEventArgs e)
        {

            DateTime pocetak = DateTime.Parse(datePicker_pocetak.SelectedDate.ToString());
            DateTime kraj = DateTime.Parse(datePicker_kraj.SelectedDate.ToString());
            if (!ValidnoUnesenDatum(pocetak, kraj))
            {
                MessageBox.Show("Period nije logicki validan!");
                return;
            }

            try
            {
                string dest = @"..\..\..\..\IzvestajZauzetostiLekara.pdf";
                PdfWriter writer = new PdfWriter(dest);
                PdfDocument pdfDoc = new PdfDocument(writer);
                pdfDoc.AddNewPage();
                Document document = new Document(pdfDoc);
                PdfFont utf8 = PdfFontFactory.CreateFont("C:/windows/fonts/arial.ttf", "Identity-H", PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
                document.SetFont(utf8);
                document.Add(new iText.Layout.Element.Paragraph("Izveštaj zauzetosti lekara")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetFontSize(26));
                string podnaslov = "Za period " + pocetak.ToString(String.Format("dd.MM.yyyy")) + " - "
                                       + kraj.ToString(String.Format("dd.MM.yyyy"));
                document.Add(new iText.Layout.Element.Paragraph(podnaslov)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(20));
                document.Add(new iText.Layout.Element.Paragraph("\n").SetFontSize(20));

                foreach (Lekar lekar in selektovaniLekari)
                {
                    document.Add(new iText.Layout.Element.Paragraph("\n\n").SetFontSize(20));
                    string naslov =  lekar.Ime + " " + lekar.Prezime;
                    document.Add(new iText.Layout.Element.Paragraph(naslov)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                        .SetFontSize(20));

                    document.Add(new iText.Layout.Element.Paragraph());
                    document.Add(new iText.Layout.Element.Paragraph("Pregledi lekara:"));
                    float[] pointColumnWidths = { 150F, 150F, 150F, 150F };
                    iText.Layout.Element.Table pdfTable = new iText.Layout.Element.Table(pointColumnWidths);
                    pdfTable.AddCell("Ordinacija");
                    pdfTable.AddCell("Vreme početka");
                    pdfTable.AddCell("Vreme kraja");
                    pdfTable.AddCell("Datum početka");
                    foreach (Pregled pregled in SviPreglediLekara(lekar.Id))
                    {
                        pdfTable.AddCell(pregled.Lekar.Ordinacija.Id);
                        pdfTable.AddCell(pregled.VremePocetkaPregleda.ToString(String.Format("HH:mm")));
                        pdfTable.AddCell(pregled.VremeKrajaPregleda.ToString(String.Format("HH:mm")));
                        pdfTable.AddCell(pregled.VremePocetkaPregleda.ToString(String.Format("dd.MM.yyyy")));
                    }
                    document.Add(pdfTable
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER));
                    if (!lekar.Oblast.JelOpstaPraksa())
                    {
                        document.Add(new iText.Layout.Element.Paragraph());
                        document.Add(new iText.Layout.Element.Paragraph("Operacije lekara:"));
                        float[] pointColumnWidths2 = { 150F, 150F, 150F, 150F };
                        iText.Layout.Element.Table pdfTableOperacije = new iText.Layout.Element.Table(pointColumnWidths2);
                        pdfTableOperacije.AddCell("Sala za operaciju");
                        pdfTableOperacije.AddCell("Vreme početka");
                        pdfTableOperacije.AddCell("Vreme kraja");
                        pdfTableOperacije.AddCell("Datum početka");
                        foreach (Operacija operacija in SveOperacijeLekara(lekar.Id))
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
                    document.Add(new iText.Layout.Element.Paragraph());
                }
                document.Close();
                MessageBox.Show("Generisanje uspešno!");
            }
            catch (Exception exception)
            {
                MessageBox.Show("Zatvorite IzvestajZauzetostiLekara.pdf!");
                return;
            }
        }

        private List<Pregled> SviPreglediLekara(string idLekara)
        {
            DateTime pocetak = DateTime.Parse(datePicker_pocetak.SelectedDate.ToString());
            DateTime kraj = DateTime.Parse(datePicker_kraj.SelectedDate.ToString());
            List<Pregled> sviPregledi = pregledKontroler.GetSviPreglediLekara(idLekara);
            List<Pregled> odabraniPregledi = new List<Pregled>();
            foreach (Pregled pregledIter in sviPregledi)
            {
                if (pregledIter.VremePocetkaPregleda > pocetak && pregledIter.VremePocetkaPregleda < kraj)
                {
                    odabraniPregledi.Add(pregledIter);
                }
            }
            return odabraniPregledi;
        }

        private List<Operacija> SveOperacijeLekara(string idLekara)
        {
            DateTime pocetak = DateTime.Parse(datePicker_pocetak.SelectedDate.ToString());
            DateTime kraj = DateTime.Parse(datePicker_kraj.SelectedDate.ToString());
            List<Operacija> sveOperacije = operacijaKontroler.GetSveOperacijeLekara(idLekara);
            List<Operacija> odabraneOperacije = new List<Operacija>();
            foreach (Operacija operacijaIter in sveOperacije)
            {
                if (operacijaIter.VremePocetkaOperacije > pocetak && operacijaIter.VremePocetkaOperacije < kraj)
                {
                    odabraneOperacije.Add(operacijaIter);
                }
            }
            return odabraneOperacije;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
