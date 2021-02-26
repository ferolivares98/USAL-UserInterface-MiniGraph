using System;
using System.Windows;

namespace Hoja_Calculo_IGAFinal.View
{
    public class AjustesEventArgs : EventArgs
    {
        public double escalaNueva { get; set; }
        public AjustesEventArgs(double escalaNueva)
        {
            this.escalaNueva = escalaNueva;
        }
    }

    public delegate void AjustesDelegate(object sender, AjustesEventArgs e);

    public partial class AjustesGraph : Window
    {
        public event AjustesDelegate onAplicarEscala;

        public AjustesGraph(double escala)
        {
            InitializeComponent();
            textEscala.Text = escala.ToString();
        }

        private void BotonAplicarEscala_Click(object sender, RoutedEventArgs e)
        {
            double escalaADev;

            if(textEscala.Text.Length > 0)
            {
                escalaADev = Double.Parse(textEscala.Text);
            }
            else
            {
                escalaADev = 80; //Predeterminado.
            }
            OnAplicarEscala(escalaADev);
        }

        private void OnAplicarEscala(double escala)
        {
            if (onAplicarEscala != null) onAplicarEscala(this, new AjustesEventArgs(escala));
        }
    }
}
