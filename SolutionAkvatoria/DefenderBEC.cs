using System;
using System.Collections.Generic;

namespace NavalTacticalModel
{
    public class DefenderBEC : BEC
    {
        public double Speed { get; private set; }
        public double AttackRadius { get; private set; }
        private List<AttackerBEC> _knownAttackers = new List<AttackerBEC>();
        private Random _random = new Random();
        private TimeSpan _lastDirectionChangeTime;

        public DefenderBEC(Point2D startPosition, ConflictSide side, string name,
                         double maxAcceleration, double speed, double attackRadius)
            : base(startPosition, side, name)
        {
            Speed = speed;
            MaxTransverseAcceleration = maxAcceleration;
            AttackRadius = attackRadius;
            _lastDirectionChangeTime = TimeSpan.Zero;

            // Начальное случайное направление
            ChangePatrolDirection();
        }

        public void AddKnownAttacker(AttackerBEC attacker)
        {
            if (!_knownAttackers.Contains(attacker))
            {
                _knownAttackers.Add(attacker);
            }
        }

        private void ChangePatrolDirection()
        {
            double angle = _random.NextDouble() * Math.PI * 2;
            Velocity = new Point2D(Math.Cos(angle) * Speed, Math.Sin(angle) * Speed);
            _lastDirectionChangeTime = lastUpdated;
        }

        public override (TimeSpan, FastAbstractEvent) getNearestEvent()
        {
            // Проверка возможности атаки известных атакующих БЭК
            foreach (var attacker in _knownAttackers)
            {
                if (!attacker.IsDestroyed && Position.DistanceTo(attacker.Position) <= AttackRadius)
                {
                    return (lastUpdated.Add(TimeSpan.FromSeconds(1)),
                           new ModelEvent
                           {
                               EventType = ModelEventType.Attack,
                               AdditionalInfo = attacker.Name
                           });
                }
            }

            // Удаляем уничтоженные БЭК из списка известных
            _knownAttackers.RemoveAll(a => a.IsDestroyed);

            // Изменение направления патрулирования (не чаще чем раз в 60 секунд)
            if ((lastUpdated - _lastDirectionChangeTime).TotalSeconds > 60 &&
                _random.NextDouble() < 0.05) // 5% chance to change direction
            {
                return (lastUpdated.Add(TimeSpan.FromSeconds(_random.Next(1, 5))),
                       new ModelEvent
                       {
                           EventType = ModelEventType.ChangeEngine,
                           AdditionalInfo = "Patrol direction change"
                       });
            }

            return (TimeSpan.MaxValue, null);
        }
    }
}