using Console.Interop.Internal;
using System;

namespace Console.Interop {
    public class Coordinate {
        internal COORD coord;

        public Coordinate() {
            coord = new COORD();
        }

        public Coordinate(short x, short y) : this() {
            this.X = x;
            this.Y = y;
        }

        internal Coordinate(COORD coord) {
            this.coord = coord;
        }

        internal COORD AsCOORD() {
            return coord;
        }

        public short X {
            get {
                return coord.X;
            }
            set {
                coord.X = value;
            }
        }

        public short Y {
            get {
                return coord.Y;
            }
            set {
                coord.Y = value;
            }
        }

        public override string ToString() {
            return String.Format("@{{ X = {0}; Y = {1} }}", X, Y);
        }
    }
}
