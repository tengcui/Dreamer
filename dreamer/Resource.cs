using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
namespace dreamer
{
    public class Resource
    {
        public static Dictionary<string, Texture2D> texture2ds = new Dictionary<string, Texture2D>();

        public static Dictionary<string, Song> songs = new Dictionary<string, Song>();

        public static Dictionary<string, SoundEffect> soundsEffect = new Dictionary<string, SoundEffect>();

        public static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        public static Texture2D AddTexture2D(GraphicsDevice gd, string fileName)
        {
            if (!texture2ds.ContainsKey(fileName))
            {
                FileStream fileStream = new FileStream("Content\\" + fileName + ".png", FileMode.Open);
                texture2ds.Add(fileName, Texture2D.FromStream(gd, fileStream));
            }
            return texture2ds[fileName];
        }

        public static Song AddSong(ContentManager cm, string fileName)
        {
            if (!songs.ContainsKey(fileName))
            {
                songs.Add(fileName, cm.Load<Song>("Sound\\" + fileName));
            }
            return songs[fileName];
        }
        //Add Sound Effect method
        public static SoundEffect AddSounds(ContentManager cm, string fileName)
        {
            if (!soundsEffect.ContainsKey(fileName))
            {
                soundsEffect.Add(fileName, cm.Load<SoundEffect>("Sound\\SoundEffect\\" + fileName));
            }
            return soundsEffect[fileName];
        }
        //Add Font method
        public static SpriteFont AddFont(ContentManager cm, string fileName)
        {
            if (!fonts.ContainsKey(fileName))
            {
                fonts.Add(fileName, cm.Load<SpriteFont>("Font\\" + fileName));
            }
            return fonts[fileName];
        }
    }
}
