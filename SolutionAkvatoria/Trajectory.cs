using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavalTacticalModel
{
    public class Trajectory
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Speed { get; }
        public double TargetX { get; set; }
        public double TargetY { get; set; }

        public Trajectory(double x, double y, double speed, double targetX, double targetY)
        {
            X = x;
            Y = y;
            Speed = speed;
            TargetX = targetX;
            TargetY = targetY;
        }

        public void Update(TimeSpan timeDelta)
        {
            double deltaSeconds = timeDelta.TotalSeconds;
            double dx = TargetX - X;
            double dy = TargetY - Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance <= Speed * deltaSeconds)
            {
                X = Math.Clamp(TargetX, 0, SimulationConstants.SceneWidth);
                Y = Math.Clamp(TargetY, 0, SimulationConstants.SceneHeight);
            }
            else
            {
                X = Math.Clamp(X + dx / distance * Speed * deltaSeconds, 0, SimulationConstants.SceneWidth);
                Y = Math.Clamp(Y + dy / distance * Speed * deltaSeconds, 0, SimulationConstants.SceneHeight);
            }
        }

        public double DistanceTo(double x, double y) => Math.Sqrt(Math.Pow(X - x, 2) + Math.Pow(Y - y, 2));
    }
}
