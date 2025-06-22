using AbstractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkvatoriaModel
{
    internal class ChangeAccelerationRotation : FastAbstractEvent
    {
        private UMSL warehouseRobot;
        private double accelerationDeg; // Угловое ускорение в градусах/с²

        public ChangeAccelerationRotation(UMSL warehouseRobot, double accelerationDeg)
        {
            this.warehouseRobot = warehouseRobot;
            this.accelerationDeg = accelerationDeg;
        }

        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            warehouseRobot.SetAccelerationRotation(accelerationDeg);

            AkvatoriaWrapper warehouseWrapper = wrapper as AkvatoriaWrapper;
            warehouseWrapper.logs.Add(new WarehouseEvent(timeSpan, WarehouseEventType.ChangeAccelerationRotation, WarehouseObjectType.Robot, warehouseRobot, ""));
            warehouseWrapper.instructions.Add(new RMSInstruction(timeSpan, WarehouseEventType.ChangeAccelerationRotation, WarehouseObjectType.Robot, warehouseRobot, ""));


            warehouseRobot.FinishLocalEvent();
        }
    }
}
