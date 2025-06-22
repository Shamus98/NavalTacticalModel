namespace NavalTacticalModel
{
    public class DefendBEK : FastAbstractObject
    {
        public Trajectory Trajectory { get; }
        public bool IsDestroyed { get; private set; }

        public DefendBEK()
        {
            double x = new Random().NextDouble() * (SimulationConstants.DefenderZoneMaxX - SimulationConstants.DefenderZoneMinX) + SimulationConstants.DefenderZoneMinX;
            double y = new Random().NextDouble() * SimulationConstants.SceneHeight;
            Trajectory = new Trajectory(x, y, SimulationConstants.DefenderSpeed, x, y);
        }

        public void MoveToTarget(double targetX, double targetY) =>
            Trajectory.TargetX = targetX;

        public override (TimeSpan, FastAbstractEvent) getNearestEvent() =>
            (TimeSpan.MaxValue, null);

        public override void Update(TimeSpan timeSpan)
        {
            if (!IsDestroyed) Trajectory.Update(timeSpan);
        }

        public void Destroy() => IsDestroyed = true;
    }
}