using Hoja_Calculo_IGUFinal.ViewModel;
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

namespace Hoja_Calculo_IGUFinal
{
    public partial class Tabla : Window
    {
        TablaViewModel tbvm;
        public Tabla(int flagSelectedIdex, TablaViewModel vm)
        {
            InitializeComponent();
            EscogerVentana(flagSelectedIdex);
            tbvm = vm;
            tablaPuntos.ItemsSource = tbvm.Modelo.listaPuntos;
        }
        public void EscogerVentana(int flagSelectedIndex)
        {
            if (flagSelectedIndex == 1)
            {
                this.Tabs.SelectedIndex = 0;
            }
            else
            {
                this.Tabs.SelectedIndex = 1;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            double pX = Double.Parse(textEjeX.Text);
            double pY = Double.Parse(textEjeY.Text);
            Point p = new Point(pX, pY);
            tbvm.AddPunto(p);
            textEjeX.Clear();
            textEjeY.Clear();
        }

        private void Elim_Click(object sender, RoutedEventArgs e)
        {
            if(tablaPuntos.SelectedIndex != -1)
            {
                int aElim = tablaPuntos.SelectedIndex;
                tbvm.RemovePunto(aElim);
            }
        }

        private void Mod_Click(object sender, RoutedEventArgs e)
        {
            if (tablaPuntos.SelectedIndex != -1)
            {
                string nX = textEjeX.Text;
                string nY = textEjeY.Text;
                int indice = tablaPuntos.SelectedIndex;
                tbvm.ModPunto(nX, nY, indice);
            }
        }

        private void TextEjes_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(textEjeX.Text.Length > 0 && textEjeY.Text.Length > 0)
            {
                Add.IsEnabled = true;
                Mod.IsEnabled = true;
            }
            else
            {
                Add.IsEnabled = false;
                Mod.IsEnabled = false;
            }
        }

        private void GrafPuntos_Click(object sender, RoutedEventArgs e)
        {
            tbvm.DibGraficaPuntos();
        }

        private void GrafBarras_Click(object sender, RoutedEventArgs e)
        {
            tbvm.DibGraficaBarras();
        }
    }
}
