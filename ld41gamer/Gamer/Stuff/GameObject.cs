using ld41gamer.Gamer.Screener;
using ld41gamer.Gamer.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Obo.GameUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer
{
    public class GameObject : AnimatedSprite
    {

        public Vector2 Center => Position + (Size * .5f);

        public Vector2 Direction;
        public float Speed;


        #region COLLISIONS
        public Vector2 CollisionSize { get; set; }
        public void SetCollisionCenter(int x, int y)
        {
            CollisionSize = new Vector2(x, y);
            CollisionOrigin = new Vector2(Size.X / 2, Size.Y / 2);
        }
        public void SetCollisionCenter(int xy)
        {
            CollisionSize = new Vector2(xy, xy);
            CollisionOrigin = new Vector2(Size.X / 2, Size.Y / 2);
        }
        public void SetCollisionBot(int x, int y)
        {
            CollisionSize = new Vector2(x, y);
            CollisionOrigin = new Vector2((Size.X / 2), Size.Y - (CollisionSize.Y / 2));
        }
        public void SetCollisionBot(int xy)
        {
            CollisionSize = new Vector2(xy, xy);
            CollisionOrigin = new Vector2((Size.X / 2), Size.Y - (CollisionSize.Y / 2));
        }

        public Vector2 CollisionOrigin { get; set; }
        public Vector2 CollisionPosition => new Vector2(Position.X + CollisionOrigin.X, Position.Y + CollisionOrigin.Y);

        public Rectangle CollisionBox => new Rectangle(
                (int)(Position.X + CollisionOrigin.X - (CollisionSize.X / 2)),
                (int)(Position.Y + CollisionOrigin.Y - (CollisionSize.Y / 2)),
                (int)CollisionSize.X, (int)CollisionSize.Y);
        #endregion


        public virtual void Update(GameTime gt, Map map, GameScreen gs)
        {
            UpdateAnimation(gt);

        }

        public virtual void UpdatePosition(GameTime gt)
        {
            var dt = gt.Delta();
            Position += Speed * Direction * dt;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

        }

        public virtual void DrawShadow(SpriteBatch sb, float xoff = 0, float yoff = 0, float woff = 0f, float hoff = 0f)
        {
            var size = new Vector2((CollisionBox.Width * 1.5f) + woff, 6 + (CollisionBox.Height / 3) + hoff);

            float x = GHelper.Center(CollisionBox, size).X;
            float y;

            if(CollisionBox.Bottom < Map.GroundCollisionBox.Top)
            {
                y = Map.GroundCollisionBox.Top - (size.Y / 3);
            }
            else
                y = CollisionBox.Bottom - (size.Y / 3);

            var pos = new Vector2(x, y);

            int alpha = 150 * (Map.GroundCollisionBox.Top / 1300);
            var color = new Color(alpha, alpha, alpha, alpha);

            sb.Draw(GameContent.shadow, new Rectangle(pos.ToPoint(), size.ToPoint()), color, Layer.Shadow);
        }

    }
}
