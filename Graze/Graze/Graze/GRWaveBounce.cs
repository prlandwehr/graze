using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Graze
{
    class GRWaveBounce : GRWave
    {
        ////
        //FIELDS
        ////

        private const float maxbulletspin = 3.0f;
        private const int maxbullets = 12;

        ////
        //CONSTRUCTORS
        ////

        public GRWaveBounce()
            : base()
        {
            //
        }

        public GRWaveBounce(int wavenum, Rectangle gamearea, Texture2D waveTex, Color cColor)
        {
            this.wavetime = 0;
            this.wavenum = wavenum;
            this.gamearea = gamearea;
            this.waveTex = waveTex;
            bullets = new ArrayList();
            Random rand = new Random();

            //Init number of bullets
            int numbullets = wavenum + 2;
            if (numbullets > maxbullets)
            {
                numbullets = maxbullets;
            }

            //Create and init bullets
            for (int index = 0; index < numbullets; index++)
            {
                GRBullet abullet = new GRBullet();
                
                abullet.setTex(waveTex);
                abullet.position = new Vector2(
                    rand.Next(gamearea.Left + abullet.sprTx.Width / 2, gamearea.Right - abullet.sprTx.Width / 2),
                    rand.Next(gamearea.Top + abullet.sprTx.Height / 2, gamearea.Bottom - abullet.sprTx.Height / 2));
                abullet.velocity = new Vector2(rand.Next(1,50), rand.Next(1,50));
                abullet.velocity.Normalize();
                abullet.velocity *= GRWave.BULLETSPEED;
                abullet.rotation = (float)rand.NextDouble();
                abullet.rotation = (abullet.rotation - 0.5f) * maxbulletspin;
                abullet.color = cColor;
                abullet.fading = true;
                abullet.opacity = 0;

                bullets.Add(abullet);
            }
        }

        ////
        //METHODS
        ////

        //update for wave, primary focus on bullet behavior
        public override void Update(GameTime gtime, float gamespeed, bool inSlowMo, Color cColor)
        {
            //base wave update
            base.Update(gtime, gamespeed, inSlowMo, cColor);

            //BULLETS
            GRBullet cbullet;
            for (int index = 0; index < bullets.Count; index++)
            {
                cbullet = (GRBullet) bullets[index];

                //bounce bullets off play area edges
                if (cbullet.position.X < gamearea.Left + cbullet.sprTx.Width / 2)
                {
                    cbullet.velocity.X = -cbullet.velocity.X;
                }
                if (cbullet.position.X > gamearea.Right - cbullet.sprTx.Width / 2)
                {
                    cbullet.velocity.X = -cbullet.velocity.X;
                }
                if (cbullet.position.Y < gamearea.Top + cbullet.sprTx.Height / 2)
                {
                    cbullet.velocity.Y = -cbullet.velocity.Y;
                }
                if (cbullet.position.Y > gamearea.Bottom - cbullet.sprTx.Height / 2)
                {
                    cbullet.velocity.Y = -cbullet.velocity.Y;
                }
            }
        }

        //accessor method for wave being over
        public override bool isWaveComplete()
        {
            return (wavetime >= GRWave.WAVEDURATION);
        }

    }
}
