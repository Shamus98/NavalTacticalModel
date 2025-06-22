using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavalTacticalModel
{
    public class SimulationWrapper : FastAbstractWrapper
    {
        private readonly Random random = new();
        private readonly List<string> eventLogs = new();

        public List<string> EventLogs => eventLogs;

        public void InitializeSimulation()
        {
            eventLogs.Clear();
            LogEvent(EventType.System, TimeSpan.Zero, "SYSTEM", "Simulation started");

            // Create attacker
            var attacker = new AttackBEK();
            addObject(attacker);
            LogEvent(EventType.Creation, TimeSpan.Zero, attacker.uid,
                $"Created at {FormatCoordinates(attacker.Trajectory.X, attacker.Trajectory.Y)} ({(attacker.IsElectric ? "Electric" : "Gasoline")})");

            // Create defender
            var defender = new DefendBEK();
            addObject(defender);
            LogEvent(EventType.Creation, TimeSpan.Zero, defender.uid,
                $"Created at {FormatCoordinates(defender.Trajectory.X, defender.Trajectory.Y)}");

            // Create buoys
            for (int i = 0; i < SimulationConstants.BuoyCount; i++)
            {
                var (x, y) = GenerateValidCoordinates();
                bool isElectric = random.NextDouble() > 0.5;
                var buoy = new RGB(x, y, isElectric);
                addObject(buoy);
                LogEvent(EventType.Creation, TimeSpan.Zero, buoy.uid,
                    $"Created at {FormatCoordinates(x, y)} ({(isElectric ? "Electric" : "Gasoline")})");
            }
        }

        public void RunSimulation()
        {
            while (Next())
            {
                var attacker = GetFilteredObjects(o => o is AttackBEK).FirstOrDefault() as AttackBEK;
                var defender = GetFilteredObjects(o => o is DefendBEK).FirstOrDefault() as DefendBEK;

                if (attacker == null || defender == null) break;

                CheckVictoryConditions(attacker, defender);
                CheckCollisions(attacker, defender);
                CheckDetections(attacker);
            }
        }

        private (double x, double y) GenerateValidCoordinates()
        {
            int attempts = 0;
            while (attempts++ < 1000)
            {
                double x = random.NextDouble() * SimulationConstants.SceneWidth;
                double y = random.NextDouble() * SimulationConstants.SceneHeight;

                if (!IsInForbiddenZone(x, y))
                    return (x, y);
            }
            throw new Exception("Failed to generate valid coordinates");
        }

        private bool IsInForbiddenZone(double x, double y) =>
            (x >= SimulationConstants.AttackerZoneMinX && x <= SimulationConstants.AttackerZoneMaxX) ||
            (x >= SimulationConstants.DefenderZoneMinX && x <= SimulationConstants.DefenderZoneMaxX);

        private void CheckVictoryConditions(AttackBEK attacker, DefendBEK defender)
        {
            if (attacker.Trajectory.X >= SimulationConstants.DefenderZoneMinX)
            {
                LogEvent(EventType.Victory, updatedTime, attacker.uid,
                    "Reached defender zone - ATTACKER WINS");
                eventList.Clear();
            }
        }

        private void CheckCollisions(AttackBEK attacker, DefendBEK defender)
        {
            if (attacker.Trajectory.DistanceTo(defender.Trajectory.X, defender.Trajectory.Y) <= SimulationConstants.BEKSize)
            {
                AddEvent(updatedTime, new CollisionEvent { TargetId = defender.uid }, attacker.uid);
            }
        }

        private void CheckDetections(AttackBEK attacker)
        {
            foreach (var obj in objects.Values.Where(o => o is RGB))
            {
                var buoy = (RGB)obj;
                if (buoy.Detect(attacker.Trajectory.X, attacker.Trajectory.Y))
                {
                    LogEvent(EventType.Detection, updatedTime, buoy.uid,
                        $"Detected attacker at {FormatCoordinates(attacker.Trajectory.X, attacker.Trajectory.Y)}");

                    AddEvent(updatedTime, new DetectionEvent
                    {
                        DetectorId = buoy.uid,
                        TargetId = attacker.uid,
                        X = attacker.Trajectory.X,
                        Y = attacker.Trajectory.Y
                    }, buoy.uid);
                }
            }
        }

        public void LogEvent(EventType type, TimeSpan time, string objectId, string message)
        {
            string formattedTime = $"{time.TotalSeconds:F2}s";
            string shortId = objectId.Length > 8 ? objectId.Substring(0, 8) : objectId;
            string logEntry = $"{formattedTime,-8} [{type.ToString()[0]}] {shortId,-8} {message}";

            eventLogs.Add(logEntry);
            writeDebug(logEntry);
        }

        public string FormatCoordinates(double x, double y)
        {
            if (x < 0 || x > SimulationConstants.SceneWidth ||
                y < 0 || y > SimulationConstants.SceneHeight)
            {
                return "(INVALID)";
            }
            return $"({x / 1000:F1}km, {y / 1000:F1}km)";
        }
    }
}
