using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Obo.GameUtility;
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
            playerSheet,
            compassbox,
            ground,
            acorn,
            cone,
            particlesheet,
            hammer,
            blastcloud,

            antSheet,
            waspSheet,
            wormSheet,
            wormHole,
            beaverSheet,

            turretsheet,

            icons,

            botleftBranch,
            botrightBranch,
            topleftBranch,
            toprightBranch,
            
            layer0,
            layer1,
            layer2,
            layer3,

            shadow,

            bgmenu,
            title,
            btplank,
            bigplank
            ;

        public static SpriteFont font12, font14, font24, font48;

        public static void Load(ContentManager c, GraphicsDevice gd)
        {
            content = c;
            AnimationManager.Load();
            tree = Texture("tree");
            treeInside = Texture("treeInside");
            playerSheet = Texture("playerSheet");
            compassbox = Texture("compassbox");
            ground = Texture("ground");
            acorn = Texture("acorn");
            cone = Texture("cone");
            hammer = Texture("hammer");
            blastcloud = Texture("blastcloud");

            antSheet = Texture("antSheet");
            waspSheet = Texture("waspSheet");
            wormSheet = Texture("wormSheet");
            wormHole = Texture("wormHole");
            beaverSheet = Texture("beaverSheet");

            turretsheet = Texture("turretsheet");
            particlesheet = Texture("particlesheet");

            icons = Texture("icons");

            botleftBranch = Texture("botleftBranch");
            botrightBranch = Texture("botrightBranch");
            topleftBranch = Texture("topleftBranch");
            toprightBranch = Texture("toprightBranch");

            layer0 = Texture("layer0");
            layer1 = Texture("layer1");
            layer2 = Texture("layer2");
            layer3 = Texture("layer3");

            font12 = Font("font14");
            font14 = Font("font14");
            font24 = Font("font24");
            font48 = Font("font48");

            btplank = Texture("plank");
            bigplank = Texture("bigplank");

            shadow = Extras.DrawCircle(gd, 400, .25f, Color.Black);
        }

        public static void LoadMenu(ContentManager c)
        {
            content = c;

            title = Texture("title");
            bgmenu = Texture("bgmenu");

            btplank = Texture("plank");
            bigplank = Texture("bigplank");

            font12 = Font("font12");
            font14 = Font("font14");
            font24 = Font("font24");
            font48 = Font("font48");
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
