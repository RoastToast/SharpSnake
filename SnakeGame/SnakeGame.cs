using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Net;
using System.Text;

namespace SnakeGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SnakeGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int snakeDir = 0; // 1,2,3,4 north, east, south, west
        Texture2D snakeBlock;
        Vector2 _snakePosition;
        int _snakeWidth = 16;
        int _snakeHeight = 16;

        public SnakeGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsFixedTimeStep = true;
            this.graphics.SynchronizeWithVerticalRetrace = true;
            this.TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 264); // 33ms = 30fps

            this.Activated += (sender, args) =>
            {
                this.Window.Title = "Active Application";
            };
            this.Deactivated += (sender, args) =>
            {
                this.Window.Title = "InActive Application";
            };
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            Rectangle snakeRect = new Rectangle(
                this.graphics.PreferredBackBufferWidth / 2 - _snakeWidth / 2,
                this.graphics.PreferredBackBufferHeight / 2 - _snakeHeight / 2,
                _snakeWidth - 1,
                _snakeHeight - 1
            );
            _snakePosition = new Vector2(this.graphics.PreferredBackBufferWidth / 2 - _snakeWidth / 2,
                this.graphics.PreferredBackBufferHeight / 2 - _snakeHeight / 2);
            Color[] data = new Color[snakeRect.Width * snakeRect.Height];
            snakeBlock = new Texture2D(GraphicsDevice, snakeRect.Width, snakeRect.Height);

            for (int i = 0; i < data.Length; ++i)
                data[i] = Color.White;

            snakeBlock.SetData(data);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            KeyboardState state = Keyboard.GetState();

            // If they hit esc, exit
            if (state.IsKeyDown(Keys.Escape))
                Exit();

            // Print to debug console currently pressed keys
            System.Text.StringBuilder sb = new StringBuilder();
            foreach (var key in state.GetPressedKeys())
                sb.Append("Key: ").Append(key).Append(" pressed ");

            if (sb.Length > 0)
                System.Diagnostics.Debug.WriteLine(sb.ToString());
            else
                System.Diagnostics.Debug.WriteLine("No Keys pressed");

            // Change Snake Direction based on key pressed:
            if (state.IsKeyDown(Keys.Right))
                snakeDir = 2; //Right
            if (state.IsKeyDown(Keys.Left))
                snakeDir = 4; //Left
            if (state.IsKeyDown(Keys.Up))
                snakeDir = 1; //Up
            if (state.IsKeyDown(Keys.Down))
                snakeDir = 3; //Down

            switch (snakeDir)
            {
                case 0:
                    break;
                case 1:
                    _snakePosition.Y -= _snakeHeight;
                    break;
                case 2:
                    _snakePosition.X += _snakeWidth;
                    break;
                case 3:
                    _snakePosition.Y += _snakeHeight;
                    break;
                case 4:
                    _snakePosition.X -= _snakeWidth;
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(snakeBlock, _snakePosition, Color.Lime);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
