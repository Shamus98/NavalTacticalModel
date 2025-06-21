namespace NavalTacticalModel
{
    public class Vector2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double Length => Math.Sqrt(X * X + Y * Y);

        public Vector2D Normalized()
        {
            double length = Length;
            return length > 0 ? new Vector2D(X / length, Y / length) : new Vector2D(0, 0);
        }

        public static Vector2D operator *(Vector2D v, double scalar)
        {
            return new Vector2D(v.X * scalar, v.Y * scalar);
        }
    }
}