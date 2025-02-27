using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Sandbox.Oyun.Sahneler;

namespace Sandbox.Engine.Scene
{
    public class SceneManager
    {
        public Scenes ActiveScene { get; set; }
        private readonly Dictionary<Scenes, Scene> _scenes = new();

        public SceneManager()
        {
            _scenes.Add(Scenes.MainMenu, new MainMenu());
            _scenes.Add(Scenes.CinematicEntry, new CinematicEntry());
            _scenes.Add(Scenes.Gameplay, new Gameplay());

            ActiveScene = Scenes.MainMenu;
        }

        public void SwitchScene(Scenes scene, Game game1)
        {
            if (_scenes.ContainsKey(scene))
            {
                // CLEAN
                Globals.entities.Clear(); // Cleaning previous scene entities

                ActiveScene = scene;

                // Initialize and load content for the new scene
                _scenes[ActiveScene].Initialize();
                _scenes[ActiveScene].LoadContent(game1);
            }
            else
            {
                // Handle the case where the scene does not exist in the dictionary
                Console.WriteLine($"Scene: {scene} does not exist.");
            }
        }

        public void Initialize()
        {
            _scenes[ActiveScene].Initialize();
        }

        public void LoadContent(Game game1) //  TODO: ADD UNLOAD CONTENT???? Memory Management
        {
            _scenes[ActiveScene].LoadContent(game1);
        }

        public void Update(GameTime gameTime, Game game1)
        {
            _scenes[ActiveScene].Update(gameTime, game1);
        }
        
        public void Draw(GameTime gameTime, Game game1)
        {
            _scenes[ActiveScene].Draw(gameTime, game1);
        }
    }
}
