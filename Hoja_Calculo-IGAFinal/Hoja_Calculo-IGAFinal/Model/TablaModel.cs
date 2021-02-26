using System;
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
            Point pChange = new Point(Double.Parse(nX), Double.Parse(nY));
            listaPuntos[indice] = pChange;
        }

        public void GenerarLista(ObservableCollection<Point> lista)
        {
            listaPuntos.Clear();
            listaPuntos = lista;
        }
    }
}
