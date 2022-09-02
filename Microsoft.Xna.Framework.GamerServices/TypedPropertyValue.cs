using System;
using System.Globalization;

namespace Microsoft.Xna.Framework.GamerServices
{
    internal class TypedPropertyValue<T> : PropertyValue where T : IEquatable<T>
    {
        protected T currentValue;

        public override object GetValue() => (object)this.currentValue;

        public T GetTypedValue() => this.currentValue;

        public override void SetValue(object value) => this.SetTypedValue((T)Convert.ChangeType(value, typeof(T), (IFormatProvider)CultureInfo.InvariantCulture));

        public void SetTypedValue(T value)
        {
            if (value.Equals(this.currentValue))
                return;
            this.currentValue = value;
            this.IsChanged = true;
        }
    }
}
