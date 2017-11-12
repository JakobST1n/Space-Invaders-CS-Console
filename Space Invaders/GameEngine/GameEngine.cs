using System;
using System.Collections.Generic;
using System.Threading;

namespace GameEngine {
    
    abstract class Game {

        virtual public int ConsoleHeight { get; set; }
        virtual public int ConsoleWidth { get; set; }

        public List<GameObject> GameObjects = new List<GameObject>();
        public Thread _listnerThread = new Thread(Input.Listner);

        public Random Rand = new Random();

        private Graphics _graphics;
        private bool _running;

        abstract public void Setup();

        virtual public void Start() {
            // Do initialization
            _graphics = new Graphics(ConsoleHeight, ConsoleWidth);

            // Run start thing in all GameObjects
            for (int i = 0; i < GameObjects.Count; i++) {
                GameObjects[i].Start();
            }

            // Start loop and other threads
            _running = true;           // This is a notifier to every thread, that will help us close the app
            _listnerThread.Start();
            GameLoop();
        }

        private void GameLoop() {
            while (_running) {
                
                // Run the Update method in each GameObject in the scene
                for (int i = 0; i < GameObjects.Count; i++) {
                    GameObjects[i].Update();
                }
                
                // Scale every GameObject
                for (int i = 0; i < GameObjects.Count; i++) {
                    GameObjects[i].ScaleSprite();
                }

                // Last thing to do each time. Render the frame and display it!
                Frame nextFrame = new Frame();
                nextFrame.Buffer = RenderGameObjects();  // Make buffer, remove dead objects
                _graphics.DrawFrame(nextFrame);

                // Run lateUpdate on each gameObject
                for (int i = 0; i < GameObjects.Count; i++) {
                    GameObjects[i].LateUpdate(nextFrame);
                }
            }
        }

        private char[,] RenderGameObjects() {
            // This will make a buffer of all gameObjects on Screen
            char[,] buffer = new char[ConsoleHeight, ConsoleWidth];

            for (int i = 0; i < GameObjects.Count; i++) {
                GameObject gameObject = GameObjects[i];
                char[,] sprite = gameObject.Sprite;
                
                if (gameObject.isDead) {
                    GameObjects.RemoveAt(i);
                    continue;
                }
                if (gameObject.Sprite == null) { continue; }

                for (int y = 0; y < sprite.GetLength(0); y++) {
                    for (int x = 0; x < sprite.GetLength(1); x++) {
                        try {
                            buffer[gameObject.yPos + y, gameObject.xPos + x] = sprite[y, x];
                        } catch { }
                    }
                }
            }

            return buffer;
        }

    }

    static class Input {

        static List<ConsoleKey> KeyList = new List<ConsoleKey>();

        static public void Listner() {
            while (true) {
                if (Console.KeyAvailable) {
                    KeyList.Add(Console.ReadKey().Key);
                }
            }
        }
        
        static public bool KeyPressed(ConsoleKey keyCode) {
            for (int i = 0; i < KeyList.Count; i++) {
                if (KeyList[i] == keyCode) {
                    KeyList.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

    }

    abstract class GameObject {
        public Game Parent;
        virtual public bool isDead { get; set; }
        virtual public String Tag { get; set; }
        virtual public int Scale { get; set; }
        virtual public int xPos { get; set; }
        virtual public int yPos { get; set; }
        virtual public Char[,] Sprite { get; set; }

        private Char[,] RenderedSprite;

        virtual public void Start() {
            // This method is called once before the game actually starts
        }

        virtual public void Update() {
            // This method is called right before the frame is rendered
        }

        virtual public void LateUpdate(Frame thisFrame) {
            // This method is called after the frame is rendered
        }

        public bool CollidingWith(GameObject other) {
            // This method checks if 'this' collides with another GameObject
            // TODO Make this check if the position contains '\0', if so we are not colliding. This will make hitboxes more natural
            int x1 = other.xPos;
            int x2 = other.xPos + other.Sprite.GetLength(1);
            int y1 = other.yPos;
            int y2 = other.yPos + other.Sprite.GetLength(0);
            return (x1 <= xPos && xPos < x2 && y1 <= yPos && yPos <= y2);
        }

        public void ScaleSprite() {
            // This is called after the Update method, and scales the sprite if it has changed
            if (RenderedSprite == Sprite) { return; }

            RenderedSprite = new Char[Sprite.GetLength(0) * Scale, Sprite.GetLength(1) * Scale * 2];

            for (int y = 0; y < Sprite.GetLength(0); y++) {
                for (int x = 0; x < Sprite.GetLength(1); x++) {

                    for (int y1 = 0; y1 < Scale; y1++) {
                        for (int x1 = 0; x1 < (Scale * 2); x1++) { // The *2 is because a char is twice as high as wide

                            try {
                                int xOffset = x1 + ((Scale + Scale) * x) - x;
                                int yOffset = y1 + (Scale * y) - y;
                                int xPos = xOffset + x;
                                int yPos = yOffset + y;
                                char cChar = Sprite[y, x];
                                RenderedSprite[yPos, xPos] = cChar;
                            }  catch { }

                        }
                    }


                }
            }
            Sprite = RenderedSprite;
        }

    }

}
