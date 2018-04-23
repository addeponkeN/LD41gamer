using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Threading;

namespace ld41gamer.Gamer
{
    public enum GameSoundType
    {
        Shoot,
        Building,
        Song1,
        Song2
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
        public static float Music = 0.5f;
        public static float Master = 0.5f;

        public static int SoundChannels = 16;

        public static Dictionary<GameSoundType, GameSound> Sounds;
        static ContentManager c;

        public static void Load(ContentManager cm)
        {
            c = cm;
            Sounds = new Dictionary<GameSoundType, GameSound>();

            AddSound(GameSoundType.Building, "buildsound");

            AddSound(GameSoundType.Song1, "song1");
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
            Sounds[type].loop.Stop();
        }

        public static void SetVol(GameSoundType type, float vol)
        {
            Sounds[type].Volume = MathHelper.Clamp(vol, 0f, 1f);
        }

    }
}
