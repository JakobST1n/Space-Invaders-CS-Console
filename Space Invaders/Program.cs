using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;

namespace SpaceInvaders {

    class Program {
        static void Main(string[] args) {
            Game SpaceInvaders = new SpaceInvaders();
            SpaceInvaders.Setup();
            SpaceInvaders.Start();
        }
    }

   class SpaceInvaders : Game {

        public override void Setup() {
            // This sub is meant for setting the properties of the GameEngine. DO NOT MAKE NEW ONES HERE
            // IMPORTANT! You might have to change the console font size, depending on how many rows and columns you want to have!
            ConsoleWidth = 600;  // Bigger than 15
            ConsoleHeight = 120; // Bigger than 15
        }

        public override void Start() {
            // Initialize all gameobjects here
            GameObject gameLogic = new GameLogic();
            gameLogic.Parent = this;
            GameObjects.Add(gameLogic);

            // Initialize the player
            GameObject player = new Player();
            player.Parent = this;
            player.xPos = ConsoleWidth / 2 - 10;
            player.yPos = 115;
            player.Scale = 1;
            GameObjects.Add(player);

            // Init Obstacles
            for (int i = 0; i < 6; i++) {
                GameObject obstacle = new Obstacle();
                player.Parent = this;
                obstacle.xPos = 50 + (i * 90);
                obstacle.yPos = 90;
                GameObjects.Add(obstacle);
            }

            base.Start(); // Do start from inherited class, Required for the engine to actually start
        }

    }

}
