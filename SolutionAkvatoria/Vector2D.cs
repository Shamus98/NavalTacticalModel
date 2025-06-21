using System;

namespace NavalTacticalModel
{
    // Класс для представления точки в 2D пространстве
    public class Vector2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double DistanceTo(Vector2D other)
        {
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
        }

        public override string ToString()
        {
            return $"({X:F2}, {Y:F2})";
        }
    }
}
