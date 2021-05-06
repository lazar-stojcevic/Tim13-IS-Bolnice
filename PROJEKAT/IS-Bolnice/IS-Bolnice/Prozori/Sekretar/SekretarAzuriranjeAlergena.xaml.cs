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

namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for SekretarAzuriranjeAlergena.xaml
    /// </summary>
    public partial class SekretarAzuriranjeAlergena : Window
    {
        private Pacijent pacijentRef;
        private BazaPacijenata bazaPacijenata;
        private BazaSastojaka bazaSastojaka;
        private ObservableCollection<string> MoguciAlergeniZaDodavanje;
        public ObservableCollection<Sastojak> AlergeniPacijenta
        {
            get;
            set;
        }

        public SekretarAzuriranjeAlergena(Pacijent pacijent)
        {
            InitializeComponent();
            bazaPacijenata = new BazaPacijenata();
            bazaSastojaka = new BazaSastojaka();
            pacijentRef = pacijent;

            this.DataContext = this;
            pacijentTxt.Text = pacijent.Ime + " " + pacijent.Prezime;

            AlergeniPacijenta = new ObservableCollection<Sastojak>(pacijent.Alergeni);
            MoguciAlergeniZaDodavanje = new ObservableCollection<string>();
            AzuriranjeMogucihAlergenaZaDodavanje();
        }

        private void AzuriranjeMogucihAlergenaZaDodavanje()
        {
            List<Sastojak> sviSastojci = bazaSastojaka.SviSastojci();
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
            // u ovom slucaju se nikada nece menjati jmbg pa je moguce staviti istu instancu za oba parametra
            bazaPacijenata.IzmeniPacijenta(pacijentRef, pacijentRef);
            Close();
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
