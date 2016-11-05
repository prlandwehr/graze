using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Graze
{
    class GRPlayer : GRSprite
    {
        ////
        //FIELDS
        ////

        private float speed;
        public int lives;
        public bool focusOn;
        private bool isInvincible;
        private float invincibleCounter;
        private const float invincibleduration = 2.5f;
        private const float hitradscale = 0.6f; //0.8f
        private const float grazeradscale = 1.8f; //1.8
        public static readonly int STARTING_LIVES = 3;
        private float hitrad;
        private float grazerad;
        private float corescale;
        private Rectangle gamearea;
        private Texture2D pcore;
        private float pcoretimer;

        ////
        //CONSTRUCTORS
        ////

        public GRPlayer()
            : base()
        {
            lives = STARTING_LIVES;
            speed = 325; //450
            focusOn = false;
            isInvincible = false;
            invincibleCounter = invincibleduration;
        }

        public GRPlayer(Rectangle gamearea, ContentManager cm)
            : this()
        {
            this.gamearea = gamearea;
            color = Color.Aquamarine;
            pcoretimer = 0;
            pcore = cm.Load<Texture2D>("coresingle");
            corescale = 1;
            hitrad = (pcore.Width / 2) * hitradscale; //currently basing hit radius on core sprite
        }

        ////
        //METHODS
        ////

        //player texture setter
        public override void setTex(Texture2D tex)
        {
            base.setTex(tex);
            //hitrad = (tex.Width / 2) * hitradscale;
            grazerad = (tex.Width / 2) * grazeradscale; //currently basing graze radius on normal sprite
        }

        //player update method
        public override void Update(GameTime gtime, float gamespeed)
        {
            //Lerp player color when invincible
            if (invincibleCounter < invincibleduration)
            {
                invincibleCounter += (float)gtime.ElapsedGameTime.TotalSeconds;
                if(invincibleCounter < invincibleduration / 2.0f)
                {
                    color = Color.Lerp(Color.Aquamarine, Color.Red,invincibleCounter / (invincibleduration / 2.0f));
                }
                else if(invincibleCounter >= invincibleduration / 2.0f)
                {
                    color = Color.Lerp(Color.Red, Color.Aquamarine, (invincibleCounter - (invincibleduration / 2.0f)) / (invincibleduration / 2.0f));
                }
            }
            else
            {
                isInvincible = false;
            }

            //add player speed to sprite velocity
            velocity *= speed;
            //do focus if relevant
            if (focusOn)
            {
                velocity /= 2;
            }
            if (velocity != Vector2.Zero)
            {
                angle = (float)Math.Atan2(velocity.Y, velocity.X) + (float)Math.PI / 2;
            }

            //pcore update
            pcoretimer += (float)gtime.ElapsedGameTime.TotalSeconds;
            if (pcoretimer <= 0.75f)
            {
                corescale -= (float)gtime.ElapsedGameTime.TotalSeconds /1.55f;
            }
            else
            {
                corescale += (float)gtime.ElapsedGameTime.TotalSeconds /1.55f; 
            }
            if (pcoretimer >= 1.5f)
            {
                pcoretimer = 0;
                corescale = 1;
            }

            //super update - player always moves at gamespeed 1.0f
            base.Update(gtime, 1.0f);

            //correct for offscreen position
            if (position.X < gamearea.Left + sprTx.Width/2)
            {
                position.X = gamearea.Left + sprTx.Width / 2;
            }
            if (position.X > gamearea.Right - sprTx.Width / 2)
            {
                position.X = gamearea.Right - sprTx.Width / 2;
            }
            if (position.Y < gamearea.Top + sprTx.Height / 2)
            {
                position.Y = gamearea.Top + sprTx.Height / 2;
            }
            if (position.Y > gamearea.Bottom - sprTx.Height / 2)
            {
                position.Y = gamearea.Bottom - sprTx.Height / 2;
            }
        }

        //check for collision with a bullet, false if invincible or bullet is fading in/out
        public bool collidesWithBullet(GRBullet bullet)
        {
            if (isInvincible || bullet.fading)
            {
                return false;
            }
            float deltadist = (float) Math.Sqrt( Math.Pow(position.X - bullet.position.X, 2) + Math.Pow(position.Y - bullet.position.Y, 2) );
            if (deltadist <= this.hitrad + bullet.hitrad)
            {
                return true;
            }
            return false;
        }

        //check for graze with a bullet, false if invincible
        public bool grazesBullet(GRBullet bullet)
        {
            if (isInvincible /*|| bullet.fading*/)
            {
                return false;
            }
            float deltadist = (float)Math.Sqrt(Math.Pow(position.X - bullet.position.X, 2) + Math.Pow(position.Y - bullet.position.Y, 2));
            if (deltadist <= this.grazerad + bullet.hitrad)
            {
                return true;
            }
            return false;
        }

        //turn on player's temp. invincibility
        public void setInvincible()
        {
            isInvincible = true;
            invincibleCounter = 0;
        }
        public void setInvincibleOff()
        {
            isInvincible = false;
            invincibleCounter = invincibleduration;
            color = Color.Aquamarine;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(pcore, position, null, Color.White, angle, new Vector2(pcore.Width / 2, pcore.Height / 2), corescale, SpriteEffects.None, 0);
            base.Draw(sb);
        }

    }
}
