using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

namespace PF.Xna {
    public class Objeto2D : DrawableGameComponent {
        Texture2D sprite;
        Vector2 posicao;

        public bool desenhar;

        public int Largura {
            get {
                if (null == sprite)
                    return 0;
                else
                    return sprite.Width;
            }
        }
        public int Altura {
            get {
                if (null == sprite)
                    return 0;
                else
                    return sprite.Height;
            }
        }

        public float X {
            get { return posicao.X; }
            set { posicao.X = value; }
        }
        public float Y {
            get { return posicao.Y; }
            set { posicao.Y = value; }
        }

        public Texture2D Sprite {
            get { return this.sprite; }
            set { sprite = value; }
        }
        public Vector2 Posicao {
            get { return posicao; }
            set { posicao = value; }
        }
        public Color Cor { get; set; }

        private void Criar() {
            this.posicao = Vector2.Zero;
            this.desenhar = true;
            this.Cor = Color.White;
        }

        public Objeto2D(Game game, Texture2D sprite)
            : base(game) {
            this.sprite = sprite;
            Criar();
        }
        public Objeto2D(Game game)
            : base(game) {
            Criar();
        }

        public override void Update(GameTime gameTime) {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            if (desenhar && null != sprite && null != spriteBatch && Visivel) {
                spriteBatch.Draw(sprite, posicao, Cor);
                //spriteBatch.DrawString(Game.Content.Load<SpriteFont>(@"Fontes\Calibri"), desenhar.ToString(), posicao, Color.Red);

                base.Draw(gameTime);
            }
        }

        public bool Colide(int x, int y) {
            Color pixel = sprite.GetPixel(x - (int)posicao.X, y - (int)posicao.Y);
            return pixel.A > 0;
        }
        public bool Colide(Vector2 local) {
            return Colide((int)local.X, (int)local.Y);
        }

        public bool Dentro(int x, int y) {
            Rectangle retangulo = new Rectangle((int)this.X, (int)this.Y, this.Largura, this.Altura);
            return (x >= retangulo.Left && x <= retangulo.Right && y >= retangulo.Top && y <= retangulo.Bottom);
        }

        public bool Visivel {
            get {
                Rectangle retangulo = new Rectangle((int)this.X, (int)this.Y, this.Largura, this.Altura);
                return (
                    (retangulo.Left >= 0 && retangulo.Top >= 0) ||
                    (retangulo.Left >= 0 && retangulo.Bottom <= Game.Window.ClientBounds.Bottom) ||
                    (retangulo.Top >= 0 && retangulo.Right <= Game.Window.ClientBounds.Right) ||
                    (retangulo.Right <= Game.Window.ClientBounds.Right && retangulo.Bottom <= Game.Window.ClientBounds.Bottom)
                );
            }
        }
    }
}