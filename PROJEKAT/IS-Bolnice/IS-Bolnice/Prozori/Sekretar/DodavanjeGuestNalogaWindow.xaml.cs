using System.Windows;
using IS_Bolnice.Kontroleri;


namespace IS_Bolnice.Prozori.Sekretar
{
    public partial class DodavanjeGuestNalogaWindow : Window
    {
        private PacijentKontroler pacijentKontroler = new PacijentKontroler();

        public DodavanjeGuestNalogaWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            string tempIme = txtIme.Text;
            string tempPrezime = txtPrezime.Text;
            string tempJmbg = txtJMBG.Text;

            Pacijent noviGuestPacijent = new Pacijent(tempJmbg, tempIme, tempPrezime);
            noviGuestPacijent.Guest = true;
            pacijentKontroler.KreirajPacijenta(noviGuestPacijent);

            this.Close();
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
