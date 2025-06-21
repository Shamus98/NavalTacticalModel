using System;

namespace NavalTacticalModel
{
    public abstract class BEC : FastAbstractObject
    {
        public Point2D Position { get; protected set; }
        public Point2D Velocity { get; protected set; }
        public double MaxTransverseAcceleration { get; protected set; }
        public ConflictSide Side { get; protected set; }
        public string Name { get; protected set; }

        protected BEC(Point2D startPosition, ConflictSide side, string name)
        {
            Position = startPosition;
            Side = side;
            Name = name;
            lastUpdated = TimeSpan.Zero;
            Velocity = new Point2D(0, 0);
        }

        public void UpdatePosition(TimeSpan deltaTime)
        {
            double deltaSeconds = deltaTime.TotalSeconds;
            Position = new Point2D(
                Position.X + Velocity.X * deltaSeconds,
                Position.Y + Velocity.Y * deltaSeconds
            );
        }

        public override void Update(TimeSpan timeSpan)
        {
            TimeSpan deltaTime = timeSpan - lastUpdated;
            UpdatePosition(deltaTime);
            lastUpdated = timeSpan;
        }

        public abstract override (TimeSpan, FastAbstractEvent) getNearestEvent();

        public override string ToString()
        {
            return $"{Name} ({Side}) at {Position}";
        }
    }
}