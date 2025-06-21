namespace NavalTacticalModel
{
    // Класс, представляющий событие в модели
    public class SimulationEvent
    {
        public DateTime Time { get; }
        public string EventType { get; }
        public string ObjectId { get; }
        public Vector2D Coordinates { get; }

        public SimulationEvent(DateTime time, string eventType, string objectId, Vector2D coordinates)
        {
            Time = time;
            EventType = eventType;
            ObjectId = objectId;
            Coordinates = coordinates;
        }

        public override string ToString()
        {
            return $"{Time:HH:mm:ss} {EventType} {ObjectId} {Coordinates}";
        }
    }
}