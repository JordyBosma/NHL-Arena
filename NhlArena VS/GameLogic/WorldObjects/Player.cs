using Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldObjects
{
    public class Player : Object3D
    {
        private Client playerClient;
        public string username { get; }
        private bool _isMoving;
        public bool isMoving;

        private int _health;
        private int _armour;
        public int health { get { return _health; } }
        public int armour { get { return _armour; } }

        private int _kills;
        private int _deaths;
        public int kills { get { return _kills; } }
        public int deaths { get { return _deaths; } }

        public Player(Client playerClient, double x, double y, double z, double rotationX, double rotationY, double rotationZ) : base(x, y, z, rotationX, rotationY, rotationZ, "Player")
        {
            this.playerClient = playerClient;
            if (playerClient == null)
            {
                this.username = "foobar";
            }
            else
            {
                this.username = playerClient.username;
            }
            this._health = 100;
            this._armour = 0;
            playerClient.SetPlayer(this);
        }

        /// <summary>
        /// handle damage done to the player. an chekc if the player dies
        /// </summary>
        /// <param name="damage"></param>
        /// <returns>true is the player dies</returns>
        public bool DoDamage(int damage)
        {
            int armourDamageMultiplier = 2;
            int currentDamage = damage;

            if (_armour > 0)
            {
                int armourDamage = currentDamage * armourDamageMultiplier;

                if (armourDamage > _armour)
                {
                    armourDamage -= _armour;
                    _armour = 0;
                    currentDamage = armourDamage / 2;
                }
                else
                {
                    _armour -= armourDamage;
                    return false;
                }
            }

            if (currentDamage >= _health)
            {
                _health = 100;
                _deaths++;
                return true;
            }

            _health -= currentDamage;
            return false;
        }

        /// <summary>
        /// add a kill to the players score
        /// </summary>
        public void addKill()
        {
            _kills++;
        }

        public void addDeath()
        {
            _deaths++;
        }

        public void addHealth(int healthValue)
        {
            if(health < 100)
            {
                _health += healthValue;
                if (health > 100)
                {
                    _health = 100;
                }
            }
        }

        public void addArmour(int armourValue)
        {
            if (armour < 100)
            {
                _armour += armourValue;
                if (armour > 100)
                {
                    _armour = 100;
                }
            }
        }

        /// <summary>
        /// move the player to a ne set of coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public override void Move(double x, double y, double z)
        {
            if (this.x == x && this.y == y && this.z == z) isMoving = false;
            else isMoving = true;


            base.Move(x, y, z);
        }

        /// <summary>
        /// rotate the player object along its different axis.
        /// </summary>
        /// <param name="rotationX"></param>
        /// <param name="rotationY"></param>
        /// <param name="rotationZ"></param>
        public override void Rotate(double rotationX, double rotationY, double rotationZ)
        {
            base.Rotate(rotationX, rotationY, rotationZ);
        }

        /// <summary>
        /// returns the client connected to the player
        /// </summary>
        /// <returns></returns>
        public Client GetClient()
        {
            return playerClient;
        }
    }
}
