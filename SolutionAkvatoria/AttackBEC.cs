namespace NavalTacticalModel
{
    public class AttackBEK : FastAbstractObject
    {
        private readonly Random random = new();
        public Trajectory Trajectory { get; }
        public bool IsElectric { get; }
        public bool IsDestroyed { get; private set; }

        public AttackBEK()
        {
            double x = random.NextDouble() * (SimulationConstants.AttackerZoneMaxX - SimulationConstants.AttackerZoneMinX) + SimulationConstants.AttackerZoneMinX;
            double y = random.NextDouble() * SimulationConstants.SceneHeight;

            double targetX = SimulationConstants.DefenderZoneMinX +
                           random.NextDouble() * (SimulationConstants.DefenderZoneMaxX - SimulationConstants.DefenderZoneMinX);
            double targetY = random.NextDouble() * SimulationConstants.SceneHeight;

            IsElectric = random.NextDouble() > 0.5;
            double speed = IsElectric ? SimulationConstants.AttackerElectricSpeed : SimulationConstants.AttackerGasolineSpeed;

            Trajectory = new Trajectory(x, y, speed, targetX, targetY);
        }

        public override (TimeSpan, FastAbstractEvent) getNearestEvent() =>
            (TimeSpan.MaxValue, null);

        public override void Update(TimeSpan timeSpan)
        {
            if (!IsDestroyed) Trajectory.Update(timeSpan);
        }

        public void Destroy() => IsDestroyed = true;
    }
}