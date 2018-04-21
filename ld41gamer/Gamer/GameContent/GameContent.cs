using Microsoft.Xna.Framework;
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
            treeInside,
            player,
            playerSheet,
            compassbox,
            ground,
            acorn,
            enemysheet,
            antSheet,

            turret1,
            turret2,
            turretCon,

            botleftBranch,
            botrightBranch,
            topleftBranch,
            toprightBranch,
            
            layer0,
            layer1,
            layer2,
            layer3
            ;

        public static void Load(ContentManager c)
        {
            content = c;
            AnimationManager.Load();
            tree = Texture("tree");
            treeInside = Texture("treeInside");
            player = Texture("ekorr");
            playerSheet = Texture("playerSheet");
            compassbox = Texture("compassbox");
            ground = Texture("ground");
            acorn = Texture("acorn");
            enemysheet = Texture("ekorr");
            antSheet = Texture("antSheet");

            turret1 = Texture("turret1");
            turret2 = Texture("turret2");
            turretCon = Texture("turretCon");

            botleftBranch = Texture("botleftBranch");
            botrightBranch = Texture("botrightBranch");
            topleftBranch = Texture("topleftBranch");
            toprightBranch = Texture("toprightBranch");

            layer0 = Texture("layer0");
            layer1 = Texture("layer1");
            layer2 = Texture("layer2");
            layer3 = Texture("layer3");
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
