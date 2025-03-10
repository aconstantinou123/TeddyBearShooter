﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    /// <summary>
    /// A burger
    /// </summary>
    public class Burger
    {
        #region Fields
        

        // graphic and drawing info
        Texture2D sprite;
        Rectangle drawRectangle;

        // burger stats
        int health = 100;

        // shooting support
        bool canShoot = true;
        int elapsedCooldownMilliseconds = 0;

        // sound effect
        SoundEffect shootSound;

        #endregion

        #region Constructors

        /// <summary>
        ///  Constructs a burger
        /// </summary>
        /// <param name="contentManager">the content manager for loading content</param>
        /// <param name="spriteName">the sprite name</param>
        /// <param name="x">the x location of the center of the burger</param>
        /// <param name="y">the y location of the center of the burger</param>
        /// <param name="shootSound">the sound the burger plays when shooting</param>
        public Burger(ContentManager contentManager, string spriteName, int x, int y,
            SoundEffect shootSound)
        {
            LoadContent(contentManager, spriteName, x, y);
            this.shootSound = shootSound;
        }

        #endregion

        #region Properties

       
        /// <summary>
        /// Gets the collision rectangle for the burger
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

        //burger health

        public int Health
        {
            get { return health; }
            set
            {
                health = value;
                if (health <= 0)
                {
                    health = 0;
                }

                if(health >= GameConstants.BurgerInitialHealth)
                {
                    health = GameConstants.BurgerInitialHealth;
                }
            }
        }




        #endregion

        #region Public methods

        /// <summary>
        /// Updates the burger's location based on the keyboad. Also fires 
        /// french fries as appropriate
        /// </summary>
        /// <param name="gameTime">game time</param>
        /// <param name="keyboard">keyboard</param>
        public void Update(GameTime gameTime, KeyboardState keyboard)
        {

            // burger should only respond to input if it still has health

            if (health > 0)
            {
                if (keyboard.IsKeyDown(Keys.W))
                {
                    drawRectangle.Y += GameConstants.BurgerMovementAmount * -1;
                }

                if (keyboard.IsKeyDown(Keys.S))
                {
                    drawRectangle.Y += GameConstants.BurgerMovementAmount;
                }

                if (keyboard.IsKeyDown(Keys.A))
                {
                    drawRectangle.X += GameConstants.BurgerMovementAmount * -1;
                }

                if (keyboard.IsKeyDown(Keys.D))
                {
                    drawRectangle.X += GameConstants.BurgerMovementAmount;
                }

            }
            //                    Change the game so the player no longer uses the mouse to control the burger.Make it so WASD are used to move
            //the burger and space is used to make the burger shoot.You'll have to change the second parameter of the Burger
            //Update method to a KeyboardState parameter to make this work properly. The GameConstants class
            //contains a BurgerMovementAmount constant you should use for the movement part.You do NOT have to avoid
            //the Doom Strafe 40 bug in your code.

            //drawRectangle.X = mouse.X - sprite.Width / 2;
            //drawRectangle.Y = mouse.Y - sprite.Height / 2;
            


            // move burger using mouse

            //drawRectangle.X = mouse.X - sprite.Width / 2;
            //drawRectangle.Y = mouse.Y - sprite.Height / 2;

            // clamp burger in window

            if (drawRectangle.X < 0)
            {
                drawRectangle.X = 0;
            }

            if (drawRectangle.X > GameConstants.WindowWidth)
            {
                drawRectangle.X = GameConstants.WindowWidth - sprite.Width;
            }

            if (drawRectangle.Top < 0)
            {
                drawRectangle.Y = 0;
            }

            if (drawRectangle.Bottom > GameConstants.WindowHeight)
            {
                drawRectangle.Y = GameConstants.WindowHeight - sprite.Height;
            }

            // update shooting allowed

            if (keyboard.IsKeyDown(Keys.Space) && health > 0 && canShoot == true)
            {
                canShoot = false;
                Projectile projectile = new Projectile(ProjectileType.FrenchFries,
                    Game1.GetProjectileSprite(ProjectileType.FrenchFries), 
                    drawRectangle.Center.X - GameConstants.FrenchFriesProjectileOffset, 
                    drawRectangle.Y - GameConstants.FrenchFriesProjectileOffset,
                    GameConstants.FrenchFriesProjectileSpeed);
                Game1.AddProjectile(projectile);
                shootSound.Play();
            }

            if(canShoot == false)
            {
               elapsedCooldownMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedCooldownMilliseconds + gameTime.ElapsedGameTime.Milliseconds > 
                    GameConstants.BurgerTotalCooldownMilliseconds /*|| /*mouse.LeftButton == ButtonState. Released*/)
                {
                    canShoot = true;
                    elapsedCooldownMilliseconds = 0;
                }
            }
        
            // timer concept (for animations) introduced in Chapter 7

            // shoot if appropriate

        }

        /// <summary>
        /// Draws the burger
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, Color.White);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads the content for the burger
        /// </summary>
        /// <param name="contentManager">the content manager to use</param>
        /// <param name="spriteName">the name of the sprite for the burger</param>
        /// <param name="x">the x location of the center of the burger</param>
        /// <param name="y">the y location of the center of the burger</param>
        private void LoadContent(ContentManager contentManager, string spriteName,
            int x, int y)
        {
            // load content and set remainder of draw rectangle
            sprite = contentManager.Load<Texture2D>(spriteName);
            drawRectangle = new Rectangle(x - sprite.Width / 2,
                y - sprite.Height / 2, sprite.Width,
                sprite.Height);
        }

        #endregion
    }
}
