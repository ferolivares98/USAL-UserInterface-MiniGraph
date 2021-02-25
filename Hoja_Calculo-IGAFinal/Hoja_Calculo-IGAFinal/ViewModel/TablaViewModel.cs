using Hoja_Calculo_IGUFinal.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Hoja_Calculo_IGUFinal.ViewModel
{

    public delegate void ViewModelDelegate(object sender, EventArgs e);

    public class TablaViewModel : INotifyPropertyChanged
    {
        private TablaModel tm;

        public event ViewModelDelegate addPunto;
        public event ViewModelDelegate removePunto;
        public event ViewModelDelegate modPunto;
        public event ViewModelDelegate dibGraficaPuntos;
        public event ViewModelDelegate dibGraficaBarras;

        public TablaModel Modelo { get => tm; set => tm = value; }

        //Constructor.
        public TablaViewModel()
        {
            tm = new TablaModel();
        }

        public int SelectedIndex
        {
            set
            {
                Notify("SelectedIndex");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Notify(string Sele)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(Sele));
            }
        }

        public void AddPunto(Point p)
        {
            tm.AddPunto(p);
            OnAddPunto();
        }

        private void OnAddPunto()
        {
            if (addPunto != null) addPunto(this, new EventArgs());
        }

        public void RemovePunto(int aElim)
        {
            tm.RemovePunto(aElim);
            OnRemovePunto();
        }

        private void OnRemovePunto()
        {
            if (removePunto != null) removePunto(this, new EventArgs());
        }

        public void ModPunto(string nX, string nY, int indice)
        {
            tm.ModPunto(nX, nY, indice);
            OnModPunto();
        }

        private void OnModPunto()
        {
            if (modPunto != null) modPunto(this, new EventArgs());
        }

        public void DibGraficaPuntos()
        {
            OnDibGraficaPuntos();
        }

        private void OnDibGraficaPuntos()
        {
            if (dibGraficaPuntos != null) dibGraficaPuntos(this, new EventArgs());
        }

        public void DibGraficaBarras()
        {
            OnDibGraficaBarras();
        }

        private void OnDibGraficaBarras()
        {
            if (dibGraficaBarras != null) dibGraficaBarras(this, new EventArgs());
        }
    }
}
