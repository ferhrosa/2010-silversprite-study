﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using SilverArcade.SilverSprite;
using SilverArcade.SilverSprite.Audio;
using SilverArcade.SilverSprite.Content;
using SilverArcade.SilverSprite.GamerServices;
using SilverArcade.SilverSprite.Graphics;
using SilverArcade.SilverSprite.Input;
using SilverArcade.SilverSprite.Media;
using SilverArcade.SilverSprite.Storage;

namespace Pseudo {

    public class Jogo : SilverArcade.SilverSprite.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Grade grade;

        SpriteFont fonteCalibri;

        TimeSpan mousePressionado;

        bool mostrouMensagemInicial;

        public Jogo() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;

            Content.RootDirectory = "Content";

            MouseLeave +=new MouseEventHandler(Jogo_MouseLeave);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            grade = new Grade(this);
            grade.DrawOrder = 2;
            Components.Add(grade);

            Fps fps = new Fps(this);
            fps.DrawOrder = 100000;
            Components.Add(fps);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Cria um novo SpriteBatch, que pode ser usado para desenhar texturas.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            fonteCalibri = Content.Load<SpriteFont>(@"Fontes\Calibri");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            //if (MousePressionado(gameTime)) Pagina.janela.MostrarEsconder();

            if (!mostrouMensagemInicial && gameTime.TotalGameTime.TotalSeconds > 1) {
                //Pagina.janela.Mostrar();
                mostrouMensagemInicial = true;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            //spriteBatch.Begin();

            //string texto = "Parent: " + ((Grid)((Canvas)Parent).Parent).Parent.ToString();

            //spriteBatch.DrawString(fonteCalibri,
            //    texto,
            //    new Vector2(Window.ClientBounds.Width - fonteCalibri.MeasureString(texto).X, Window.ClientBounds.Height - fonteCalibri.MeasureString(texto).Y),
            //    Color.Red);

            //spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool MousePressionado(GameTime gameTime) {
            if (Mouse.LeftButtonDown && ((null == mousePressionado) || (gameTime.TotalRealTime.TotalMilliseconds >= (mousePressionado.TotalMilliseconds + 500)))) {
                mousePressionado = gameTime.TotalRealTime;
                return true;
            } else
                return false;
        }

        private void Jogo_MouseLeave(object sender, EventArgs e) {
            Mouse.LeftButtonDown = false;
        }

        public Grade Grade { get { return this.grade; } }

        public Grid LayoutRoot { get { return (Grid)this.Parent; } }
        public MainPage Pagina { get { return (MainPage)this.LayoutRoot.Parent; } }
    }
}
