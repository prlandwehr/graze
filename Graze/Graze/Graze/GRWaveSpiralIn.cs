using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Graze
{
    class GRWaveSpiralIn : GRWave
    {
        ////
        //FIELDS
        ////

        private Random rand;
        private float bulletspawntimer;
        private int linedirection;
        private const float maxbulletspin = 3.0f;
        private const float bulletspawninterval = 0.2f;
        private const int numlindirs = 25;

        ////
        //CONSTRUCTORS
        ////

        public GRWaveSpiralIn()
            : base()
        {
            //
        }

        public GRWaveSpiralIn(int wavenum, Rectangle gamearea, Texture2D waveTex, Color cColor)
        {
            linedirection = 0;
            this.wavetime = 0;
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
            double anglevariant = (rand.NextDouble() - 0.5) * Math.PI/2;
            //Create and init bullets
            GRBullet abullet = new GRBullet();

            abullet.setTex(waveTex);
            abullet.position = Vector2.Zero;
            abullet.position.X = gamearea.Center.X + spawnradius * (float)Math.Cos(2 * Math.PI / numlindirs * linedirection);
            abullet.position.Y = gamearea.Center.Y + spawnradius * (float)Math.Sin(2 * Math.PI / numlindirs * linedirection);
            abullet.velocity = Vector2.Zero;
            abullet.velocity.X = -GRWave.BULLETSPEED * (float)Math.Cos(2 * Math.PI / numlindirs * linedirection);
            abullet.velocity.Y = -GRWave.BULLETSPEED * (float)Math.Sin(2 * Math.PI / numlindirs * linedirection);
            abullet.rotation = (float)rand.NextDouble();
            abullet.rotation = (abullet.rotation - 0.5f) * maxbulletspin;
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
        }

        //accessor method for wave being over
        public override bool isWaveComplete()
        {
            return (wavetime >= GRWave.WAVEDURATION);
        }
    }
}
