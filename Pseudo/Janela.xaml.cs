using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Pseudo {
    public partial class Janela : UserControl {
        public Janela() {
            InitializeComponent();
        }

        public void Mostrar() {
            grid.Visibility = Visibility.Visible;
            sbMostrar.Begin();
        }
        
        public void Esconder() {
            sbEsconder.Begin();
        }
        private void sbEsconder_Completed(object sender, EventArgs e) {
            grid.Visibility = Visibility.Collapsed;
        }

        public void MostrarEsconder() {
            if (grid.Visibility == Visibility.Visible)
                Esconder();
            else
                Mostrar();
        }

        public Grid Conteudo { get { return gridConteudo; } }
    }
}
