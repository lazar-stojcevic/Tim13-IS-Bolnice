using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarFrejm.xaml
    /// </summary>
    public partial class LekarFrejm : Window
    {
        private bool checker;
        public string Sifra { get; set; }

        public LekarFrejm(string jmbgLekara)
        {
            Checker = false;
            InitializeComponent();
            this.DataContext = this;
            Page glaviMeni = new LekarGlavniMeni(jmbgLekara);
            this.frame.NavigationService.Navigate(glaviMeni);
        }

        public bool Checker
        {
            get { return checker; }
            set
            {
                checker = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        }
    }
}
