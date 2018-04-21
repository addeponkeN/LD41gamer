﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer
{
    class GameContent
    {
        static ContentManager content;

        public static Texture2D
            tree,
            player,
            playerSheet,
            compassbox,
            ground,
            acorn,
            enemysheet
            ;

        public static void Load(ContentManager c)
        {
            content = c;
            AnimationManager.Load();
            tree = Texture("tree");
            player = Texture("ekorr");
            playerSheet = Texture("playerSheet");
            compassbox = Texture("compassbox");
            ground = Texture("ground");
            acorn = Texture("acorn");
            enemysheet = Texture("ekorr");
        }

        public static void Unload()
        {
            content.Unload();
            content.Dispose();
            content = null;
        }

        #region CustomLoaders

        // load texture2d
        static Texture2D Texture(string path)
        {
            return content.Load<Texture2D>("Textures/" + path);
        }

        // load sound
        static SoundEffect Sound(string path)
        {
            return content.Load<SoundEffect>(path);
        }

        static SpriteFont Font(string path)
        {
            return content.Load<SpriteFont>("Fonts/" + path);
        }

        #endregion



    }
}
