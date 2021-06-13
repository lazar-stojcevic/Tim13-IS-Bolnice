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
    /// Interaction logic for SlobodniZauzetiTerminiGraf.xaml
    /// </summary>
    public partial class SlobodniZauzetiTerminiGraf : UserControl
    {

        PregledKontroler pregledKontrolet = new PregledKontroler();
        OperacijaKontroler operacijaKontroler = new OperacijaKontroler();

        public SlobodniZauzetiTerminiGraf(string idLekara)
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Broj pregleda",
                    Values = new ChartValues<int>(pregledKontrolet.BrojPrijemaZaPetDana(idLekara))
                },
                new ColumnSeries
                {
                    Title = "Broj operacija",
                    Values = new ChartValues<int>(operacijaKontroler.BrojPrijemaZaPetDana(idLekara))
                }
            };

            Labels = LabelsZaDatume();
            Formatter = value => value.ToString();

            DataContext = this;
        }

        private string[] LabelsZaDatume()
        {
            string[] datumi = new string[5];
            for (int i = 0; i < 5; i++) {
                datumi[i] = DateTime.Now.AddDays(i).ToString("dd/MM");
            }
            return datumi;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
    }
}
