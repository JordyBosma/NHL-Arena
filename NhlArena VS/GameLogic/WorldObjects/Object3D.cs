using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldObjects
{
    /// <summary>
    /// Deze class is de abstacte parent class van alle objecten die vanaf de server geladen moeten worden, deze class regelt positie, rotatie en movement van objecten.
    /// </summary>
    public class Object3D
    {
        private double _x = 0;
        private double _y = 0;
        private double _z = 0;
        private double _rX = 0;
        private double _rY = 0;
        private double _rZ = 0;
        private string _type = "";

        public Guid guid { get; }
        public string type { get { return _type; } }
        public double x { get { return _x; } }
        public double y { get { return _y; } }
        public double z { get { return _z; } }
        public double rotationX { get { return _rX; } }
        public double rotationY { get { return _rY; } }
        public double rotationZ { get { return _rZ; } }

        public bool needsUpdate = true;

        public Object3D(double x, double y, double z, double rotationX, double rotationY, double rotationZ, string type)
        {
            this._type = type;
            this.guid = Guid.NewGuid();

            this._x = x;
            this._y = y;
            this._z = z;

            this._rX = rotationX;
            this._rY = rotationY;
            this._rZ = rotationZ;
        }

        public virtual void Move(double x, double y, double z)
        {
            this._x = x;
            this._y = y;
            this._z = z;
        }

        public virtual void Rotate(double rotationX, double rotationY, double rotationZ)
        {
            this._rX = rotationX;
            this._rY = rotationY;
            this._rZ = rotationZ;
        }
    }
}

