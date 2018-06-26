using System;
using System.Collections.Generic;
using System.Linq;
using SilverArcade.SilverSprite;
using SilverArcade.SilverSprite.Graphics;
using SilverArcade.SilverSprite.Input;

namespace Pseudo {
    public class Grade : DrawableGameComponent {
        SpriteBatch spriteBatch;

        List<Bloco> blocos;

        Vector2 posicao;
        Vector2 inicio;

        Texture2D texBloco;
        Texture2D texGrama;
        SpriteFont fonteCalibri;

        Vector2 clique, origemClique, posicaoMovimento, blocoClique;
        bool mousePressionado, movendo;

        Objeto2D objetoAdicionando;
        bool adicionando;
        
        const int blocoLargura = 52;
        const int blocoAltura = 26;

        
        protected Vector2 Inicio {
            get { return inicio; }
        }

        public int Linhas { get; set; }
        public int Colunas { get; set; }

        public float Largura {
            get {
                var blocos = from Bloco bloco in this.blocos
                             orderby bloco.xGrade descending, bloco.yGrade descending
                             select bloco;

                return blocos.First().x + blocoLargura - posicao.X;
            }
        }
        public float Altura {
            get {
                var blocos = from Bloco bloco in this.blocos
                             orderby bloco.xGrade, bloco.yGrade descending
                             select bloco;

                return blocos.First().y + blocoAltura - posicao.Y;
            }
        }

        Jogo Jogo { get { return (Jogo)Game; } }

        public Grade(Jogo game)
            : base(game) {

            this.Linhas = 10;
            this.Colunas = 10;

            posicao = Vector2.One;
            inicio = Vector2.Zero;

            DefinirInicio();

            blocos = new List<Bloco>();

            for (int j = 0; j < Colunas; j++)
                for (int i = 0; i < Linhas; i++)
                    blocos.Add(new Bloco(this, 0, 0, i, j));

            Centralizar();

            clique = new Vector2(-1, -1);
            origemClique = new Vector2(-1, -1);
            posicaoMovimento = Vector2.Zero;
            blocoClique = new Vector2(-1, -1);
            mousePressionado = false;
            movendo = false;
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            texBloco = Game.Content.Load<Texture2D>(@"Imagens\Bloco");
            texGrama = Game.Content.Load<Texture2D>(@"Imagens\Grama");

            fonteCalibri = Game.Content.Load<SpriteFont>(@"Fontes\Calibri");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime) {
            UpdateMouse();
            UpdateTeclado(gameTime);

            if (movendo) posicao = posicaoMovimento + (new Vector2(Mouse.X, Mouse.Y) - origemClique);

            DefinirInicio();

            foreach (Bloco bloco in blocos)
                bloco.DefinePosicao();

            base.Update(gameTime);
        }

        void UpdateMouse() {
            clique.X = -1;
            clique.Y = -1;

            if (!mousePressionado && Mouse.LeftButtonDown) {
                origemClique.X = Mouse.X;
                origemClique.Y = Mouse.Y;
                posicaoMovimento = posicao;
            } else if (mousePressionado && !Mouse.LeftButtonDown) {
                if (!movendo) clique = origemClique;
                movendo = false;

                origemClique.X = -1;
                origemClique.Y = -1;
                posicaoMovimento = Vector2.Zero;
            } else if (mousePressionado && Mouse.LeftButtonDown) {
                if (!movendo && ((origemClique.X - Mouse.X > 5) || (Mouse.X - origemClique.X > 5) || (origemClique.Y - Mouse.Y > 5) || (Mouse.Y - origemClique.Y > 5)))
                    movendo = true;
            }

            if (clique.X >= 0 && clique.Y >= 0) {
                foreach (Bloco bloco in blocos) bloco.Selecionado = false;

                var areaBlocos = from Bloco bloco in blocos
                                 orderby bloco.Objeto.DrawOrder descending
                                 where (clique.X >= bloco.Objeto.X
                                     && clique.X < bloco.Objeto.X + bloco.Objeto.Largura
                                     && clique.Y >= bloco.Objeto.Y
                                     && clique.Y < bloco.Objeto.Y + bloco.Objeto.Altura)
                                 select bloco;

                foreach (Bloco bloco in areaBlocos) {
                    if (bloco.Objeto.Colide(clique)) {
                        bloco.Selecionado = true;
                        break;
                    }
                }
            }

            mousePressionado = Mouse.LeftButtonDown;
        }
        void UpdateTeclado(GameTime gameTime) {
            if (!movendo) {
                float movimento = (float)gameTime.ElapsedGameTime.TotalSeconds * 100;

                KeyboardState keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyDown(Keys.Left)) posicao.X -= movimento;
                if (keyboardState.IsKeyDown(Keys.Right)) posicao.X += movimento;
                if (keyboardState.IsKeyDown(Keys.Up)) posicao.Y -= movimento;
                if (keyboardState.IsKeyDown(Keys.Down)) posicao.Y += movimento;
            }
        }

        public override void Draw(GameTime gameTime) {
            spriteBatch.Begin();

            foreach (Bloco bloco in blocos) {
                spriteBatch.Draw(texGrama, bloco.Posicao, Color.White);
                //spriteBatch.DrawString(fonteCalibri, bloco.PosicaoGrade.X.ToString() + "-" + bloco.PosicaoGrade.Y.ToString(), bloco.Posicao + new Vector2(10, 5), Color.LightBlue);
            }

            DrawMouse();

            spriteBatch.End();

            base.Draw(gameTime);
        }

        void DrawMouse() {
            //spriteBatch.DrawString(fonteCalibri, origemClique.X.ToString() + " | " + origemClique.Y.ToString() + " | " + movendo.ToString(), Vector2.One, Color.LightBlue);
            //spriteBatch.DrawString(fonteCalibri, blocoClique.X.ToString() + " | " + blocoClique.Y.ToString(), posicao, Color.LightBlue);
            
            if (adicionando) {
                objetoAdicionando.X = Mouse.X;
                objetoAdicionando.Y = Mouse.Y;
            }
        }

        void DefinirInicio() {
            inicio.X = posicao.X;
            inicio.Y = posicao.Y + (Colunas * ((blocoAltura / 2) - 1)) - Linhas;
        }
        void Centralizar() {
            float descontoRodape = 0;

            if (Jogo.Pagina.gridRodape.Visibility == System.Windows.Visibility.Visible)
                descontoRodape = (float)Jogo.Pagina.gridRodape.Height;

            posicao.X = (Game.Window.ClientBounds.Width / 2) - (Largura / 2);
            posicao.Y = ((Game.Window.ClientBounds.Height - descontoRodape) / 2) - (Altura / 2);
        }

        public void AdicionarCasa() {
            for (int i = 0; i < blocos.Count; i++) {
                if (blocos[i].ObjetoNulo) {
                    blocos[i].Objeto.Sprite = Game.Content.Load<Texture2D>(@"Imagens\Casa");
                    break;
                }
            }
        }

        public void IniciarAdicao(string objeto) {
            switch (objeto) {
                case "Casa":
                    objetoAdicionando = new Objeto2D(Game, Game.Content.Load<Texture2D>(@"Imagens\Casa"));
                    break;
            }

            if (objetoAdicionando != null) {
                adicionando = true;
                objetoAdicionando.Cor = new Color(Color.White, 100);
                Game.Components.Add(objetoAdicionando);
            }
        }

        //public Bloco ClicouObjeto(Bloco bloco) {

        //}

        /// <summary>
        /// Representa um bloco na grade diagonal.
        /// </summary>
        public class Bloco {
            public float x;
            public float y;

            public int xGrade;
            public int yGrade;

            Grade grade;
            Objeto2D objeto;

            Color corSelecionado = new Color(150, 200, 255);

            public Vector2 Posicao {
                get { return new Vector2(this.x, this.y); }
            }
            public Vector2 PosicaoGrade {
                get { return new Vector2(xGrade, yGrade); }
            }

            public Objeto2D Objeto {
                get {
                    if (null == objeto) {
                        objeto = new Objeto2D(grade.Game);
                        objeto.DrawOrder = ((grade.Colunas - xGrade) * 100) + yGrade;
                        grade.Game.Components.Add(objeto);
                    }
                    return objeto;
                }
            }

            public bool ObjetoNulo {
                get { return (null == objeto || null == objeto.Sprite); }
            }

            public bool Selecionado {
                set {
                    if (!ObjetoNulo) {
                        if (value)
                            objeto.Cor = corSelecionado;
                        else
                            objeto.Cor = Color.White;
                    }
                }
            }

            public Bloco(Grade grade, float x, float y, int xGrade, int yGrade) {
                this.grade = grade;
                this.x = x;
                this.y = y;
                this.xGrade = xGrade;
                this.yGrade = yGrade;
                this.objeto = null;

                DefinePosicao();
            }

            public void DefinePosicao() {
                x = grade.Inicio.X + (xGrade * blocoLargura / 2) + (yGrade * blocoLargura / 2) - (xGrade * 2);
                y = grade.Inicio.Y + (yGrade * blocoAltura / 2) - (xGrade * blocoAltura / 2) + xGrade;

                DefinePosicaoObjeto();
            }

            private void DefinePosicaoObjeto() {
                if (!ObjetoNulo) {
                    objeto.X = x;
                    objeto.Y = y - (objeto.Altura - blocoAltura);
                }
            }

        }
    }
}
