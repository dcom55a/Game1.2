using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Game1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        InputEventHandler _inputEventHandler = new InputEventHandler();
        private PhysicsHandler _physicsHandler ;
        private PhyModel _phyModel;
        

        public List<PhyObject> ObjectList;
        Texture2D sprBall;
        SpriteFont font;
        Vector2 velocity = new Vector2(0, 0);

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _physicsHandler = new PhysicsHandler(GraphicsDevice.PresentationParameters.Bounds.Width, GraphicsDevice.PresentationParameters.Bounds.Height);
            ObjectList = new List<PhyObject>();
            Hitbox PlayerHitbox = new Hitbox("circle", 10f);
            Texture2D PlayerTexture = this.Content.Load<Texture2D>("Spr_Ball");
            PhyObject player = new PhyObject("player", PlayerHitbox, PlayerTexture, new Vector2(0, 0), new Vector2(100, 100), 1, true);
            _physicsHandler.phyModel.ObjList.Add(player);

            float s = MathFunctions.IntersecFindS(new Vector2(0, 0), new Vector2(5, 5), new Vector2(2.5f, 0), new Vector2(0, 5));

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            sprBall = this.Content.Load<Texture2D>("Spr_Ball");
            font = this.Content.Load<SpriteFont>("File");

        }

        protected override void Update(GameTime gameTime)
        {
            //exit game if Esc is pressed
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            { Exit(); }

            //get player input
            Vector2 swing = _inputEventHandler.GetSwing();

            //calculate Physics
            _physicsHandler.UpdatePhysics(gameTime,swing);
            
            base.Update(gameTime); 
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LawnGreen);

            SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteBatch.Begin();
            if (ObjectList != null)
            {
                foreach (PhyObject obj in _physicsHandler.phyModel.ObjList)
                {
                    if (obj.Hitbox.Shape == "circle")
                    {
                        spriteBatch.Draw(texture: obj.Texture,
                                         destinationRectangle: new Rectangle((int)obj.Position.X, 
                                                                             (int)obj.Position.Y, 
                                                                             (int)obj.Hitbox.Height * 2,
                                                                             (int)obj.Hitbox.Height * 2),
                                         color: Color.White);
                    }
                }
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}