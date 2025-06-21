using System;
using System.Collections.Generic;

namespace NavalTacticalModel
{
    public class TacticalScene : FastAbstractWrapper
    {
        private List<RGB> _rgbs = new List<RGB>();
        private List<AttackerBEC> _attackers = new List<AttackerBEC>();
        private List<DefenderBEC> _defenders = new List<DefenderBEC>();
        private Point2D _targetPosition;
        public HexGrid HexGrid { get; private set; } // Изменено на public свойство

        public TacticalScene(Point2D targetPosition, double hexSize)
        {
            _targetPosition = targetPosition;
            updatedTime = TimeSpan.Zero;
            HexGrid = new HexGrid(hexSize); // Инициализация HexGrid
            isDebug = true;
        }

        public void AddRGB(RGB rgb)
        {
            _rgbs.Add(rgb);
            addObject(rgb);
        }

        public void AddAttacker(AttackerBEC attacker)
        {
            _attackers.Add(attacker);
            addObject(attacker);
            AddEvent(TimeSpan.Zero, new ModelEvent
            {
                EventType = ModelEventType.Start,
                AdditionalInfo = $"{attacker.Name} started mission"
            });
        }

        public void AddDefender(DefenderBEC defender)
        {
            _defenders.Add(defender);
            addObject(defender);
        }

        public void RunSimulation(TimeSpan duration)
        {
            TimeSpan endTime = updatedTime.Add(duration);
            WriteDebug($"Starting simulation for {duration.TotalSeconds} seconds");

            while (updatedTime < endTime && Next())
            {
                // Проверяем обнаружение атакующих РГБ
                foreach (var attacker in _attackers)
                {
                    if (attacker.IsDestroyed) continue;

                    foreach (var rgb in _rgbs)
                    {
                        if (rgb.DetectObject(attacker))
                        {
                            // Оповещаем защитников
                            foreach (var defender in _defenders)
                            {
                                defender.AddKnownAttacker(attacker);
                            }

                            AddEvent(updatedTime.Add(TimeSpan.FromSeconds(1)), new ModelEvent
                            {
                                EventType = ModelEventType.StartDetect,
                                AdditionalInfo = $"{attacker.Name} detected by RGB at {rgb.Position}"
                            });
                        }
                    }
                }

                // Обработка атак защитников
                var updateObjects = GetObjectsForUpdate();
                foreach (var obj in updateObjects)
                {
                    if (obj is DefenderBEC defender)
                    {
                        var (_, evt) = defender.getNearestEvent();
                        if (evt is ModelEvent modelEvent && modelEvent.EventType == ModelEventType.Attack)
                        {
                            var attacker = _attackers.Find(a => a.Name == modelEvent.AdditionalInfo);
                            if (attacker != null && !attacker.IsDestroyed)
                            {
                                attacker.MarkAsDestroyed();
                                AddEvent(updatedTime.Add(TimeSpan.FromSeconds(1)), new ModelEvent
                                {
                                    EventType = ModelEventType.Finish,
                                    AdditionalInfo = $"{attacker.Name} destroyed by {defender.Name}"
                                });
                            }
                        }
                    }
                }
            }

            WriteDebug($"Simulation completed at {updatedTime.TotalSeconds} seconds");
        }

        public void PrintStatus()
        {
            WriteDebug("\n=== Current Status ===");
            WriteDebug($"Time: {updatedTime}");

            WriteDebug("\nAttackers:");
            foreach (var attacker in _attackers)
            {
                string status = attacker.IsDestroyed ? "DESTROYED" : "ACTIVE";
                WriteDebug($"{attacker.Name}: {attacker.Position}, Engine: {attacker.CurrentEngine}, Status: {status}");
            }

            WriteDebug("\nDefenders:");
            foreach (var defender in _defenders)
            {
                WriteDebug($"{defender.Name}: {defender.Position}, Speed: {defender.Speed}");
            }

            WriteDebug("\nRGBs:");
            foreach (var rgb in _rgbs)
            {
                WriteDebug(rgb.ToString());
            }
        }
    }
}