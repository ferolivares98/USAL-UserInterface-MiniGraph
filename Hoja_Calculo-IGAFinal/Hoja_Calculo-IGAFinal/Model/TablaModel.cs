using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;

namespace Hoja_Calculo_IGUFinal.Model
{
    public class TablaModel
    {
        public ObservableCollection<Point> listaPuntos;

        public TablaModel()
        {
            listaPuntos = new ObservableCollection<Point>();
        }

        public void AddPunto(Point p)
        {
            listaPuntos.Add(p);
        }

        public void RemovePunto(int aElim)
        {
            listaPuntos.RemoveAt(aElim);
        }

        public void ModPunto(string nX, string nY, int indice)
        {
            Point pChange = new Point(DoubleValue(nX), DoubleValue(nY));
            listaPuntos[indice] = pChange;
        }

        internal double DoubleValue(string text)
        {
            return Double.Parse(text);
        }
    }
}
