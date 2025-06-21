using System;

namespace NavalTacticalModel
{
    public class AttackerBEC : BEC
    {
        public EngineType CurrentEngine { get; private set; }
        public double GasolineSpeed { get; private set; }
        public double ElectricSpeed { get; private set; }
        public bool IsDestroyed { get; private set; }

        private Point2D _targetPosition;
        private Random _random = new Random();
        private TimeSpan _lastEngineChangeTime;

        public AttackerBEC(Point2D startPosition, Point2D targetPosition, ConflictSide side,
                         string name, double maxAcceleration, double gasolineSpeed, double electricSpeed)
            : base(startPosition, side, name)
        {
            _targetPosition = targetPosition;
            MaxTransverseAcceleration = maxAcceleration;
            GasolineSpeed = gasolineSpeed;
            ElectricSpeed = electricSpeed;
            SetEngine(EngineType.Gasoline);
            _lastEngineChangeTime = TimeSpan.Zero;
            IsDestroyed = false;
        }

        public void SetEngine(EngineType engineType)
        {
            if (IsDestroyed) return;

            CurrentEngine = engineType;
            double speed = 0;

            switch (engineType)
            {
                case EngineType.Gasoline:
                    speed = GasolineSpeed;
                    break;
                case EngineType.Electric:
                    speed = ElectricSpeed;
                    break;
                case EngineType.Stopped:
                    speed = 0;
                    break;
            }

            Vector2D direction = new Vector2D(_targetPosition.X - Position.X, _targetPosition.Y - Position.Y).Normalized();
            Velocity = new Point2D(direction.X * speed, direction.Y * speed);
            _lastEngineChangeTime = lastUpdated;
        }

        public void MarkAsDestroyed()
        {
            IsDestroyed = true;
            Velocity = new Point2D(0, 0);
        }

        public override (TimeSpan, FastAbstractEvent) getNearestEvent()
        {
            if (IsDestroyed) return (TimeSpan.MaxValue, null);

            // Проверка достижения цели
            if (Position.DistanceTo(_targetPosition) < 10) // 10 метров - близко достаточно
            {
                return (lastUpdated.Add(TimeSpan.FromSeconds(1)),
                       new ModelEvent { EventType = ModelEventType.Finish, AdditionalInfo = "Target reached" });
            }

            // Случайное изменение двигателя (не чаще чем раз в 30 секунд)
            if ((lastUpdated - _lastEngineChangeTime).TotalSeconds > 30 &&
                _random.NextDouble() < 0.01) // 1% chance to change engine
            {
                EngineType newEngine = (EngineType)_random.Next(0, 3);
                return (lastUpdated.Add(TimeSpan.FromSeconds(_random.Next(5, 15))),
                       new ModelEvent
                       {
                           EventType = ModelEventType.ChangeEngine,
                           AdditionalInfo = newEngine.ToString()
                       });
            }

            return (TimeSpan.MaxValue, null);
        }
    }
}