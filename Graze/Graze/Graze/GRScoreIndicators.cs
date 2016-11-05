using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Graze
{
    class GRScoreIndicators
    {
        class GRPlusScoreObj
        {
            public string plusscorestring;
            public Vector2 position;
            public Vector2 offset;
            public float opacity;
            public float TTL;
            public const float startingTTL = 1.5f;
            public const float offsetacc = -2.0f;
            public Color textcolor; //render color (color + opacity)

            public GRPlusScoreObj(float score, GRPlayer player)
            {
                plusscorestring = "+" + (int)score;
                position = player.position;
                offset = Vector2.Zero;
                opacity = 255;
                TTL = startingTTL;
            }
        }
        private ArrayList scoreObjs;

        public GRScoreIndicators()
        {
            scoreObjs = new ArrayList();
        }

        public void addScore(float score, GRPlayer player)
        {
            scoreObjs.Add(new GRPlusScoreObj(score, player));
        }

        public void Update(GameTime gTime, GRPlayer player)
        {
            for (int index = 0; index < scoreObjs.Count; index++)
            {
                GRPlusScoreObj cScore = (GRPlusScoreObj)scoreObjs[index];

                cScore.offset.Y += (float)gTime.ElapsedGameTime.TotalSeconds * GRPlusScoreObj.offsetacc;
                cScore.position += cScore.offset;

                //fade score out over time (currently lerp)
                cScore.opacity = 1.0f - ((GRPlusScoreObj.startingTTL - cScore.TTL) / GRPlusScoreObj.startingTTL);

                //remove if dead
                cScore.TTL -= (float)gTime.ElapsedGameTime.TotalSeconds;
                if (cScore.TTL <= 0)
                {
                    scoreObjs.RemoveAt(index);
                    index--;
                }
            }
        }

        public void Draw(SpriteBatch sb, SpriteFont sf, Color color)
        {
            for (int index = 0; index < scoreObjs.Count; index++)
            {
                GRPlusScoreObj cScore = (GRPlusScoreObj)scoreObjs[index];

                cScore.textcolor = color * cScore.opacity;
                
                sb.DrawString(sf, cScore.plusscorestring, new Vector2(cScore.position.X + 25, cScore.position.Y - 40), cScore.textcolor);       
            }
        }
    }
}
