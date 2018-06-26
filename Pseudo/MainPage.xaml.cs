using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Pseudo.Servicos;

namespace Pseudo {
    public partial class MainPage : UserControl {
        ServicoPseudoClient servico;

        public MainPage() {
            InitializeComponent();

            servico = new ServicoPseudoClient();
            servico.CarregarItensCompleted += new EventHandler<CarregarItensCompletedEventArgs>(servico_CarregarItensCompleted);
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e) {
            Button btnOk = new Button();
            btnOk.Content = "OK";
            btnOk.Click += new RoutedEventHandler(btnOk_Click);

            janela.Conteudo.Children.Add(btnOk);
        }

        protected void btnOk_Click(object sender, EventArgs e) {
            janela.Esconder();
        }

        private void oiCasa_Click(object sender, EventArgs e) {
            jogo.Grade.AdicionarCasa();
        }

        public void Alert(string texto) {
            HtmlPage.Window.Invoke("alert", new string[] { texto });
        }

        [ScriptableMemberAttribute]
        public string EventoTeste() {
            return this.ToString();
        }

        void servico_CarregarItensCompleted(object sender, CarregarItensCompletedEventArgs e) {
            pnlItens.Children.Clear();

            if (oiConstruir.Selecionado) {
                foreach (Item item in e.Result) {
                    ObjetoItem objetoItem = new ObjetoItem(item.Imagem, item.Descricao, item.Preco);
                    objetoItem.Selecionavel = true;

                    objetoItem.Selecionar += new EventHandler(objetoItem_Selecionar);

                    pnlItens.Children.Add(objetoItem);
                }
            }
        }

        void objetoItem_Selecionar(object sender, EventArgs e) {
            ObjetoItem objetoItem = (ObjetoItem)sender;

            var itens = from ObjetoItem item in ((StackPanel)objetoItem.Parent).Children
                        where item != objetoItem
                        select item;                       

            foreach (ObjetoItem item in itens) {
                item.Selecionado = false;
            }

            //jogo.Grade.AdicionarCasa();
            jogo.Grade.IniciarAdicao("Casa");
        }

        private void oiConstruir_Selecionar(object sender, EventArgs e) {
            servico.CarregarItensAsync();
        }

        private void oiConstruir_Desselecionar(object sender, EventArgs e) {
            pnlItens.Children.Clear();
        }

        //[ScriptableMemberAttribute]
        //public Jogo Jogo { get { return jogo; } }
    }
}
