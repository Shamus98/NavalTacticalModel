namespace NavalTacticalModel
{
    // Класс, представляющий БЭК (безэкипажный катер)
    public class USV
    {
        public string Id { get; }
        public Vector2D Position { get; private set; }
        public Vector2D Velocity { get; private set; }
        public EngineType CurrentEngine { get; private set; }
        public double MaxAcceleration { get; }
        public double GasolineSpeed { get; }
        public double ElectricSpeed { get; }
        public bool IsDestroyed { get; private set; }
        public bool HasReachedTarget { get; private set; }

        public USV(string id, Vector2D startPosition, double maxAcceleration,
                 double gasolineSpeed, double electricSpeed)
        {
            Id = id;
            Position = startPosition;
            Velocity = new Vector2D(0, 0);
            CurrentEngine = EngineType.Stopped;
            MaxAcceleration = maxAcceleration;
            GasolineSpeed = gasolineSpeed;
            ElectricSpeed = electricSpeed;
            IsDestroyed = false;
            HasReachedTarget = false;
        }

        public void SetEngine(EngineType engineType)
        {
            CurrentEngine = engineType;
        }

        public void Move(double timeStep, Vector2D targetZone)
        {
            if (IsDestroyed || HasReachedTarget) return;

            // Определяем желаемую скорость в зависимости от типа двигателя
            double desiredSpeed = 0;
            switch (CurrentEngine)
            {
                case EngineType.Gasoline:
                    desiredSpeed = GasolineSpeed;
                    break;
                case EngineType.Electric:
                    desiredSpeed = ElectricSpeed;
                    break;
                case EngineType.Stopped:
                    desiredSpeed = 0;
                    break;
            }

            // Рассчитываем направление к цели
            Vector2D direction = new Vector2D(
                targetZone.X - Position.X,
                targetZone.Y - Position.Y);

            double distanceToTarget = Position.DistanceTo(targetZone);
            if (distanceToTarget > 0)
            {
                direction.X /= distanceToTarget;
                direction.Y /= distanceToTarget;
            }

            // Ускорение к целевой скорости
            Vector2D desiredVelocity = new Vector2D(
                direction.X * desiredSpeed,
                direction.Y * desiredSpeed);

            Vector2D acceleration = new Vector2D(
                (desiredVelocity.X - Velocity.X) / timeStep,
                (desiredVelocity.Y - Velocity.Y) / timeStep);

            // Ограничиваем ускорение максимальным значением
            double accelMagnitude = Math.Sqrt(acceleration.X * acceleration.X + acceleration.Y * acceleration.Y);
            if (accelMagnitude > MaxAcceleration)
            {
                acceleration.X = acceleration.X * MaxAcceleration / accelMagnitude;
                acceleration.Y = acceleration.Y * MaxAcceleration / accelMagnitude;
            }

            // Обновляем скорость и позицию
            Velocity.X += acceleration.X * timeStep;
            Velocity.Y += acceleration.Y * timeStep;

            Position.X += Velocity.X * timeStep;
            Position.Y += Velocity.Y * timeStep;

            // Проверяем достижение цели
            if (distanceToTarget < 100) // 100м - радиус зоны G
            {
                HasReachedTarget = true;
            }
        }

        public void Destroy()
        {
            IsDestroyed = true;
            Velocity = new Vector2D(0, 0);
        }
    }
}