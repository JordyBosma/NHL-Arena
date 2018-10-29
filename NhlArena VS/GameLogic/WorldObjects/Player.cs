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
                    currentDamage = armourDamage / 2;
                }
                else
                {
                    _armour -= armourDamage;
                    return false;
                }
            }

            if (currentDamage > _health)
            {
                _health = 100;
                _deaths++;
                return true;
            }

            _health -= currentDamage;
            return false;
        }

        public void addKill()
        {
            _kills++;
        }

        public override void Move(double x, double y, double z)
        {
            if (this.x == x && this.y == y && this.z == z) isMoving = false;
            else isMoving = true;


            base.Move(x, y, z);
        }

        public override void Rotate(double rotationX, double rotationY, double rotationZ)
        {
            base.Rotate(rotationX, rotationY, rotationZ);
        }

    }
}
