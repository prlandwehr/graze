using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Graze
{
    class GRWaveSwirl : GRWave
    {
        ////
        //FIELDS
        ////

        private ArrayList spirals;
        private float bulletspawntimer;
        private Vector2[] spiralcenter;
        private Random rand;
        private int numspirals;
        private int extrabullettoggle = 0;
        private int bulletsperburst = 8;
        private const float bulletspin = 3.0f;
        private const float bulletspawninterval = 2.0f;
        private const float spiralvelmod = 0.5f;
        private const float spiralthetamod = 0.25f;

        ////
        //CONSTRUCTORS
        ////

        public GRWaveSwirl()
            : base()
        {
            //
        }

        public GRWaveSwirl(int wavenum, Rectangle gamearea, Texture2D waveTex, Color cColor)
        {
            rand = new Random();
            bullets = new ArrayList();
            spirals = new ArrayList();
            if (wavenum < 5)
            {
                numspirals = 1;
            }
            else
            {
                numspirals = 2;
            }
            this.wavetime = 0;
            this.wavenum = wavenum;
            this.gamearea = gamearea;
            this.waveTex = waveTex;
            bulletspawntimer = 0;
            spiralcenter = new Vector2[numspirals];
            for (int index = 0; index < numspirals; index++)
            {
                spiralcenter[index] = new Vector2(rand.Next(gamearea.Left, gamearea.Right), rand.Next(gamearea.Top, gamearea.Bottom));
                spirals.Add(new ArrayList());
            }
            spawnburst(cColor);
        }

        ////
        //METHODS
        ////

        private void spawnburst(Color cColor)
        {
            bulletspawntimer = 0;
            //
            extrabullettoggle = -extrabullettoggle;
            bulletsperburst += extrabullettoggle;
            //
            for (int burstind = 0; burstind < numspirals; burstind++)
            {
                ArrayList cspiral = (ArrayList)spirals[burstind];
                for (int index = 0; index < bulletsperburst; index++)
                {
                    GRBullet abullet = new GRBullet();

                    abullet.setTex(waveTex);
                    abullet.position = new Vector2(spiralcenter[burstind].X - waveTex.Width / 2, spiralcenter[burstind].Y - waveTex.Height / 2);
                    abullet.spiraltheta = 2 * (float)Math.PI / bulletsperburst * index;

                    abullet.rotation = bulletspin;
                    abullet.color = cColor;
                    abullet.fading = true;
                    //add bullet both to its spiral(for updates) and the masterlist(for rendering)
                    cspiral.Add(abullet);
                    bullets.Add(abullet);
                }
                //spirals.Add(cBurst);
            }
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
                spawnburst(cColor);
            }

            //BULLETS
            for (int burstind = 0; burstind < spirals.Count; burstind++)
            {
                ArrayList cspiral = (ArrayList)spirals[burstind];

                GRBullet cbullet;
                for (int index = 0; index < cspiral.Count; index++)
                {
                    cbullet = (GRBullet)cspiral[index];

                    //movement
                    cbullet.spiraltheta += spiralthetamod * gamespeed * (float)gtime.ElapsedGameTime.TotalSeconds;
                    cbullet.spiralradius += spiralvelmod * gamespeed * (float)gtime.ElapsedGameTime.TotalSeconds;
                    cbullet.position.X = spiralcenter[burstind].X - GRWave.BULLETSPEED * cbullet.spiralradius * (float)Math.Cos(cbullet.spiraltheta);
                    cbullet.position.Y = spiralcenter[burstind].Y - GRWave.BULLETSPEED * cbullet.spiralradius * (float)Math.Sin(cbullet.spiraltheta);

                    //remove bullet if offscreen
                    //(no longer in use)
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
