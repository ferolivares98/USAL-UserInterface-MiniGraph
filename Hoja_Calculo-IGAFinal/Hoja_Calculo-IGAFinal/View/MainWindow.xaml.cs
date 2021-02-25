using Hoja_Calculo_IGUFinal.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace Hoja_Calculo_IGUFinal
{
    public partial class MainWindow : Window
    {
        Tabla tb;
        TablaViewModel tbvm;
        private Line ejeX, ejeY;
        private double ancho, alto;
        private ScaleTransform scaleTr;
        private TranslateTransform translateTr;
        private TransformGroup transGr;

        public MainWindow()
        {
            InitializeComponent();
            tbvm = new TablaViewModel();
            this.Loaded += Comienzo;
            scaleTr = new ScaleTransform();
            translateTr = new TranslateTransform();
            transGr = new TransformGroup();
            transGr.Children.Add(translateTr);
            transGr.Children.Add(scaleTr);

            tbvm.dibGraficaPuntos += DibujarGraficaPuntos;
            tbvm.dibGraficaBarras += DibujarGraficaBarras;
        }

        private void Comienzo(object sender, EventArgs e)
        {
            ejeX = null;
            ejeY = null;
            Ejes();
        }

        private void MenuItemAjustes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemSalir_Click(object sender, RoutedEventArgs e)
        {
            Close(); //Como Owner de tb, también la cerrará.
        }

        private void MenuItemAbrirCoord_Click(object sender, RoutedEventArgs e)
        {
            tb = new Tabla(1, tbvm);
            tb.Owner = this;
            tb.Closing += Tb_Closing;
            tb.Show();
            HeaderDatos.IsEnabled = false;
        }

        private void MenuItemAbrirPolin_Click(object sender, RoutedEventArgs e)
        {
            tb = new Tabla(2, tbvm);
            tb.Owner = this;
            tb.Closing += Tb_Closing;
            tb.Show();
            HeaderDatos.IsEnabled = false;
        }

        private void Tb_Closing(object sender, CancelEventArgs e)
        {
            HeaderDatos.IsEnabled = true;
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ancho = Graph.ActualWidth;
            alto = Graph.ActualHeight;

            translateTr.X = 20;
            translateTr.Y = 20;

            scaleTr.ScaleX = ancho / 40;
            scaleTr.ScaleY = alto / 40;
        }

        private void Ejes()
        {
            ejeX = new Line();
            ejeY = new Line();
            ancho = Graph.ActualWidth;
            alto = Graph.ActualHeight;

            ejeX.Stroke = Brushes.Black;
            ejeX.StrokeThickness = 0.15;
            ejeX.X1 = -20;
            ejeX.Y1 = 0;
            ejeX.X2 = 20;
            ejeX.Y2 = 0;

            ejeX.RenderTransform = transGr;
            Graph.Children.Add(ejeX);

            ejeY.Stroke = Brushes.Black;
            ejeY.StrokeThickness = 0.15;
            ejeY.X1 = 0;
            ejeY.Y1 = -20;
            ejeY.X2 = 0;
            ejeY.Y2 = 20;

            ejeY.RenderTransform = transGr;
            Graph.Children.Add(ejeY);
        }

        private void DibujarGraficaPuntos(object sender, EventArgs e)
        {
            Graph.Children.Clear();
            Ejes();

            ObservableCollection<Point> listaP = tbvm.Modelo.listaPuntos;
            Polyline linea = new Polyline();
            Point p = new Point();

            //Declaraciones de la línea
            linea.Stroke = Brushes.Red;
            linea.StrokeThickness = 0.2;

            /*Declaraciones del punto
            const double width = 0.2;
            const double radius = width / 2;
            Ellipse el;*/

            for (int i = 0; i < listaP.Count; i++)
            {
                p.X = listaP[i].X;
                p.Y = -(listaP[i].Y);
                linea.Points.Add(p);

                /*el = new Ellipse();
                el.SetValue(Canvas.LeftProperty, p.X - radius);
                el.SetValue(Canvas.TopProperty, p.Y - radius);
                el.Fill = Brushes.Green;
                el.StrokeThickness = 0.04;
                el.Width = width;
                el.Height = width;
                el.RenderTransform = transGr;
                Graph.Children.Add(el);*/
            }

            linea.RenderTransform = transGr;
            Graph.Children.Add(linea);
        }

        private void DibujarGraficaBarras(object sender, EventArgs e)
        {
            Graph.Children.Clear();
            Ejes();

            ObservableCollection<Point> listaP = tbvm.Modelo.listaPuntos;
            Polyline linea = new Polyline();
            Point p = new Point();
            Point pEje = new Point(0, 0);

            for(int i = 0; i < listaP.Count; i++)
            {
                linea = LineaBarras();
                p.X = listaP[i].X;
                p.Y = -(listaP[i].Y);
                pEje.X = listaP[i].X;
                linea.Points.Add(p);
                linea.Points.Add(pEje);
                linea.RenderTransform = transGr;
                Graph.Children.Add(linea);
            }
        }

        private Polyline LineaBarras()
        {
            Polyline linea = new Polyline();
            linea.Stroke = Brushes.Red;
            linea.StrokeThickness = 0.4;
            return linea;
        }
    }
}
