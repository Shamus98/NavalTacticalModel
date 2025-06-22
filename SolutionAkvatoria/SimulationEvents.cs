using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavalTacticalModel
{
    public class DetectionEvent : FastAbstractEvent
    {
        public string DetectorId { get; set; }
        public string TargetId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            var simulation = (SimulationWrapper)wrapper;
            var defender = wrapper.GetFilteredObjects(o => o is DefendBEK).FirstOrDefault() as DefendBEK;

            if (defender != null)
            {
                defender.MoveToTarget(X, Y);
                simulation.LogEvent(EventType.Movement, timeSpan, defender.uid,
                    $"Moving to target at {simulation.FormatCoordinates(X, Y)}");
            }
        }
    }

    public class CollisionEvent : FastAbstractEvent
    {
        public string TargetId { get; set; }

        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            var simulation = (SimulationWrapper)wrapper;
            var attacker = wrapper.getObject(objId) as AttackBEK;
            var defender = wrapper.getObject(TargetId) as DefendBEK;

            if (attacker != null && defender != null)
            {
                attacker.Destroy();
                simulation.LogEvent(EventType.Combat, timeSpan, objId,
                    $"Destroyed by defender at {simulation.FormatCoordinates(attacker.Trajectory.X, attacker.Trajectory.Y)}");
                wrapper.RemoveObjects(objId);
            }
        }
    }
}
