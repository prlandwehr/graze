using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Graze
{
    class GRSprite
    {
        ////
        //FIELDS
        ////

        public Vector2 position;
        public Vector2 velocity;
        public Vector2 acceleration;
        public Texture2D sprTx; //primary sprite texture
        public float angle; //texture angle, radians
        public Color color; //used to tint when drawing
        private Color alphacolor; //render color (color + opacity)
        public float opacity;
        public float TTL;
        public bool isTimed;
        public bool isAlive;
        public Rectangle dispRect;
        private Vector2 origin; //used to center texture on position when drawing/rotating

        ////
        //CONSTRUCTORS
        ////

        public GRSprite()
        {
            position = Vector2.Zero;
            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;
            sprTx = null;
            angle = 0;
            color = Color.White;
            TTL = 0;
            isTimed = false;
            isAlive = true;
            dispRect = new Rectangle(0, 0, 0, 0);
            origin = Vector2.Zero;
            opacity = 1;
        }

        public GRSprite(Texture2D spr) 
            : this()
        {
            setTex(spr);
        }

        ////
        //METHODS
        ////

        public virtual void Update(GameTime gtime, float gamespeed)
        {
            float dt = (float)gtime.ElapsedGameTime.TotalSeconds;

            if (isTimed)
            {
                TTL -= dt;
                if (TTL <= 0)
                {
                    isAlive = false;
                }
            }

            position += velocity * dt * gamespeed;
            velocity += acceleration * dt * gamespeed;

            //display rect currently unused
            dispRect.X = (int)position.X;
            dispRect.Y = (int)position.Y;
        }

        public virtual void setTex(Texture2D tex) {
            sprTx = tex;
            origin = new Vector2(sprTx.Width/2,sprTx.Height/2);
        }

        public virtual void Draw(SpriteBatch sb)
        {
            alphacolor = this.color * this.opacity;
            sb.Draw(sprTx, position, null, alphacolor, angle, origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}
