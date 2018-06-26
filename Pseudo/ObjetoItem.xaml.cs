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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pseudo {
    public partial class ObjetoItem : UserControl {
        bool clicando;

        TipoItem tipo;
        bool selecionado, mouseFoco;

        public enum TipoItem {
            Opcao,
            Item
        }

        public ObjetoItem() {
            Criar();
        }
        public ObjetoItem(string imagem, string descricao, int preco) {
            Criar();

            Imagem = imagem;
            Descricao = descricao;
            Preco = preco;
        }

        void Criar() {
            InitializeComponent();

            // Adiciona eventos.
            Click += new EventHandler(ObjetoItem_Click);
            Selecionar += new EventHandler(ObjetoItem_Selecionar);
            Desselecionar += new EventHandler(ObjetoItem_Desselecionar);
        }

        void ObjetoItem_Click(object sender, EventArgs e) {
            if (Selecionavel) Selecionado = !Selecionado;
        }

        void ObjetoItem_MouseEnter(object sender, MouseEventArgs e) {
            mouseFoco = true;
            if (!Selecionado) sbMouseEnter.Begin();
        }
        void ObjetoItem_MouseLeave(object sender, MouseEventArgs e) {
            mouseFoco = false;
            clicando = false;

            if (!Selecionado) sbMouseLeave.Begin();
        }

        void ObjetoItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            clicando = true;
        }
        void ObjetoItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            if (clicando) Click.Invoke(sender, new EventArgs());
            clicando = false;
        }

        void ObjetoItem_Selecionar(object sender, EventArgs e) {
            sbSelecionar.Begin();
        }
        void ObjetoItem_Desselecionar(object sender, EventArgs e) {
            sbDesselecionar.Begin();
            
            // Se o cursor do mouse não estiver sobre esse objeto, inicia animação de estar saindo o mouse dele.
            if (!mouseFoco) sbMouseLeave.Begin();
        }


        public string Imagem {
            get {
                if (null == imagem.Source)
                    return String.Empty;
                else
                    return ((BitmapImage)imagem.Source).UriSource.ToString();
            }
            set {
                Uri uri = new Uri(value, UriKind.Relative);

                if (null == imagem.Source)
                    imagem.Source = new BitmapImage(uri);
                else
                    ((BitmapImage)imagem.Source).UriSource = uri;
            }
        }

        public string Descricao {
            get { return descricao.Text; }
            set { descricao.Text = value; }
        }

        public int Preco {
            get { return Int32.Parse(preco.Text); }
            set { preco.Text = "$" + value.ToString(); }
        }

        public TipoItem Tipo {
            get { return tipo; }
            set {
                tipo = value;

                Color corInicial = new Color();
                Color corFinal = new Color();
                Color corBorda = new Color();

                switch (value) {
                    case TipoItem.Opcao:
                        Thickness margin = new Thickness(0);
                        LayoutRoot.Margin = margin;
                        corInicial.R = 200;
                        corInicial.G = 100;
                        corInicial.B = 100;
                        corFinal.R = 150;
                        corFinal.G = 50;
                        corFinal.B = 50;
                        corBorda.R = 200;
                        corBorda.G = 100;
                        corBorda.B = 100;
                        break;
                    default:
                        corInicial.R = 102;
                        corInicial.G = 153;
                        corInicial.B = 204;
                        corFinal.R = 51;
                        corFinal.G = 102;
                        corFinal.B = 153;
                        corBorda.R = 102;
                        corBorda.G = 153;
                        corBorda.B = 204;
                        break;
                }

                CorInicial = corInicial;
                CorFinal = corFinal;
                CorBorda = corBorda;

                quadroLuz.Color = corBorda;
            }
        }

        Color CorInicial {
            set {
                Color corVisivel = value;
                corVisivel.A = 153;

                quadroFundo1.Color = value;
                sbMouseEnter_quadroFundo1.From = value;
                sbMouseEnter_quadroFundo1.To = corVisivel;
                sbMouseLeave_quadroFundo1.From = corVisivel;
                sbMouseLeave_quadroFundo2.To = value;
            }
        }
        Color CorFinal {
            set {
                Color corVisivel = value;
                corVisivel.A = 153;

                quadroFundo2.Color = value;
                sbMouseEnter_quadroFundo2.From = value;
                sbMouseEnter_quadroFundo2.To = corVisivel;
                sbMouseLeave_quadroFundo2.From = corVisivel;
                sbMouseLeave_quadroFundo2.To = value;
            }
        }
        Color CorBorda {
            set {
                Color corVisivel = value;
                corVisivel.A = 204;

                quadroBorda.Color = value;
                sbMouseEnter_quadroBorda.From = value;
                sbMouseEnter_quadroBorda.To = corVisivel;
                sbMouseLeave_quadroBorda.From = corVisivel;
                sbMouseLeave_quadroBorda.To = value;
            }
        }

        public bool Selecionavel { get; set; }
        public bool Selecionado {
            get { return selecionado; }
            set {
                if (selecionado != value) {
                    selecionado = value;

                    if (selecionado)
                        Selecionar.Invoke(this, new EventArgs());
                    else
                        Desselecionar.Invoke(this, new EventArgs());
                }
            }
        }


        public event EventHandler Click;
        public event EventHandler Selecionar;
        public event EventHandler Desselecionar;

    }
}
