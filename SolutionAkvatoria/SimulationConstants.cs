using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavalTacticalModel
{
    public static class SimulationConstants
    {
        // Масштаб сцены
        public const double SceneWidth = 200000; // 200 км в метрах
        public const double SceneHeight = 200000; // 200 км в метрах

        // Зоны
        public const double AttackerZoneMinX = 0;
        public const double AttackerZoneMaxX = 50000;
        public const double AttackerZoneMinY = 0;
        public const double AttackerZoneMaxY = 200000;

        public const double DefenderZoneMinX = 150000;
        public const double DefenderZoneMaxX = 200000;
        public const double DefenderZoneMinY = 0;
        public const double DefenderZoneMaxY = 200000;

        // Физические параметры
        public const double AttackerGasolineSpeed = 10;
        public const double AttackerElectricSpeed = 3;
        public const double DefenderSpeed = 7;
        public const double MaxTransverseAcceleration = 5;

        // Радиусы
        public const double DetectionRadiusGasoline = 1000;
        public const double DetectionRadiusElectric = 500;
        public const double AttackRadius = 5000;

        // Размеры
        public const double BEKSize = 1;
        public const double BuoySize = 1;

        // Количество
        public const int BuoyCount = 10;
    }
    public enum EventType
    {
        System,
        Creation,
        Movement,
        Detection,
        Combat,
        Victory,
        Error
    }
}
