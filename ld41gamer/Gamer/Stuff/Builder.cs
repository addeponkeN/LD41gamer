using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Obo.GameUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer
{
    public class Builder
    {
        public List<Turret> Con;


        public Turret b;

        public Builder()
        {
            Con = new List<Turret>();
        }

        public void Add()
        {

        }

        public void Update(GameTime gt, Map map)
        {
            var dt = gt.Delta();

            map.player.IsBuilding = false;
            foreach(var t in Con)
            {
                if(map.player.CollisionBox.Intersects(t.Rectangle))
                {
                    if(Input.KeyHold(Keys.F))
                    {
                        //  is building
                        t.BuildTime -= dt;
                        map.player.IsBuilding = true;
                        if(t.BuildTime <= 0)
                        {
                            //  build compelte,

                            var turrent = new Turret(t.Type);
                            map.Turrets.Add(turrent);
                        }
                    }
                }
            }
            

            if(Input.KeyClick(Keys.D1))
            {
                b = Create(TowerType.AcornTurret);
            }
            if(Input.KeyClick(Keys.D2))
            {
                b = Create(TowerType.AcornSniper);
            }
            if(Input.KeyClick(Keys.D3))
            {

            }
            if(Input.KeyClick(Keys.D4))
            {

            }


            if(b!= null)
            {
                var p = map.player;
                Vector2 pos;
                if(map.player.LatestDirection == 1)
                {
                    pos = new Vector2(p.CollisionBox.Right, p.CollisionBox.Bottom - b.Size.Y);
                }
                else
                {
                    pos = new Vector2(p.CollisionBox.Left - b.Size.X, p.CollisionBox.Bottom - b.Size.Y);
                }

                b.Position = pos;
                b.SpriteEffects = p.SpriteEffects;

            }


        }

        Turret Create(TowerType type)
        {
            var t = new Turret(type);

            return t;
        }

        public void Draw(SpriteBatch sb)
        {

            foreach(var t in Con)
            {
                //t.Draw(sb);
            }

            b?.Draw(sb);

        }

    }
}
