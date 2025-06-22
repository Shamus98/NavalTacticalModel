// Инициализация сцены 200x200 км
using NavalTacticalModel;

var simulation = new SimulationWrapper();
simulation.isDebug = true;
simulation.InitializeSimulation();
simulation.RunSimulation();

// Сохранение логов
File.WriteAllLines("simulation_log.txt", simulation.EventLogs);