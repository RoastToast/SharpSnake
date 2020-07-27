using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
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

        private const int _blockWidth = 16;
        private const int _blockHeight = 16;

        private int _GameWidth = 30;
        private int _GameHeight = 20;

        private int _widthOffset = 0;
        private int _heightOffset = 0;

        Texture2D _texture;
        Rectangle _playField;

        int delay = 0;

        int snakeDir = 0; // 1,2,3,4 north, east, south, west
        Texture2D snakeBlock;
        Vector2 _snakePosition;
        List<Vector2> _snakeTail;

        Vector2 _applePosition;

        Random rand;

        public SnakeGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _snakeTail = new List<Vector2>();

            _playField = new Rectangle(_widthOffset, _heightOffset, _GameWidth * _blockWidth, _GameHeight * _blockHeight);

            rand = new Random();
            _applePosition = new Vector2();
            _applePosition.X = rand.Next(1, _GameWidth) * _blockWidth - _blockWidth / 2;
            _applePosition.Y = rand.Next(1, _GameHeight) * _blockHeight - _blockHeight / 2;
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

            _texture = new Texture2D(GraphicsDevice, 1, 1);
            _texture.SetData(new Color[] { Color.DarkSlateGray });

            Rectangle snakeRect = new Rectangle(
                16 * _blockHeight,
                11 * _blockWidth,
                _blockWidth - 1,
                _blockHeight - 1
            );
            _snakePosition = new Vector2(this.graphics.PreferredBackBufferWidth / 2 - _blockWidth / 2,
                this.graphics.PreferredBackBufferHeight / 2 - _blockHeight / 2);
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
            if (_snakePosition.X == _applePosition.X && _snakePosition.Y == _applePosition.Y)
            {
                _applePosition.X = rand.Next(0, _GameWidth) * _blockWidth - _blockWidth / 2;
                _applePosition.Y = rand.Next(0, _GameHeight) * _blockHeight - _blockHeight / 2;
                Vector2 newTail = new Vector2();
                switch (snakeDir)
                {
                    case 1:
                        newTail.X = _snakePosition.X;
                        newTail.Y = _snakePosition.Y + _blockHeight;
                        break;
                    case 2:
                        newTail.X = _snakePosition.X - _blockWidth;
                        newTail.Y = _snakePosition.Y;
                        break;
                    case 3:
                        newTail.X = _snakePosition.X;
                        newTail.Y = _snakePosition.Y - _blockHeight;
                        break;
                    case 4:
                        newTail.X = _snakePosition.X + _blockWidth;
                        newTail.Y = _snakePosition.Y;
                        break;
                }
                _snakeTail.Add(newTail);
            }

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
            if (delay == 7)
            {
                delay = 0;
                switch (snakeDir)
                {

                    case 0:
                        break;
                    case 1:
                        updateTail();
                        _snakePosition.Y -= _blockHeight;
                        checkSnake();
                        break;
                    case 2:
                        updateTail();
                        _snakePosition.X += _blockWidth;
                        checkSnake();
                        break;
                    case 3:
                        updateTail();
                        _snakePosition.Y += _blockHeight;
                        checkSnake();
                        break;
                    case 4:
                        updateTail();
                        _snakePosition.X -= _blockWidth;
                        checkSnake();
                        break;
                }
            }
            else
            {
                delay++;
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

            spriteBatch.Draw(_texture, _playField, Color.White);

            spriteBatch.Draw(snakeBlock, _applePosition, Color.Red);
            spriteBatch.Draw(snakeBlock, _snakePosition, Color.Lime);
            for (int i = 0; i < _snakeTail.Count; i++)
            {
                spriteBatch.Draw(snakeBlock, _snakeTail[i], Color.Lime);
            }

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        protected void updateTail()
        {
            if (_snakeTail.Count == 1)
                _snakeTail[0] = _snakePosition;
            if (_snakeTail.Count > 1)
            {
                for (int i = 0; i < _snakeTail.Count - 1; i++)
                {
                    _snakeTail[i] = _snakeTail[i + 1];
                }
                _snakeTail[_snakeTail.Count - 1] = _snakePosition;
            }
        }
        protected void checkSnake()
        {
            if (_snakePosition.X > (_GameWidth * _blockWidth) || _snakePosition.X < 0)
            {
                Console.WriteLine(_snakePosition.X);
                Console.WriteLine((_GameWidth * _blockWidth));
                Exit();
            }
            if (_snakePosition.Y > (_GameHeight * _blockHeight) || _snakePosition.Y < 0)
            {
                Console.WriteLine(_snakePosition.Y);
                Console.WriteLine((_GameHeight * _blockHeight));
                Exit();
            }
            if (_snakeTail.Contains(_snakePosition))
            {
                Console.WriteLine(_snakePosition.X + _snakePosition.Y);
                Console.WriteLine(_snakeTail.IndexOf(_snakePosition));
                Exit();
            }
        }
    }
}
