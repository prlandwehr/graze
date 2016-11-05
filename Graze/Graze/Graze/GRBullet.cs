using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Graze
{
    class GRBullet : GRSprite
    {
        ////
        //FIELDS
        ////

        public float hitrad;
        public float rotation;
        // hackish vars for spiral wave
        public float spiraltheta;
        public float spiralradius;
        ////
        public bool fading;
        public bool grazeCooldown;
        private const float grazeduration = 1.0f;
        private const float hitradmultiplier = 0.85f;
        private float grazetimer;

        ////
        //CONSTRUCTORS
        ////

        public GRBullet()
            : base()
        {
            fading = false;
            rotation = 0;
            grazeCooldown = false;
            grazetimer = 0;
            spiraltheta = 0;
            spiralradius = 0;
        }

        ////
        //METHODS
        ////

        public override void setTex(Texture2D tex)
        {
            base.setTex(tex);
            hitrad = (sprTx.Width / 2) * hitradmultiplier;
        }

        public override void Update(GameTime gtime, float gamespeed)
        {

            if (grazeCooldown)
            {
                grazetimer += (float)gtime.ElapsedGameTime.TotalSeconds;
                if (grazetimer >= grazeduration)
                {
                    grazeCooldown = false;
                    grazetimer = 0;
                }
            }
            base.Update(gtime, gamespeed);
            this.angle += rotation * (float)gtime.ElapsedGameTime.TotalSeconds;
        }
    }
}
