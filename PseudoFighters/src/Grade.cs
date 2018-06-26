using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PF.Xna;

namespace PF.Silverlight.PseudoFighters {

    public class Grade : DrawableGameComponent {
        const int tamanhoBloco = 40;

        bool mousePressionado;

        SpriteBatch spriteBatch;

        List<GradeBloco> blocos;
        List<Texture2D> terrenos;

        Vector2 posicao;
        GradeBloco terrenoSelecionado;

        Jogador jogador;

        public float X { get { return posicao.X; } set { posicao.X = value; } }
        public float Y { get { return posicao.Y; } set { posicao.Y = value; } }

        public int Linhas { get; set; }
        public int Colunas { get; set; }

        public Grade(Game game)
            : base(game) {

            mousePressionado = false;

            blocos = new List<GradeBloco>();
            terrenos = new List<Texture2D>();

            posicao = new Vector2(-50, -50);

            this.Linhas = 25;
            this.Colunas = 25;
        }

        protected override void LoadContent() {
            // Cria um novo SpriteBatch, que pode ser usado para desenhar texturas.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int j = 0; j < Colunas; j++)
                for (int i = 0; i < Linhas; i++) {
                    GradeBloco bloco = new GradeBloco(Game, CarregarTerreno(@"Imagens\Terrenos\Grama1"), i, j);
                    blocos.Add(bloco);
                }

            terrenoSelecionado = new GradeBloco(Game, Game.Content.Load<Texture2D>(@"Imagens\Outros\TerrenoSelecionado"), 1, 1);
            terrenoSelecionado.desenhar = false;

            jogador = new Jogador(Game, Game.Content.Load<Texture2D>(@"Imagens\Personagens\Basico"), 4, 4);

            base.LoadContent();
        }

        Texture2D CarregarTerreno(string nome) {
            Texture2D textura;

            var pesquisa = from Texture2D terreno in terrenos
                           where (terreno.AssetName == nome)
                           select terreno;

            if (pesquisa.Count() > 0)
                textura = pesquisa.First();
            else {
                textura = Game.Content.Load<Texture2D>(nome);
                terrenos.Add(textura);
            }

            return textura;
        }

        public override void Update(GameTime gameTime) {
            if (terrenoSelecionado.desenhar && !mousePressionado && Mouse.LeftButtonDown) {
                jogador.xGrade = terrenoSelecionado.xGrade;
                jogador.yGrade = terrenoSelecionado.yGrade;
            }

            DefinirPosicao();

            terrenoSelecionado.desenhar = false;

            foreach (GradeBloco bloco in blocos) {
                bloco.DefinirPosicao(X, Y);
                bloco.desenhar = true;

                if (bloco.Dentro(Mouse.X, Mouse.Y)) {
                    terrenoSelecionado.xGrade = bloco.xGrade;
                    terrenoSelecionado.yGrade = bloco.yGrade;
                    terrenoSelecionado.DefinirPosicao(X, Y);
                    terrenoSelecionado.desenhar = true;
                }
            }

            //jogador.DefinirPosicao(X, Y);

            mousePressionado = Mouse.LeftButtonDown;

            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Begin();

            foreach (GradeBloco bloco in blocos)
                bloco.Draw(gameTime, spriteBatch);

            terrenoSelecionado.Draw(gameTime, spriteBatch);
            jogador.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
        public override void Draw(GameTime gameTime) {
            Draw(gameTime, spriteBatch);
            base.Draw(gameTime);
        }

        void DefinirPosicao() {
            jogador.X = (Game.Window.ClientBounds.Center.X - jogador.Largura / 2);
            jogador.Y = Game.Window.ClientBounds.Center.Y;

            X = (jogador.X - (jogador.xGrade * tamanhoBloco));
            Y = (jogador.Y - (jogador.yGrade * tamanhoBloco));
        }

    }


    public class GradeBloco : Objeto2D {
        public int xGrade;
        public int yGrade;

        public GradeBloco(Game game) : base(game) { }
        public GradeBloco(Game game, Texture2D sprite) : base(game, sprite) { }

        public GradeBloco(Game game, Texture2D sprite, int xGrade, int yGrade)
            : base(game, sprite) {
            this.xGrade = xGrade;
            this.yGrade = yGrade;
        }

        public void DefinirPosicao(float xBase, float yBase) {
            X = xBase + (xGrade * Largura);
            Y = yBase + (yGrade * Altura);
        }
    }


    public class Jogador : GradeBloco {
        public Jogador(Game game) : base(game) { }
        public Jogador(Game game, Texture2D sprite) : base(game, sprite) { }
        public Jogador(Game game, Texture2D sprite, int xGrade, int yGrade) : base(game, sprite, xGrade, yGrade) { }

    }

}
