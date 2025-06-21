using System;

namespace NavalTacticalModel
{
    class Program
    {
        static void Main(string[] args)
        {
            // Параметры из таблицы 3.1
            double R0 = 1000; // радиус соты
            double R1 = 1000; // дальность обнаружения на бензине
            double R2 = 500;  // дальность обнаружения на электричестве
            double aTrA = 5;   // ускорение БЭК типа A
            double V1A = 10;   // скорость на бензине
            double V2A = 3;    // скорость на электричестве
            double RFPV = 5000; // радиус поражения

            // Создаем симуляцию
            var simulation = new TacticalSimulation(
                zoneG: new Vector2D(10000, 500), // зона G
                simulationTimeStep: 60); // шаг времени 60 секунд

            // Добавляем буи (случайным образом с вероятностью 0.75)
            Random rand = new Random();
            int buoyCount = 0;

            for (int i = -5; i <= 5; i++)
            {
                for (int j = -5; j <= 5; j++)
                {
                    if (rand.NextDouble() < 0.75) // вероятность установки буя
                    {
                        double x = i * R0 * 1.5;
                        double y = j * R0 * Math.Sqrt(3);
                        simulation.AddBuoy(new Buoy(
                            $"B{++buoyCount:000}",
                            new Vector2D(x, y),
                            R1, R2));
                    }
                }
            }

            // Добавляем БЭК типа A
            simulation.AddUSV(new USV(
                "A001",
                startPosition: new Vector2D(-3 * R0, 0), // начальная позиция в зоне B
                maxAcceleration: aTrA,
                gasolineSpeed: V1A,
                electricSpeed: V2A));

            // Запускаем симуляцию на 1 час
            simulation.RunSimulation(totalSimulationTime: 3600);

            // Выводим результаты
            simulation.PrintSimulationResults();
            simulation.PrintEventLog();
        }
    }
}
