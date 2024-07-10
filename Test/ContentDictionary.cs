using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Test
{
    static class ContentDictionary
    {
        public static Dictionary<string, SoundEffect> SoundDict { get; private set; }
        public static Dictionary<string, Texture2D> TextureDict { get; private set; }
        public static Dictionary<string, SpriteFont> FontDict { get; private set; }
        public static Dictionary<string, Texture2D> AnimationDict { get; private set; }
        public static Dictionary<string, Song> MusicDict { get; private set; }

        /// <summary>
        /// Initializes the static ContentDictionary class
        /// with empty Dictionarys
        /// </summary>
        static ContentDictionary()
        {
            SoundDict = new Dictionary<string, SoundEffect>();
            TextureDict = new Dictionary<string, Texture2D>();
            FontDict = new Dictionary<string, SpriteFont>();
            AnimationDict = new Dictionary<string, Texture2D>();
            MusicDict = new Dictionary<string, Song>();
        }

        private static bool IsInAnimationFolder(string filename)
        {
            // Überprüfen, ob die Datei im Ordner "Animation" oder in einem Unterordner davon liegt
            string animationFolder = "Animation" + Path.DirectorySeparatorChar;
            return filename.Contains(animationFolder);
        }


        /// <summary>
        /// Loads game content from the specified list of content files.
        /// </summary>
        /// <param name="contentFiles">List of content file names to load.</param>
        /// <param name="contentManager">The ContentManager to use for loading content.</param>
        public static void LoadContent(List<string> contentFiles, ContentManager contentManager)
        {
            foreach (string contentFile in contentFiles)
            {
                string filename = contentFile.ToString();

                if (filename.EndsWith(".wav"))
                {
                    SoundEffect soundEffect = contentManager.Load<SoundEffect>(filename.Substring(0, filename.Length - 4));
                    SoundDict.Add(filename, soundEffect);
                }
                else if (filename.EndsWith(".mp3"))
                {
                    Song song = contentManager.Load<Song>(filename.Substring(0, filename.Length - 4));
                    MusicDict.Add(filename, song);
                }
                else if (filename.EndsWith("Button.png"))
                {
                    string path = Path.Combine("Buttons", (filename.Substring(0, filename.Length - 4)));
                    Texture2D texture = contentManager.Load<Texture2D>(path);
                    TextureDict.Add(filename, texture);
                }
                else if (filename.EndsWith(".png") || filename.EndsWith(".jpg"))
                {
                    Texture2D texture = contentManager.Load<Texture2D>(filename.Substring(0, filename.Length - 4));
                    TextureDict.Add(filename, texture);
                }
                else if (filename.EndsWith(".ttf"))
                {
                    SpriteFont font = contentManager.Load<SpriteFont>(filename.Substring(0, filename.Length - 4));
                    FontDict.Add(filename, font);
                }
                else if (filename.EndsWith(".spritefont"))
                {
                    SpriteFont spriteFont = contentManager.Load<SpriteFont>(filename.Substring(0, filename.Length - 11));
                    FontDict.Add(filename, spriteFont);
                }
            }
        }
    }
}