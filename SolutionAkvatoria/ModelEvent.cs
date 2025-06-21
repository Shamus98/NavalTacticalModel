using System;

namespace NavalTacticalModel
{
    public class ModelEvent : FastAbstractEvent
    {
        public ModelEventType EventType { get; set; }
        public string AdditionalInfo { get; set; }
        public DateTime EventTime { get; set; }

        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            EventTime = DateTime.Now;
            // Базовая реализация, может быть переопределена в наследниках
            if (wrapper is TacticalScene scene)
            {
                scene.WriteDebug($"Event executed: {EventType} at {timeSpan}. Info: {AdditionalInfo}");
            }
        }

        public override string ToString()
        {
            return $"{EventType} at {EventTime:HH:mm:ss.fff}";
        }
    }
}