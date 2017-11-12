using System;
using GameEngine;

namespace SpaceInvaders {

    class GameLogic : GameObject {

        public override void Start() {
            for (int i = 0; i < 12; i++) {
                GameObject monster = new Monster();
                monster.Parent = Parent;
                monster.xPos = i * 35 + 90;
                monster.yPos = 20;
                Parent.GameObjects.Add(monster);
            }
        }

        public override void Update() {

            if (Input.KeyPressed(ConsoleKey.Escape)) {
                Environment.Exit(0);
            }

        }
    }

    class PlayerBullet : GameObject {

        public int MoveTimer = 10;

        public override void Start() {
            Tag = "Projectile";
            Scale = 1;
            Sprite = new char[1, 1] { { '\u2588' } };
        }

        public override void Update() {

            MoveTimer--;
            if (MoveTimer <= 0) {
                yPos = yPos - 1;
                MoveTimer = 10;
            }
            

            if (yPos <= 0 || yPos >= Parent.ConsoleHeight-1) {
                isDead = true;
            }

        }

        public override void LateUpdate(Frame thisFrame) {
            for (int i = 0; i < Parent.GameObjects.Count; i++) {
                GameObject other = Parent.GameObjects[i];

                if (other.Tag == "Enemy") {
                    if (CollidingWith(other)) {
                        other.isDead = true;
                        isDead = true;
                    }
                }

                if (other.Tag == "Obstacle") {
                    if (CollidingWith(other)) {
                        Obstacle obstacle = (Obstacle)other;
                        obstacle.HP = obstacle.HP - 1;
                        isDead = true;
                    }
                }
            }
        }

    }

    class MonsterBullet : GameObject {

        public int MoveTimer = 10;

        public override void Start() {
            Tag = "Projectile";
            Scale = 1;
            Sprite = new char[1, 1] { { '\u2588' } };
        }

        public override void Update() {

            MoveTimer--;
            if (MoveTimer <= 0) {
                yPos = yPos + 1;
                MoveTimer = 10;
            }


            if (yPos <= 0 || yPos >= Parent.ConsoleHeight - 1) {
                isDead = true;
            }

        }

        public override void LateUpdate(Frame thisFrame) {
            for (int i = 0; i < Parent.GameObjects.Count; i++) {
                GameObject other = Parent.GameObjects[i];

                if (other.Tag == "Enemy") {
                    if (CollidingWith(other)) {
                        isDead = true;
                    }
                }

                if (other.Tag == "Player") {
                    if (CollidingWith(other)) {
                        other.isDead = true;
                        isDead = false;
                    }
                }

                if (other.Tag == "Obstacle") {
                    if (CollidingWith(other)) {
                        Obstacle obstacle = (Obstacle)other;
                        obstacle.HP = obstacle.HP - 1;
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
                GameObject tmpBullet = new PlayerBullet();
                tmpBullet.Parent = Parent;
                tmpBullet.xPos = xPos + 8;
                tmpBullet.yPos = yPos - 1;
                tmpBullet.Start();
                Parent.GameObjects.Add(tmpBullet);
            }

        }

    }

    class Obstacle : GameObject {
        

        public int HP = 4;
        private int lastHP = 4;

        public override void Start() {
            Tag = "Obstacle";
            Scale = 4;

            Sprite = new char[1, 1] { { '\u2588' } };
        }

        public override void Update() {
                
            if (lastHP != HP) {
                lastHP = HP;

                if (HP == 4) {
                    Sprite = new char[1, 1] { { '\u2588' } };
                }
                if (HP == 3) {
                    Sprite = new char[1, 1] { { '\u2593' } };
                }
                if (HP == 2) {
                    Sprite = new char[1, 1] { { '\u2592' } };
                }
                if (HP == 1) {
                    Sprite = new char[1, 1] { { '\u2591' } };
                }
                if (HP <= 0) {
                    isDead = true;
                }

            }

        }

    }

    class Monster : GameObject {

        private int _shootTimer = 400000;
        private int _moveTimer = 400;

        private bool _movingRight = true;
        private int _UBound = 20;
        private int _currStep = 10;

        public override void Start() {
            Tag = "Enemy";
            Scale = 1;

            _shootTimer = Parent.Rand.Next(1500, 3500);

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
            
            if (_moveTimer <= 0) {
                _moveTimer = 400;
                if (_currStep >= _UBound) {
                    _currStep = 0;
                    _movingRight = !_movingRight;

                    if (_movingRight) {
                        yPos = yPos + 8;
                    }
                }

                if (_movingRight) {
                    xPos = xPos + 5;
                }
                else {
                    xPos = xPos - 5;
                }

                _currStep++;
            }
            _moveTimer--;

        }

        public override void LateUpdate(Frame thisFrame) {
            if (_shootTimer <= 0) {
                _shootTimer = Parent.Rand.Next(1500, 3500); ;
                MonsterBullet bullet = new MonsterBullet();
                bullet.Parent = Parent;
                bullet.xPos = xPos + 10;
                bullet.yPos = yPos + Sprite.GetLength(0) + 1;
                bullet.Start();
                Parent.GameObjects.Add(bullet);
            }
            _shootTimer--;
        }

    }

}
