using Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldObjects
{
    public class Player
    {
        private Client playerClient;
        public Guid playerGuid { get; }
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

        private double _posX;
        private double _posY;
        private double _posZ;
        private double _rotationY;

        public double posX { get { return _posX; } }
        public double posY { get { return _posY; } }
        public double posZ { get { return _posZ; } }
        public double rotationY { get { return _rotationY; } }

        public Player(Client playerClient)
        {
            this.playerClient = playerClient;
            this.playerGuid = new Guid();
            this.username = playerClient.username;
            this._health = 100;
            this._armour = 0;
        }
    }
}
