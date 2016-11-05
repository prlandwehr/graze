using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Graze
{
    class GRWaveHoming : GRWave
    {
        ////
        //FIELDS
        ////

        private Random rand;
        private float bulletspawntimer;
        private int linedirection;
        private GRPlayer player;
        private const float turnrate = 0.025f;
        private const float maxbulletspin = 3.0f;
        private const float bulletspawninterval = 2.5f;
        private const int numlindirs = 8;

        ////
        //CONSTRUCTORS
        ////

        public GRWaveHoming()
            : base()
        {
            //
        }

        public GRWaveHoming(int wavenum, Rectangle gamearea, Texture2D waveTex, Color cColor, GRPlayer player)
        {
            linedirection = 0;
            this.wavetime = 0;
            this.player = player;
            this.wavenum = wavenum;
            this.gamearea = gamearea;
            this.waveTex = waveTex;
            bullets = new ArrayList();
            rand = new Random();

            bulletspawntimer = 0;
            
            //Create and init bullets
            spawnBullet(cColor);
        }

        ////
        //METHODS
        ////

        private void spawnBullet(Color cColor)
        {
            //reset timer
            bulletspawntimer = 0;
            //direction, distance from center init
            linedirection = (linedirection+1) % numlindirs;
            float spawnradius = (float)Math.Sqrt(Math.Pow(gamearea.Right-gamearea.Center.X,2) + Math.Pow(gamearea.Bottom-gamearea.Center.Y,2));
            //
            //Create and init bullets
            GRBullet abullet = new GRBullet();

            abullet.setTex(waveTex);
            abullet.position = Vector2.Zero;
            abullet.position.X = gamearea.Center.X + spawnradius * (float)Math.Cos(2 * Math.PI / numlindirs * linedirection);
            abullet.position.Y = gamearea.Center.Y + spawnradius * (float)Math.Sin(2 * Math.PI / numlindirs * linedirection);
            abullet.velocity = Vector2.Zero;
            abullet.velocity.X = -GRWave.BULLETSPEED * (float)Math.Cos(2 * Math.PI / numlindirs * linedirection);
            abullet.velocity.Y = -GRWave.BULLETSPEED * (float)Math.Sin(2 * Math.PI / numlindirs * linedirection);
            abullet.angle = (float)Math.Atan2(player.position.Y - abullet.position.Y, player.position.X - abullet.position.X);
            //
            abullet.color = cColor;
            abullet.fading = true;

            bullets.Add(abullet);
        }

        //update for wave, primary focus on bullet behavior
        public override void Update(GameTime gtime, float gamespeed, bool inSlowMo, Color cColor)
        {
            //base wave update
            base.Update(gtime, gamespeed, inSlowMo, cColor);

            //bullet spawn code
            bulletspawntimer += (float)gtime.ElapsedGameTime.TotalSeconds;
            if (bulletspawntimer > bulletspawninterval / gamespeed)
            {
                spawnBullet(cColor);
            }

            //BULLETS
            GRBullet cbullet;
            for (int index = 0; index < bullets.Count; index++)
            {
                cbullet = (GRBullet) bullets[index];

                //home in on player
                float desiredAngle = (float)Math.Atan2(player.position.Y - cbullet.position.Y, player.position.X - cbullet.position.X);
                float difference = WrapAngle(desiredAngle - cbullet.angle);
                difference = MathHelper.Clamp(difference, -turnrate, turnrate);
                cbullet.angle = WrapAngle(cbullet.angle + difference);

                cbullet.velocity.X = GRWave.BULLETSPEED * (float)Math.Cos(cbullet.angle);
                cbullet.velocity.Y = GRWave.BULLETSPEED * (float)Math.Sin(cbullet.angle);
            }
        }

        //accessor method for wave being over
        public override bool isWaveComplete()
        {
            return (wavetime >= GRWave.WAVEDURATION);
        }

        /// <summary>
        /// Returns the angle expressed in radians between -Pi and Pi.
        /// </summary>
        private static float WrapAngle(float radians)
        {
            while (radians < -MathHelper.Pi)
            {
                radians += MathHelper.TwoPi;
            }
            while (radians > MathHelper.Pi)
            {
                radians -= MathHelper.TwoPi;
            }
            return radians;
        }
    }
}
