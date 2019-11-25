using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.Maths
{
    public struct Rectangle
    {
        public Vector2 Position;
        public Vector2 Size;
        public float X { get => Position.X; set => Position.X = value; }
        public float Left {get=> Position.X; set => Position.X = value; }
        public float Y {get=> Position.Y; set => Position.Y = value; }
        public float Top {get => Position.Y; set => Position.Y = value; }
        public float Right => Position.X + Size.X;
        public float Bottom => Position.Y + Size.Y;
        public float Width  { get => Size.X; set => Size.X = value; }
        public float Height { get => Size.Y; set => Size.Y = value; }
        public float Area => Width * Height;
        public Vector2 TopLeft => new Vector2(Left, Top);
        public Vector2 TopRight => new Vector2(Right, Top);
        public Vector2 BottomLeft => new Vector2(Left, Bottom);
        public Vector2 BottomRight => new Vector2(Right, Bottom);
        public Vector2 Center
        {
            get => Position + Size / 2f;
            set => Position = value - Size / 2f;
        }
        public Vector2[] Corners => new Vector2[] { TopLeft, TopRight, BottomRight, BottomLeft };

        public static Rectangle Zero { get; private set; } = new Rectangle(0f, 0f, 0f, 0f);
        public static Rectangle Unit { get; private set; } = new Rectangle(1f, 1f, 1f, 1f);

        public Rectangle(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }
        public Rectangle(float x, float y, float w, float h) : this(new Vector2(x, y), new Vector2(w, h)) { }

        public Rectangle Expand(Vector2 by)
        {
            return new Rectangle(Position, Size + by);
        }
        public Rectangle Move(Vector2 by)
        {
            return new Rectangle(Position + by, Size);
        }
        public Rectangle Scale(float by)
        {
            return new Rectangle(Position, Size * by);
        } 
        public bool Intersects(Vector2 other)
        {
            return
                X <= other.X && Right >= other.X &&
                Y <= other.Y && Bottom >= other.Y;
        }
        public bool Intersects(Rectangle other)
        {
            return
                other.X < X + Width && X < other.X + other.Width &&
                other.Y < Y + Height && Y < other.Y + other.Height;
        }
        public Rectangle Intersect(Rectangle other)
        {
            if (!Intersects(other)) return Rectangle.Zero;

            var x = Math.Max(X, other.X);
            var width = Math.Min(X + Width, other.X + other.Width);
            var y = Math.Max(Y, other.Y);
            var height = Math.Min(Y + Height, other.Y + other.Height);
            return new Rectangle(x, y, width - x, height - y);
        }
        public override string ToString()
        {
            return string.Format("[Rect X={0}, Y={1}, W={2}, H={3}]", X, Y, Width, Height);
        }
    }
}
