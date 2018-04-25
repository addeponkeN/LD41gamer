using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Obo.Utility;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ld41gamer.Gamer
{
    public enum GameSoundType
    {
        Shoot,
        TreeBuilding,
        Song1,
        Song2,

        TowerHit1,
        TowerHit2,
        TowerHit3,

        EnemyHit1,
        EnemyHit2,
        EnemyHit3,

        PlayerShoot1,
        PlayerShoot2,

        TowerBuilding,
        TowerDestroy,

        TowerFling1,
        TowerFling2,
        TowerFling3,

        TowerPlaced,

        TowerShoot1,
        TowerShoot2,
        TowerShoot3
    }

    public class GameSound
    {
        SoundEffect effect;
        SoundEffectInstance[] instance;

        public SoundEffectInstance loop;

        int i = 0;

        public float Volume { get => loop.Volume; set => loop.Volume = value; }

        public GameSound(SoundEffect e, int soundChannels)
        {
            instance = new SoundEffectInstance[soundChannels];
            effect = e;
            loop = effect.CreateInstance();
            for(int i = 0; i < instance.Length; i++)
                instance[i] = effect.CreateInstance();
        }

        public void Play()
        {
            instance[i].Volume = SoundManager.Sfx * SoundManager.Master;
            instance[i].Play();
            i++;
            if(i > instance.Length - 1)
                i = 0;
        }

        public void Loop()
        {
            if(loop.IsLooped)
                return;
            loop.IsLooped = true;
            loop.Play();
        }

        public void Loop(float vol)
        {
            Volume = vol * SoundManager.Master;
            if(loop.IsLooped)
                return;
            loop.IsLooped = true;
            loop.Play();
        }

    }

    public static class SoundManager
    {

        public static float Sfx = 0.5f;
        public static float Music = 0.6f;
        public static float Master = 0.5f;

        public static int SoundChannels = 64;

        public static Dictionary<GameSoundType, GameSound> Sounds;
        static ContentManager c;

        public static void Load(ContentManager cm)
        {
            c = cm;
            Sounds = new Dictionary<GameSoundType, GameSound>();

            AddSound(GameSoundType.TreeBuilding, "buildsound");
            AddSound(GameSoundType.TowerBuilding, "towerBuilding");
            AddSound(GameSoundType.TowerDestroy, "towerDestroy");
            AddSound(GameSoundType.TowerPlaced, "towerPlaced");

            AddSound(GameSoundType.PlayerShoot1, "playerShoot1");
            AddSound(GameSoundType.PlayerShoot2, "playerShoot2");

            AddSound(GameSoundType.TowerHit1, "towerHit1");
            AddSound(GameSoundType.TowerHit2, "towerHit2");
            AddSound(GameSoundType.TowerHit3, "towerHit3");

            AddSound(GameSoundType.TowerShoot1, "towerShoot1");
            AddSound(GameSoundType.TowerShoot2, "towerShoot2");
            AddSound(GameSoundType.TowerShoot3, "towerShoot3");

            AddSound(GameSoundType.TowerFling1, "cataFling1");
            AddSound(GameSoundType.TowerFling2, "cataFling2");
            AddSound(GameSoundType.TowerFling3, "cataFling3");


            AddSound(GameSoundType.EnemyHit1, "enemyHit1");
            AddSound(GameSoundType.EnemyHit2, "enemyHit2");
            AddSound(GameSoundType.EnemyHit3, "enemyHit3");

            AddSound(GameSoundType.Song2, "song2");
        }

        static void AddSound(GameSoundType type, string path)
        {
            var ef = c.Load<SoundEffect>("Sounds/" + path);
            var gs = new GameSound(ef, SoundChannels);
            Sounds.Add(type, gs);
        }

        public static void PlaySound(GameSoundType type)
        {
            Sounds[type].Play();
        }

        public static void PlaySound(GameSoundType type, float vol)
        {
            Sounds[type].Play();
        }

        public static void LoopSound(GameSoundType type)
        {
            Sounds[type].Loop();
        }

        public static void LoopSound(GameSoundType type, float volume)
        {
            Sounds[type].Loop(volume);
        }

        public static void StopLoop(GameSoundType type)
        {
            Sounds[type].loop.IsLooped = false;
            Sounds[type].loop.Stop();
        }

        public static void SetVol(GameSoundType type, float vol)
        {
            Sounds[type].Volume = MathHelper.Clamp(vol, 0f, 1f) * SoundManager.Master;
        }

        public static void PlayEnemyHit()
        {
            var type = (GameSoundType)Rng.Noxt(7, 9);
            PlaySound(type);
        }

        public static void PlayTowerHit()
        {
            var type = (GameSoundType)Rng.Noxt(4, 6);
            PlaySound(type);
        }

        public static void PlayerPlayerShoot()
        {
            var type = (GameSoundType)Rng.Noxt(10, 11);
            PlaySound(type);
        }

        public static void PlayFling()
        {
            var type = (GameSoundType)Rng.Noxt(14, 16);
            PlaySound(type);
        }

        public static void PlayTowerShoot()
        {
            var type = (GameSoundType)Rng.Noxt(18, 20);
            PlaySound(type);
        }
    }
}
