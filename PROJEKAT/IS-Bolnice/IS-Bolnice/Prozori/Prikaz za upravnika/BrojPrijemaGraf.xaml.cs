using IS_Bolnice.Kontroleri;
using LiveCharts;
using LiveCharts.Wpf;
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
using IS_Bolnice.Kontroleri.Termini;

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for BrojPrijemaGraf.xaml
    /// </summary>
    public partial class BrojPrijemaGraf : UserControl
    {
        PregledKontroler pregledKontrolet = new PregledKontroler();
        OperacijaKontroler operacijaKontroler = new OperacijaKontroler();

        public BrojPrijemaGraf(string idLekara)
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Pregledi",
                    Values = new ChartValues<int>(pregledKontrolet.BrojPregledaKodLekaraZaPetMeseci(idLekara))
                },
                new LineSeries
                {
                    Title = "Operacije",
                    Values = new ChartValues<int>(operacijaKontroler.BrojOperacijaKodLekaraZaPetMeseci(idLekara)),
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Pregledi u celoj bolnici",
                    Values = new ChartValues<int> (pregledKontrolet.BrojPregledaZaPetMeseci()),
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                },
                new LineSeries
                {
                    Title = "Operacije u celoj bolnici",
                    Values = new ChartValues<int> (operacijaKontroler.BrojOperacijaZaPetMeseci()),
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };

            Labels = GetMeseciString();
            YFormatter = value => value.ToString();

            DataContext = this;
        }

        private string[] GetMeseciString()
        {
            string[] meseci = new string[5];
            for (int i = 0; i < 5; i++) {
                if (DateTime.Now.Month - i > 0)
                {
                    meseci[4-i] = (DateTime.Now.Month - i).ToString();
                }
                else {
                    meseci[4-i] = (DateTime.Now.Month - i + 12).ToString();
                }
            }
            return meseci;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
    }
}
