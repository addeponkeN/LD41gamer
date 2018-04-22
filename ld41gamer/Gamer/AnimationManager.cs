using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer
{

    public enum AnimationType
    {
        None,

        PlayerWalking,
        PlayerJumping,
        PlayerFalling,
        Idle,

        EnemyWalk,
        WaspWalk,

        Hammer,
        BlastCloud,
    }

    public class AnimationManager
    {
        public static Dictionary<AnimationType, Rectangle[]> Dic;

        public static void Load()
        {
            Dic = new Dictionary<AnimationType, Rectangle[]>();

            AddAnimation(new int[] { 0 }, 0, 165, 100, AnimationType.Idle);
            AddAnimation(new int[] { 2 }, 0, 165, 100, AnimationType.PlayerJumping);
            AddAnimation(new int[] { 3 }, 0, 165, 100, AnimationType.PlayerFalling);
            AddAnimation(new int[] { 1, 2, 3 }, 0, 165, 100, AnimationType.PlayerWalking);

            AddAnimation(new int[] { 1, 2, 3 }, 0, 165, 100, AnimationType.EnemyWalk);
            AddAnimation(new int[] { 0, 1, 2 }, 0, 152, 106, AnimationType.WaspWalk);

            AddAnimation(new int[] { 0, 1 }, 0, 100, 100, AnimationType.Hammer);
            AddAnimation(new int[] { 1, 2, 3, 4, 4, 5, 5, 6, 6, 7, 7 }, 0, 221, 123, AnimationType.BlastCloud);
        }

        /// <summary>
        ///  standard spritesize: 256
        /// </summary>
        static void AddAnimation(int[] column, int row, AnimationType type)
        {
            var f = column.Length;
            var frames = new Rectangle[f];
            for(int i = 0; i < f; i++)
                frames[i] = new Rectangle(column[i] * 256, row * 256, 256, 256);
            Dic.Add(type, frames);
        }

        static void AddAnimation(int[] column, int row, int width, int height, AnimationType type)
        {
            var f = column.Length;
            Rectangle[] frames = new Rectangle[f];
            for(int i = 0; i < f; i++)
                frames[i] = new Rectangle(column[i] * width, row * height, width, height);
            Dic.Add(type, frames);
        }

        static void AddAnimation(int[] column, int[] row, int width, int height, AnimationType type)
        {
            var f = column.Length;
            Rectangle[] frames = new Rectangle[f];
            for(int i = 0; i < f; i++)
                frames[i] = new Rectangle(column[i] * width, row[i] * height, width, height);
            Dic.Add(type, frames);
        }

    }
}

