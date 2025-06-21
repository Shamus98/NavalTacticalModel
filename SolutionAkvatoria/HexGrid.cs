using System;
using System.Collections.Generic;

namespace NavalTacticalModel
{
    public class HexGrid
    {
        public double HexSize { get; private set; }
        public double HexWidth { get; private set; }
        public double HexHeight { get; private set; }

        public HexGrid(double hexSize)
        {
            HexSize = hexSize;
            HexWidth = 2 * hexSize;
            HexHeight = Math.Sqrt(3) * hexSize;
        }

        public Point2D GetHexCenter(int x, int y)
        {
            double centerX = x * HexWidth * 0.75;
            double centerY = y * HexHeight + (x % 2) * HexHeight / 2;
            return new Point2D(centerX, centerY);
        }

        public (int, int) GetHexIndex(Point2D point)
        {
            double q = (2.0 / 3 * point.X) / HexSize;
            double r = (-1.0 / 3 * point.X + Math.Sqrt(3) / 3 * point.Y) / HexSize;

            int x = (int)Math.Round(q);
            int y = (int)Math.Round(r);

            return (x, y);
        }

        public List<Point2D> GetHexCorners(Point2D center)
        {
            var corners = new List<Point2D>();
            for (int i = 0; i < 6; i++)
            {
                double angle = 2 * Math.PI / 6 * i;
                double x = center.X + HexSize * Math.Cos(angle);
                double y = center.Y + HexSize * Math.Sin(angle);
                corners.Add(new Point2D(x, y));
            }
            return corners;
        }
    }
}