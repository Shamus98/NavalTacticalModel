using System;

namespace NavalTacticalModel
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создаем тактическую сцену
           var targetPosition = new Point2D(10000, 500); // Цель в зоне G
        var scene = new TacticalScene(targetPosition, 1000); // Размер соты 1000м

        // Добавляем РГБ (вероятность 75% по документу)
        var random = new Random();
        for (int x = -5; x <= 5; x++)
        {
            for (int y = -5; y <= 5; y++)
            {
                if (random.NextDouble() < 0.75) // 75% вероятность установки буя
                {
                    var hexCenter = scene.HexGrid.GetHexCenter(x, y); // Теперь обращаемся через свойство
                    var rgb = new RGB(hexCenter, 1000, 500); // R1=1000, R2=500
                    scene.AddRGB(rgb);
                }
            }
        }

            // Добавляем атакующие БЭК (сторона B)
            for (int i = 1; i <= 3; i++)
            {
                var startPos = new Point2D(-3000 - i * 500, i * 500); // Начинают западнее зоны B
                var attacker = new AttackerBEC(
                    startPos, targetPosition, ConflictSide.Blue,
                    $"Attacker-{i}", 5, 10, 3); // aTr=5, V1=10, V2=3
                scene.AddAttacker(attacker);
            }

            // Добавляем защитные БЭК (сторона G)
            for (int i = 1; i <= 2; i++)
            {
                var startPos = new Point2D(5000 + i * 1000, i * 1000); // Начинают в зоне G
                var defender = new DefenderBEC(
                    startPos, ConflictSide.Green,
                    $"Defender-{i}", 5, 7, 5000); // aTr=5, V1=7, R_FPV=5000
                scene.AddDefender(defender);
            }

            // Запускаем симуляцию на 1 час
            scene.RunSimulation(TimeSpan.FromHours(1));

            // Выводим итоговый статус
            scene.PrintStatus();
        }
    }
}