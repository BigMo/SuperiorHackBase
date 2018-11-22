using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.Maths
{
    public struct Rectangle
    {
        public static Rectangle Zero { get; private set; } = new Rectangle(0, 0, 0, 0);

        #region VARIABLES
        public float X, Y, Width, Height;
        #endregion

        #region PROPERTIES
        public Vector2 Position
        {
            get { return new Vector2(X, Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        public Vector2 Size
        {
            get { return new Vector2(Width, Height); }
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }
        public Vector2 Center { get { return Position + Size * 0.5f; } }

        public Vector2 TopLeft { get { return new Vector2(Top,Left); } }
        public Vector2 TopRight { get { return new Vector2(Top, Right); } }
        public Vector2 BottomLeft { get { return new Vector2(Bottom, Left); } }
        public Vector2 BottomRight { get { return new Vector2(Bottom, Right); } }

        public bool IsEmpty { get { return X == 0f && Y == 0f && Width == 0f && Height == 0f; } }
        public bool IsNegative { get { return Width < 0f || Height < 0f; } }

        public float Left { get { return X; } }
        public float Right { get { return X + Width; } }
        public float Top { get { return Y; } }
        public float Bottom { get { return Y + Height; } }
        #endregion

        #region CONSTRUCTORS
        public Rectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        #endregion

        #region OPERATORS
        public static bool operator ==(Rectangle r1, Rectangle r2)
        {
            return r1.X == r2.X && r1.Y == r2.Y && r1.Width == r2.Width && r1.Height == r2.Height;
        }
        public static bool operator !=(Rectangle r1, Rectangle r2)
        {
            return r1.X != r2.X || r1.Y != r2.Y || r1.Width != r2.Width || r1.Height != r2.Height;
        }
        #endregion

        #region METHODS
        public void Inflate(float delta)
        {
            Inflate(delta, delta);
        }
        public void Inflate(float width, float height)
        {
            X -= width;
            Y -= height;
            Width += width * 2f;
            Height += height * 2f;
        }

        public bool Contains(Vector2 point)
        {
            return Left >= point.X && Right <= point.X && Top >= point.Y && Bottom <= point.Y;
        }

        public Rectangle Intersect(Rectangle other)
        {
            return Intersect(this, other);
        }

        public static Rectangle Intersect(Rectangle a, Rectangle b)
        {
            float left = Math.Max(a.X, b.X);
            float right = Math.Min(a.X + a.Width, b.X + b.Width);
            float top = Math.Max(a.Y, b.Y);
            float bottom = Math.Min(a.Y + a.Height, b.Y + b.Height);
            if (right >= left && bottom >= top)
                return new Rectangle(left, top, right - left, bottom - top);
            else
                return Zero;
        }
        #endregion
    }
}
