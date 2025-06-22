using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavalTacticalModel
{
    public class RGB : FastAbstractObject
    {
        public double X { get; }
        public double Y { get; }
        public double DetectionRadius { get; }

        public RGB(double x, double y, bool isElectric)
        {
            X = x;
            Y = y;
            DetectionRadius = isElectric
                ? SimulationConstants.DetectionRadiusElectric
                : SimulationConstants.DetectionRadiusGasoline;
        }

        public override (TimeSpan, FastAbstractEvent) getNearestEvent() =>
            (TimeSpan.MaxValue, null);

        public override void Update(TimeSpan timeSpan) { }

        public bool Detect(double x, double y) =>
            Math.Sqrt(Math.Pow(X - x, 2) + Math.Pow(Y - y, 2)) <= DetectionRadius;
    }
}
