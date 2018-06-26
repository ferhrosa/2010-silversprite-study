using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PF.Xna {
    public class Fps:DrawableGameComponent {
        SpriteBatch spriteBatch;        
        SpriteFont fonte;

        Vector2 posicao;

        int quantidadeQuadros, quantidadeQuadrosMostrar;
        TimeSpan tspanSegundoAnterior;

        public Fps(Game game)
            : base(game) {
        }
        public Fps(Game game, SpriteFont fonte)
            : base(game) {
            this.fonte = fonte;
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            if (null == fonte) fonte = Game.Content.Load<SpriteFont>(@"Fontes\Calibri");

            tspanSegundoAnterior = TimeSpan.Zero;

            //posicao = (new Vector2(Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height)) - fonte.MeasureString(quantidadeQuadrosMostrar.ToString());
            posicao = (new Vector2(1, Game.Window.ClientBounds.Height - fonte.MeasureString(quantidadeQuadrosMostrar.ToString()).Y));

            base.LoadContent();
        }

        public override void Update(GameTime gameTime) {
            if (gameTime.TotalGameTime - tspanSegundoAnterior >= TimeSpan.FromSeconds(1)) {
                quantidadeQuadrosMostrar = quantidadeQuadros;
                quantidadeQuadros = 0;
                tspanSegundoAnterior = TimeSpan.FromSeconds(gameTime.TotalGameTime.TotalSeconds);
            }
            quantidadeQuadros++;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            spriteBatch.Begin();
            spriteBatch.DrawString(fonte, quantidadeQuadrosMostrar.ToString(), posicao, Color.Red);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
