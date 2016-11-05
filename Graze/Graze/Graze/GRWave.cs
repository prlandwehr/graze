using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Graze
{
    class GRWave
    {
        ////
        //FIELDS
        ////

        //bullets should contain all bullets in the wave the game needs to draw/check collisions on
        public ArrayList bullets;
        protected int wavenum;
        protected Rectangle gamearea;
        protected Texture2D waveTex;
        protected float wavetime;
        protected static readonly float BULLETSPEED = 100;
        protected static readonly float WAVEDURATION = 15.0f;
        protected static readonly float FADETIME = 1.1f;

        ////
        //CONSTRUCTORS
        ////

        public GRWave()
        {
            bullets = new ArrayList();
            wavetime = 0;
        }

        public GRWave(int wavenum, Rectangle gamearea) 
            : this()
        {
            this.wavetime = 0;
            this.wavenum = wavenum;
            this.gamearea = gamearea;
        }

        ////
        //METHODS
        ////

        //inSlowMode should not be used to modify bullets directly, gamespeed is for that purpose
        //instead it can be used to modify the spawn interval for new bullets where applicable
        public virtual void Update(GameTime gtime, float gamespeed, bool inSlowMo, Color cColor)
        {
            //wave timer update
            wavetime += (float)gtime.ElapsedGameTime.TotalSeconds;

            //bullets update
            GRBullet cbullet;
            for (int index = 0; index < bullets.Count; index++)
            {
                cbullet = (GRBullet)bullets[index];

                //set bullet to current bullet color
                cbullet.color = cColor;
                //apply fading if wave is fading in or out
                if (wavetime < GRWave.FADETIME)
                {
                    cbullet.opacity = wavetime / GRWave.FADETIME;
                    cbullet.fading = true;
                }
                else if (Math.Abs(GRWave.WAVEDURATION - wavetime) < GRWave.FADETIME)
                {
                    cbullet.opacity = Math.Abs((GRWave.WAVEDURATION - wavetime) / GRWave.FADETIME);
                    cbullet.fading = true;
                }
                else
                {
                    cbullet.fading = false;
                }

                //bullet update call
                cbullet.Update(gtime, gamespeed);
            }
        }

        public virtual bool isWaveComplete()
        {
            return (wavetime >= GRWave.WAVEDURATION);
        }
    }
}
