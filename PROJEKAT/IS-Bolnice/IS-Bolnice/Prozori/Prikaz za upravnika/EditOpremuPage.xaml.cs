﻿using IS_Bolnice.Kontroleri;
using IS_Bolnice.Servisi;
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

namespace IS_Bolnice.Prozori.UpravnikPages
{
    /// <summary>
    /// Interaction logic for EditOpremuPage.xaml
    /// </summary>
    public partial class EditOpremuPage : Page
    {

        OpremaKontroler opremaKontroler = new OpremaKontroler();

        public EditOpremuPage(string selectedID)
        {
            InitializeComponent();
            id_txt.Text = selectedID;
            OpremaFajlRepozitorijum baza = new OpremaFajlRepozitorijum();
            Predmet izmenjenPredmet = baza.DobaviPoId(selectedID);
            naziv_txt.Text = izmenjenPredmet.Naziv;
            if (izmenjenPredmet.Tip == TipOpreme.dinamicka)
            {
                tip_opreme_txt.SelectedIndex = 0;
            }
            else {
                tip_opreme_txt.SelectedIndex = 1;
            }

        }

        private void Izmeni_btn_Click(object sender, RoutedEventArgs e)
        {
            Predmet izmenjenPredmet = opremaKontroler.DobaviPoId(id_txt.Text);
            izmenjenPredmet.Naziv = naziv_txt.Text;
            izmenjenPredmet.Tip = DobaviTip();
            opremaKontroler.IzmeniPredmet(izmenjenPredmet);
            Page upravljanje = new UpravljanjeOpremomPage();
            this.NavigationService.Navigate(upravljanje);
        }

        private TipOpreme DobaviTip() 
        {
            if (tip_opreme_txt.SelectedIndex == 1)
            {
                return TipOpreme.staticka;
            }
            else
            {
                return TipOpreme.dinamicka;
            }
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            Page upravljanje = new UpravljanjeOpremomPage();
            this.NavigationService.Navigate(upravljanje);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultat = MessageBox.Show("Da li ste sigurni da zelite da obrisete opremu?", "", MessageBoxButton.YesNo);
            if (resultat == MessageBoxResult.Yes)
            {
                opremaKontroler.ObrisiPredmet(id_txt.Text);
            }
            Page upravljanje = new UpravljanjeOpremomPage();
            this.NavigationService.Navigate(upravljanje);
        }
    }
}
