using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunkbedBOMRouting.Classes
{
    /// <summary>
    /// Custom struct to represent a step in the assembly process. It encapsulates an integer value and ensures that it is non-negative.
    /// </summary>
    public struct Step
    {
        private int _value;
        public Step(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"Step value must be non-negative, got {value}");

            _value = value;
        }
        public static implicit operator int(Step step) => step._value;

        public static explicit operator Step(int value) => new Step(value);
        public static explicit operator Step(long value) => new Step((int)value);
        public static explicit operator Step(double value) => new Step((int)value);
        public static explicit operator Step(decimal value) => new Step((int)value);

        public override string ToString() => _value.ToString();
    }
}
