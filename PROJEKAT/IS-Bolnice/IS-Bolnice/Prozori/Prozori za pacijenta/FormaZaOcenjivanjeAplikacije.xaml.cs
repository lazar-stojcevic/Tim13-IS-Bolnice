using IS_Bolnice.Kontroleri;
using IS_Bolnice.Model;
using System.Text.RegularExpressions;
using System.Windows;
using IS_Bolnice.Kontroleri.Informativni;


namespace IS_Bolnice.Prozori.Prozori_za_pacijenta
{
    public partial class FormaZaOcenjivanjeAplikacije : Window
    {
        RecenzijaKontroler recenzijaKontroler = new RecenzijaKontroler();

        public FormaZaOcenjivanjeAplikacije()
        {
            InitializeComponent();
        }

        private void potvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (DaLiJeTekstVaidan())
            {
                Recenzija recenzija = new Recenzija();
                recenzija.Ocena = KojaOcenaJeSelektovana();
                recenzija.Opis = komentar.Text;
                recenzijaKontroler.KreirajRecenziju(recenzija);
                this.Close();
            }
        }

        private void odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool DaLiJeTekstVaidan()
        {
            string strRegex = "[#]+";
            Regex re = new Regex(strRegex);

            if (re.IsMatch(komentar.Text))
            {
                string message = "Molimo vas da obrišete karakter - tarabu";
                MessageBox.Show(message);
                return false;
            }

            return true;
        }

        private int KojaOcenaJeSelektovana()
        {
            if (btn1.IsChecked.Value)
            {
                return 1;
            }
            else if (btn2.IsChecked.Value)
            {
                return 2;
            }
            else if (btn3.IsChecked.Value)
            {
                return 3;
            }
            else if (btn4.IsChecked.Value)
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }
    }
}
