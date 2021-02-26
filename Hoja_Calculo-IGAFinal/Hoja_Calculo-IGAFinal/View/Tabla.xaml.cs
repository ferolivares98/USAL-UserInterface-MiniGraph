using Hoja_Calculo_IGUFinal.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        private void BotonGenerarPoli_Click(object sender, RoutedEventArgs e)
        {
            double grado, rangoPrimero, rangoSegundo;
            double pY;
            double varInd, gradoUno, gradoDos, gradoTres;
            Point p;
            ObservableCollection<Point> lista = new ObservableCollection<Point>();

            grado = Double.Parse(textGradoPolinomio.Text);
            rangoPrimero = Double.Parse(textRangoPrimero.Text);
            rangoSegundo = Double.Parse(textRangoSegundo.Text);
            varInd = Double.Parse(textVarInd.Text);
            gradoUno = Double.Parse(textGradoUno.Text);

            for (double i = rangoPrimero; i <= rangoSegundo; i++)
            {
                pY = 0;
                if(grado == 3)
                {
                    gradoTres = Double.Parse(textGradoTres.Text);
                    pY = pY + (gradoTres * i * i * i);
                }
                if(grado >= 2)
                {
                    gradoDos = Double.Parse(textGradoDos.Text);
                    pY = pY + (gradoDos * i * i);
                }
                pY = pY + (gradoUno * i) + varInd;

                p = new Point(i, pY);
                lista.Add(p);
            }
            tbvm.GenerarPolinomio(lista);
            tablaPuntos.ItemsSource = tbvm.Modelo.listaPuntos; //Mejor manera?
            this.Tabs.SelectedIndex = 0;
        }

        private void TextGradoPolinomio_TextChanged(object sender, TextChangedEventArgs e)
        {
            textVarInd.BorderBrush = Brushes.Black;
            textVarInd.IsEnabled = false;
            textGradoUno.BorderBrush = Brushes.Black;
            textGradoUno.IsEnabled = false;
            textGradoDos.BorderBrush = Brushes.Black;
            textGradoDos.IsEnabled = false;
            textGradoTres.BorderBrush = Brushes.Black;
            textGradoTres.IsEnabled = false;
            BotonGenerarPoli.IsEnabled = false;

            double grado;
            if(textGradoPolinomio.Text.Length > 0)
            {
                grado = Double.Parse(textGradoPolinomio.Text);
                textGradoPolinomio.BorderBrush = Brushes.Green;
                switch (grado)
                {
                    case 1:
                        textGradoPolinomio.BorderBrush = Brushes.Green;
                        textVarInd.BorderBrush = Brushes.Green;
                        textVarInd.IsEnabled = true;
                        textGradoUno.BorderBrush = Brushes.Green;
                        textGradoUno.IsEnabled = true;
                        textGradoDos.Clear();
                        textGradoTres.Clear();
                        ComprobacionGrados(grado);
                        break;

                    case 2:
                        textGradoPolinomio.BorderBrush = Brushes.Green;
                        textVarInd.BorderBrush = Brushes.Green;
                        textVarInd.IsEnabled = true;
                        textGradoUno.BorderBrush = Brushes.Green;
                        textGradoUno.IsEnabled = true;
                        textGradoDos.BorderBrush = Brushes.Green;
                        textGradoDos.IsEnabled = true;
                        textGradoTres.Clear();
                        ComprobacionGrados(grado);
                        break;

                    case 3:
                        textGradoPolinomio.BorderBrush = Brushes.Green;
                        textVarInd.BorderBrush = Brushes.Green;
                        textVarInd.IsEnabled = true;
                        textGradoUno.BorderBrush = Brushes.Green;
                        textGradoUno.IsEnabled = true;
                        textGradoDos.BorderBrush = Brushes.Green;
                        textGradoDos.IsEnabled = true;
                        textGradoTres.BorderBrush = Brushes.Green;
                        textGradoTres.IsEnabled = true;
                        ComprobacionGrados(grado);
                        break;
                    default:
                        textGradoPolinomio.BorderBrush = Brushes.Red;
                        BotonGenerarPoli.IsEnabled = false;
                        break;
                }
            }
        }

        private void TextGrados_TextChanged(object sender, TextChangedEventArgs e)
        {
            BotonGenerarPoli.IsEnabled = false;

            double grado;

            if (textRangoPrimero.Text.Length > 0 && textRangoSegundo.Text.Length > 0) {
                if(textGradoPolinomio.Text.Length > 0) {
                    grado = Double.Parse(textGradoPolinomio.Text);
                    ComprobacionGrados(grado);
                }
            }
        }

        private void ComprobacionGrados(double grado)
        {
            switch (grado)
            {
                case 1:
                    if (textVarInd.Text.Length > 0 && textGradoUno.Text.Length > 0)
                    {
                        BotonGenerarPoli.IsEnabled = true;
                    }
                    break;

                case 2:
                    if (textVarInd.Text.Length > 0 && textGradoUno.Text.Length > 0 && textGradoDos.Text.Length > 0)
                    {
                        BotonGenerarPoli.IsEnabled = true;
                    }
                    break;

                case 3:
                    if (textVarInd.Text.Length > 0 && textGradoUno.Text.Length > 0 && textGradoDos.Text.Length > 0 && textGradoTres.Text.Length > 0)
                    {
                        BotonGenerarPoli.IsEnabled = true;
                    }
                    break;
            }
        }

        public void CambiarLista(ObservableCollection<Point> lista)
        {
            tbvm.GenerarPolinomio(lista);
            tablaPuntos.ItemsSource = tbvm.Modelo.listaPuntos;
        }
    }
}
