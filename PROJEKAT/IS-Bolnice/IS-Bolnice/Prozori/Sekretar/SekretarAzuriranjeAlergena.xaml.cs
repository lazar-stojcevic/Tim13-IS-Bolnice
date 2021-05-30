using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using IS_Bolnice.Kontroleri;

namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for SekretarAzuriranjeAlergena.xaml
    /// </summary>
    public partial class SekretarAzuriranjeAlergena : Window
    {
        private Pacijent pacijentRef;
        private PacijentKontroler pacijentKontroler = new PacijentKontroler();
        private SastojakKontroler sastojakKontroler = new SastojakKontroler();
        private ObservableCollection<string> MoguciAlergeniZaDodavanje;
        public ObservableCollection<Sastojak> AlergeniPacijenta
        {
            get;
            set;
        }

        public SekretarAzuriranjeAlergena(Pacijent pacijent)
        {
            InitializeComponent();
             
            this.DataContext = this;
            pacijentRef = pacijent;
            AlergeniPacijenta = new ObservableCollection<Sastojak>(pacijent.Alergeni);
            MoguciAlergeniZaDodavanje = new ObservableCollection<string>();

            pacijentTxt.Text = pacijent.Ime + " " + pacijent.Prezime;
            AzuriranjeMogucihAlergenaZaDodavanje();
        }

        private void AzuriranjeMogucihAlergenaZaDodavanje()
        {
            List<Sastojak> sviSastojci = sastojakKontroler.GetSviSastojci();
            MoguciAlergeniZaDodavanje.Clear();
            foreach (Sastojak sastojak in sviSastojci)
            {
                if (!PacijentPosedujeAlergen(sastojak))
                {
                    MoguciAlergeniZaDodavanje.Add(sastojak.Ime);
                }
            }
            comboAlergeni.ItemsSource = MoguciAlergeniZaDodavanje;
        }

        private bool PacijentPosedujeAlergen(Sastojak sastojak)
        {
            foreach (Sastojak pacijentovAlergen in AlergeniPacijenta)
            {
                if (sastojak.Isti(pacijentovAlergen))
                {
                    return true;
                }
            }
            return false;
        }

        private void Button_Click_Dodaj(object sender, RoutedEventArgs e)
        {
            string imeSastojka = (string)comboAlergeni.SelectedItem;
            if (imeSastojka != null)
            {
                AlergeniPacijenta.Add(new Sastojak(imeSastojka));
            }
            comboAlergeni.SelectedIndex = -1;
            AzuriranjeMogucihAlergenaZaDodavanje();
        }

        private void Button_Click_Obrisi(object sender, RoutedEventArgs e)
        {
            int index = dgAlergeni.SelectedIndex;
            if (index != -1)
            {
                AlergeniPacijenta.RemoveAt(index);
            }
            comboAlergeni.SelectedIndex = -1;
            AzuriranjeMogucihAlergenaZaDodavanje();
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            pacijentRef.Alergeni = AlergeniPacijenta.ToList();
            pacijentKontroler.IzmeniPacijenta(pacijentRef);
            Close();
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
