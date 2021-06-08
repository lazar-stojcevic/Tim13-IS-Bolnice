using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IS_Bolnice.DTOs;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.ViewModel.Lekar;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    public partial class ZakazivanjeOperacije : Page
    {
        private string jmbgPac = "";
        public ZakazivanjeOperacije(string jmbgPacijenta)
        {
            InitializeComponent();
            jmbgPac = jmbgPacijenta;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ZakazivanjeOperacijeViewModel(this.NavigationService, jmbgPac);
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 1;
        }

        private void ToggleButton_OnUnchecked_UnChecked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 0;
        }
       
    }
        
}
