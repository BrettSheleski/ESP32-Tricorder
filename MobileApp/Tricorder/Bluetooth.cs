using System;
using System.Collections.Generic;
using System.Text;

namespace Tricorder
{
    public static class Bluetooth
    {
        public static Guid TricorderServiceId { get; } = new Guid("61879ac8-5b8e-40bb-8165-53b531cf02bf");
        public static Guid FartSensorCharacteristicId { get; } = new Guid("0051bd71-ed61-4b91-86e6-07154a2c441d");
    }
}
