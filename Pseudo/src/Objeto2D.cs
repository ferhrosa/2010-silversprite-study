using System;
using SilverArcade.SilverSprite;
using SilverArcade.SilverSprite.Audio;
using SilverArcade.SilverSprite.Content;
using SilverArcade.SilverSprite.GamerServices;
using SilverArcade.SilverSprite.Graphics;
using SilverArcade.SilverSprite.Input;
using SilverArcade.SilverSprite.Media;
using SilverArcade.SilverSprite.Storage;

namespace Pseudo {
    public class Objeto2D : DrawableGameComponent {
        SpriteBatch spriteBatch;
        Texture2D sprite;
        Vector2 posicao;

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

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime) {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            if (null != sprite) {
                spriteBatch.Begin();
                spriteBatch.Draw(sprite, posicao, Cor);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        public bool Colide(int x, int y) {
            Color pixel = sprite.GetPixel(x - (int)posicao.X, y - (int)posicao.Y);
            return pixel.A > 0;
        }
        public bool Colide(Vector2 local) {
            return Colide((int)local.X, (int)local.Y);
        }
    }
}