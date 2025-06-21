using System.Collections.Generic;

namespace NavalTacticalModel
{
    // Класс, представляющий РГБ (радиогидроакустический буй)
    public class Buoy
    {
        public string Id { get; }
        public Vector2D Position { get; }
        public double GasolineDetectionRange { get; }
        public double ElectricDetectionRange { get; }
        public List<string> DetectedUSVs { get; }

        public Buoy(string id, Vector2D position, double gasolineDetectionRange, double electricDetectionRange)
        {
            Id = id;
            Position = position;
            GasolineDetectionRange = gasolineDetectionRange;
            ElectricDetectionRange = electricDetectionRange;
            DetectedUSVs = new List<string>();
        }

        public bool DetectUSV(USV usv)
        {
            if (usv.IsDestroyed || usv.HasReachedTarget) return false;

            double distance = Position.DistanceTo(usv.Position);
            double detectionRange = usv.CurrentEngine == EngineType.Gasoline
                ? GasolineDetectionRange
                : ElectricDetectionRange;

            if (distance <= detectionRange)
            {
                if (!DetectedUSVs.Contains(usv.Id))
                {
                    DetectedUSVs.Add(usv.Id);
                    return true; // Новое обнаружение
                }
                return false; // Уже обнаружен
            }
            else
            {
                if (DetectedUSVs.Contains(usv.Id))
                {
                    DetectedUSVs.Remove(usv.Id);
                }
                return false;
            }
        }
    }
}