using Hoja_Calculo_IGAFinal.View;
using Hoja_Calculo_IGUFinal.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Hoja_Calculo_IGUFinal
{
    public partial class MainWindow : Window
    {
        Tabla tb;
        AjustesGraph aj;
        TablaViewModel tbvm;
        private Line ejeX, ejeY;
        private double ancho, alto;
        private Point purgadoInicio, purgadoFin;
        private int tipoDeGrafica = 0;
        private double escala, traslado;
        private ScaleTransform scaleTr;
        private TranslateTransform translateTr;
        private TransformGroup transGr;

        public MainWindow()
        {
            InitializeComponent();
            tbvm = new TablaViewModel();
            escala = 80;
            traslado = escala / 2;
            this.Loaded += Comienzo;
            scaleTr = new ScaleTransform();
            translateTr = new TranslateTransform();
            transGr = new TransformGroup();
            transGr.Children.Add(translateTr);
            transGr.Children.Add(scaleTr);

            tbvm.dibGraficaPuntos += DibujarGraficaPuntos;
            tbvm.dibGraficaBarras += DibujarGraficaBarras;
            tbvm.addPunto += ListaActualizada;
            tbvm.removePunto += ListaActualizada;
            tbvm.modPunto += ListaActualizada;
        }

        private void Comienzo(object sender, EventArgs e)
        {
            ejeX = null;
            ejeY = null;
            Ejes();
        }

        private void MenuItemAjustes_Click(object sender, RoutedEventArgs e)
        {
            aj = new AjustesGraph(escala);
            aj.Owner = this;
            aj.Closing += Aj_Closing;
            aj.Show();
            HeaderAjustes.IsEnabled = false;
            aj.onAplicarEscala += AplicarEscala;
        }

        private void AplicarEscala(object sender, AjustesEventArgs e)
        {
            escala = e.escalaNueva;
            traslado = escala / 2;
            Ejes();
            if (tipoDeGrafica != 0)
            {
                if (tipoDeGrafica == 1)
                {
                    tbvm.DibGraficaPuntos();
                }
                else
                {
                    tbvm.DibGraficaBarras();
                }
            }
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
        private void Aj_Closing(object sender, CancelEventArgs e)
        {
            HeaderAjustes.IsEnabled = true;
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ancho = Graph.ActualWidth;
            alto = Graph.ActualHeight;

            translateTr.X = traslado;
            translateTr.Y = traslado;

            scaleTr.ScaleX = ancho / escala;
            scaleTr.ScaleY = alto / escala;
        }

        private void Ejes()
        {
            ejeX = new Line();
            ejeY = new Line();
            ancho = Graph.ActualWidth;
            alto = Graph.ActualHeight;
            translateTr.X = traslado;
            translateTr.Y = traslado;
            scaleTr.ScaleX = ancho / escala;
            scaleTr.ScaleY = alto / escala;

            ejeX.Stroke = Brushes.Black;
            ejeX.StrokeThickness = 0.15;
            ejeX.X1 = -traslado;
            ejeX.Y1 = 0;
            ejeX.X2 = traslado;
            ejeX.Y2 = 0;

            ejeX.RenderTransform = transGr;
            Graph.Children.Add(ejeX);

            ejeY.Stroke = Brushes.Black;
            ejeY.StrokeThickness = 0.15;
            ejeY.X1 = 0;
            ejeY.Y1 = -traslado;
            ejeY.X2 = 0;
            ejeY.Y2 = traslado;

            ejeY.RenderTransform = transGr;
            Graph.Children.Add(ejeY);
        }

        private void DibujarGraficaPuntos(object sender, EventArgs e)
        {
            Graph.Children.Clear();
            Ejes();

            tipoDeGrafica = 1;

            ObservableCollection<Point> listaP = tbvm.Modelo.listaPuntos;
            Polyline linea = new Polyline();
            Point p = new Point();

            //Declaraciones de la línea
            linea.Stroke = Brushes.Red;
            linea.StrokeThickness = 0.2;

            /*Declaraciones del punto
            const double width = 0.8;
            const double radius = width / 2;
            Ellipse el;*/

            for (int i = 0; i < listaP.Count; i++)
            {
                p.X = listaP[i].X;
                p.Y = -(listaP[i].Y);
                linea.Points.Add(p);

                /*el = new Ellipse();
                el.SetValue(Canvas.LeftProperty, p.X*(ancho/80) - width);
                el.SetValue(Canvas.TopProperty, p.Y*(alto/80) - width);
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

            tipoDeGrafica = 2;

            ObservableCollection<Point> listaP = tbvm.Modelo.listaPuntos;
            Polyline linea = new Polyline();
            Point p = new Point();
            Point pEje = new Point(0, 0);

            for (int i = 0; i < listaP.Count; i++)
            {
                linea = LineaBarras();
                p.X = listaP[i].X;
                p.Y = -(listaP[i].Y); //Porque Y va al contrario.
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

        private void ListaActualizada(object sender, EventArgs e)
        {
            if(tipoDeGrafica != 0)
            {
                if(tipoDeGrafica == 1)
                {
                    tbvm.DibGraficaPuntos();
                }
                else
                {
                    tbvm.DibGraficaBarras();
                }
            }
        }

        private void BorrarPuntos_LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            purgadoInicio.X = e.GetPosition(Graph).X;
            purgadoInicio.Y = e.GetPosition(Graph).Y;
        }

        private void BorrarPuntos_LeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point aux1, aux2 = new Point();

            purgadoFin.X = e.GetPosition(Graph).X;
            purgadoFin.Y = e.GetPosition(Graph).Y;

            if(purgadoInicio.X > purgadoFin.X && purgadoInicio.Y > purgadoFin.Y)
            {
                aux1 = purgadoInicio;
                purgadoInicio = purgadoFin;
                purgadoFin = aux1;
            }else if(purgadoInicio.X > purgadoFin.X && purgadoInicio.Y < purgadoFin.Y)
            {
                aux1 = new Point(purgadoFin.X, purgadoInicio.Y);
                aux2 = new Point(purgadoInicio.X, purgadoFin.Y);
                purgadoInicio = aux1;
                purgadoFin = aux2;
            }else if(purgadoInicio.X < purgadoFin.X && purgadoInicio.Y > purgadoFin.Y)
            {
                aux1 = new Point(purgadoInicio.X, purgadoFin.Y);
                aux2 = new Point(purgadoFin.X, purgadoInicio.Y);
                purgadoInicio = aux1;
                purgadoFin = aux2;
            }

            if(tipoDeGrafica != 0)
            {
                if(tipoDeGrafica == 1)
                {
                    BorrarPuntosGraficaPuntos();
                }
                else
                {
                    BorrarPuntosGraficaBarras();
                }
            }
        }

        private void BorrarPuntosGraficaPuntos()
        {
            ObservableCollection<Point> listaP = tbvm.Modelo.listaPuntos;
            ObservableCollection<Point> listaNueva = new ObservableCollection<Point>();
            Point p;

            double p1 = purgadoInicio.X / (ancho / escala);
            double p2 = purgadoInicio.Y / (alto / escala);
            double p3 = purgadoFin.X / (ancho / escala);
            double p4 = purgadoFin.Y / (alto / escala);

            for (int i = 0; i < listaP.Count; i++)
            {
                p = new Point(listaP[i].X + traslado, traslado - listaP[i].Y);

                if(p.X > p1 && p.X < p3)
                {
                    if(p.Y > p2 && p.Y < p4)
                    {
                        p.X = listaP[i].X;
                        p.Y = listaP[i].Y;
                        listaNueva.Add(p);
                    }
                }
            }
            tb.CambiarLista(listaNueva);
            tbvm.DibGraficaPuntos();
        }

        private void BorrarPuntosGraficaBarras()
        {
            ObservableCollection<Point> listaP = tbvm.Modelo.listaPuntos;
            ObservableCollection<Point> listaNueva = new ObservableCollection<Point>();
            Point p;
            Point pAux = new Point();

            double p1 = purgadoInicio.X / (ancho / escala);
            double p2 = purgadoInicio.Y / (alto / escala);
            double p3 = purgadoFin.X / (ancho / escala);
            double p4 = purgadoFin.Y / (alto / escala);

            for(int i = 0; i < listaP.Count; i++)
            {
                p = new Point(listaP[i].X + traslado, traslado - listaP[i].Y);

                if(listaP[i].Y >= 0) //Vamos a permitir marcar cualquier parte de la barra (excluyendo el grosor), no sólo el punto superior o inferior máximo.
                {
                    for(double j = p.Y; j <= traslado; j++)
                    {
                        pAux.X = p.X;
                        pAux.Y = j;

                        if (pAux.X > p1 && pAux.X < p3)
                        {
                            if (pAux.Y > p2 && pAux.Y < p4)
                            {
                                p.X = listaP[i].X;
                                p.Y = listaP[i].Y;
                                listaNueva.Add(p);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for(double j = p.Y; j >= traslado; j--)
                    {
                        pAux.X = p.X;
                        pAux.Y = j;

                        if (pAux.X > p1 && pAux.X < p3)
                        {
                            if (pAux.Y > p2 && pAux.Y < p4)
                            {
                                p.X = listaP[i].X;
                                p.Y = listaP[i].Y;
                                listaNueva.Add(p);
                                break;
                            }
                        }
                    }
                }
            }
            tb.CambiarLista(listaNueva);
            tbvm.DibGraficaBarras();
        }
    }
}
