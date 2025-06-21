using System;
using System.Collections.Generic;

namespace NavalTacticalModel
{
    public class RGB : FastAbstractObject
    {
        public Point2D Position { get; private set; }
        public double GasolineDetectionRadius { get; private set; }
        public double ElectricDetectionRadius { get; private set; }
        private HashSet<string> _detectedObjects = new HashSet<string>();

        public RGB(Point2D position, double gasolineRadius, double electricRadius)
        {
            Position = position;
            GasolineDetectionRadius = gasolineRadius;
            ElectricDetectionRadius = electricRadius;
            lastUpdated = TimeSpan.Zero;
        }

        public bool DetectObject(BEC bec)
        {
            if (bec is AttackerBEC attacker)
            {
                double detectionRadius = attacker.CurrentEngine == EngineType.Electric
                    ? ElectricDetectionRadius
                    : GasolineDetectionRadius;

                bool isDetected = Position.DistanceTo(attacker.Position) <= detectionRadius;
                bool wasDetected = _detectedObjects.Contains(attacker.uid);

                if (isDetected && !wasDetected)
                {
                    _detectedObjects.Add(attacker.uid);
                    return true;
                }
                else if (!isDetected && wasDetected)
                {
                    _detectedObjects.Remove(attacker.uid);
                }
            }
            return false;
        }

        public override (TimeSpan, FastAbstractEvent) getNearestEvent()
        {
            // В реальной реализации здесь должна быть проверка объектов в зоне обнаружения
            return (TimeSpan.MaxValue, null);
        }

        public override void Update(TimeSpan timeSpan)
        {
            lastUpdated = timeSpan;
        }

        public override string ToString()
        {
            return $"RGB at {Position} (G: {GasolineDetectionRadius}m, E: {ElectricDetectionRadius}m)";
        }
    }
}