using AbstractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkvatoriaModel
{
    internal class ChangeAcceleration : FastAbstractEvent
    {
        private UMSL warehouseRobot;
        private double acceleration;

        public ChangeAcceleration(UMSL warehouseRobot, double acceleration)
        {
            this.warehouseRobot = warehouseRobot;
            this.acceleration = acceleration;
        }

        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            warehouseRobot.SetAcceleration(acceleration);

            AkvatoriaWrapper warehouseWrapper = wrapper as AkvatoriaWrapper;
            warehouseWrapper.logs.Add(new WarehouseEvent(timeSpan, WarehouseEventType.ChangeAcceleration, WarehouseObjectType.Robot, warehouseRobot, ""));
            warehouseWrapper.instructions.Add(new RMSInstruction(timeSpan, WarehouseEventType.ChangeAcceleration, WarehouseObjectType.Robot, warehouseRobot, ""));
            warehouseRobot.FinishLocalEvent();
        }
    }
}
