using System;
using System.Collections.Generic;
using System.Linq;

namespace NavalTacticalModel
{
    // Основной класс моделирования
    public class TacticalSimulation
    {
        private DateTime currentTime;
        private readonly List<USV> usvs;
        private readonly List<Buoy> buoys;
        private readonly Vector2D targetZone;
        private readonly List<SimulationEvent> eventLog;
        private readonly double timeStep;

        public TacticalSimulation(Vector2D zoneG, double simulationTimeStep = 60)
        {
            currentTime = new DateTime(2025, 6, 21, 0, 0, 0);
            usvs = new List<USV>();
            buoys = new List<Buoy>();
            targetZone = zoneG;
            eventLog = new List<SimulationEvent>();
            timeStep = simulationTimeStep;
        }

        public void AddUSV(USV usv)
        {
            usvs.Add(usv);
            LogEvent("Start", usv.Id, usv.Position);
        }

        public void AddBuoy(Buoy buoy)
        {
            buoys.Add(buoy);
        }

        public void RunSimulation(double totalSimulationTime)
        {
            int steps = (int)(totalSimulationTime / timeStep);

            for (int i = 0; i < steps; i++)
            {
                currentTime = currentTime.AddSeconds(timeStep);

                // Обновляем позиции всех БЭК
                foreach (var usv in usvs)
                {
                    if (!usv.IsDestroyed && !usv.HasReachedTarget)
                    {
                        usv.Move(timeStep, targetZone);

                        // Проверяем достижение цели
                        if (usv.HasReachedTarget)
                        {
                            LogEvent("Finish", usv.Id, usv.Position);
                        }
                    }
                }

                // Проверяем обнаружение БЭК буями
                foreach (var buoy in buoys)
                {
                    foreach (var usv in usvs)
                    {
                        bool detected = buoy.DetectUSV(usv);
                        if (detected)
                        {
                            LogEvent("StartDetection", buoy.Id, usv.Position);

                            // В этой простой модели при обнаружении БЭК уничтожается
                            usv.Destroy();
                            LogEvent("Destroyed", usv.Id, usv.Position);
                        }
                    }
                }
            }
        }

        private void LogEvent(string eventType, string objectId, Vector2D coordinates)
        {
            var evt = new SimulationEvent(currentTime, eventType, objectId, coordinates);
            eventLog.Add(evt);
            Console.WriteLine(evt);
        }

        public void PrintEventLog()
        {
            Console.WriteLine("\nEvent Log:");
            Console.WriteLine("Time     Event           ObjID   Coord");
            foreach (var evt in eventLog)
            {
                Console.WriteLine(evt);
            }
        }

        public void PrintSimulationResults()
        {
            int destroyed = usvs.Count(u => u.IsDestroyed);
            int reachedTarget = usvs.Count(u => u.HasReachedTarget);
            int active = usvs.Count(u => !u.IsDestroyed && !u.HasReachedTarget);

            Console.WriteLine("\nSimulation Results:");
            Console.WriteLine($"Total USVs: {usvs.Count}");
            Console.WriteLine($"Destroyed: {destroyed}");
            Console.WriteLine($"Reached target: {reachedTarget}");
            Console.WriteLine($"Still active: {active}");
        }
    }
}
