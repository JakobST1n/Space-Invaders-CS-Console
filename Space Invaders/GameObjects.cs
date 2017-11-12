using System;
using GameEngine;

namespace SpaceInvaders {

    class GameLogic : GameObject {

        public override void Update() {

            if (Input.KeyPressed(ConsoleKey.Escape)) {
                Environment.Exit(0);
            }

        }
    }

    class Bullet : GameObject {

        int moveTimer = 10;

        public override void Start() {
            Tag = "Projectile";
            Scale = 1;
            Sprite = new char[1, 1] { { '\u2588' } };
        }

        public override void Update() {

            moveTimer--;
            if (moveTimer <= 0) {
                yPos = yPos - 1;
                moveTimer = 10;
            }
            

            if (yPos <= 0) {
                isDead = true;
            }

        }

        public override void LateUpdate(Frame thisFrame) {
            for (int i = 0; i < Parent.GameObjects.Count; i++) {
                GameObject other = Parent.GameObjects[i];

                if (other.Tag == "Enemy") {
                    int x1 = other.xPos;
                    int x2 = other.xPos + other.Sprite.GetLength(1);
                    int y1 = other.yPos;
                    int y2 = other.yPos + other.Sprite.GetLength(0);
                    if (x1 <= xPos && xPos <= x2 && y1 <= yPos && yPos <= y2) {
                        other.isDead = true;
                        isDead = true;
                    }
                }

                if (other.Tag == "Obstacle") {
                    int x1 = other.xPos;
                    int x2 = other.xPos + other.Sprite.GetLength(1);
                    int y1 = other.yPos;
                    int y2 = other.yPos + other.Sprite.GetLength(0);
                    if (x1 <= xPos && xPos <= x2 && y1 <= yPos && yPos <= y2) {
                        isDead = true;
                    }
                }
            }
        }

    }

    class Player : GameObject {

        public override void Start() {
            Tag = "Player";
            Sprite = new char[4, 9] {
                {'\0', '\0', '\0', '\0', '\u2588', '\0', '\0', '\0', '\0'},
                {'\0', '\0', '\0', '\0', '\u2588', '\0', '\0', '\0', '\0'},
                {'\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588'},
                {'\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588'}
            };
        }

        public override void Update() {
            
            if (Input.KeyPressed(ConsoleKey.RightArrow)) {
                xPos = xPos + 2;
                
            }

            if (Input.KeyPressed(ConsoleKey.LeftArrow)) {
                xPos = xPos - 2;
            }

            if (Input.KeyPressed(ConsoleKey.Spacebar)) {
                GameObject tmpBullet = new Bullet();
                tmpBullet.Parent = Parent;
                tmpBullet.xPos = xPos + 8;
                tmpBullet.yPos = yPos - 1;
                tmpBullet.Start();
                Parent.GameObjects.Add(tmpBullet);
            }

        }

    }

    class Obstacle : GameObject {

        public override void Start() {
            Tag = "Obstacle";
            Scale = 4;

            Sprite = new char[2, 5] {
                {'\u2588', '\u2588', '\u2588', '\u2588', '\u2588'},
                {'\u2588', '\0', '\0', '\0', '\u2588'}
            };
        }

    }

    class Monster : GameObject {

        public override void Start() {
            Tag = "Enemy";
            xPos = 10;
            yPos = 10;
            Scale = 1;

            Sprite = new char[8, 11] {
                {'\0', '\0', '\u2588', '\0', '\0', '\0', '\0', '\0', '\u2588', '\0', '\0'},
                {'\0', '\0', '\0', '\u2588', '\0', '\0', '\0', '\u2588', '\0', '\0', '\0'},
                {'\0', '\0', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\0', '\0'},
                {'\0', '\u2588', '\u2588', '\0', '\u2588', '\u2588', '\u2588', '\0', '\u2588', '\u2588', '\0'},
                {'\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588'},
                {'\u2588', '\0', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\u2588', '\0', '\u2588'},
                {'\u2588', '\0', '\u2588', '\0', '\0', '\0', '\0', '\0', '\u2588', '\0', '\u2588'},
                {'\0', '\0', '\0', '\u2588', '\u2588', '\0', '\u2588', '\u2588', '\0', '\0', '\0'}
            };

        }

        public override void Update() {
        }

    }

}
