using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Microsoft.Xna.Framework.GamerServices
{
    public sealed class PropertyDictionary :
      IDictionary<string, object>,
      ICollection<KeyValuePair<string, object>>,
      IEnumerable<KeyValuePair<string, object>>,
      IEnumerable
    {
        private Dictionary<string, PropertyValue> properties;
        private bool isReadOnly;
        private bool fillingReadOnlyData;
        private int changedPropertyCount;

        internal int ChangedPropertyCount => this.changedPropertyCount;

        internal PropertyDictionary(bool isReadOnly, int capacity)
        {
            this.isReadOnly = isReadOnly;
            this.properties = new Dictionary<string, PropertyValue>(capacity);
        }

        internal PropertyValue Add(string key, PropertyValue propertyValue)
        {
            this.properties.Add(key, propertyValue);
            if (!this.fillingReadOnlyData)
                ++this.changedPropertyCount;
            return propertyValue;
        }

        internal PropertyValue GetProperty(string key, bool demandCreate)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            PropertyValue propertyValue;
            if (this.properties.TryGetValue(key, out propertyValue))
                return propertyValue;
            return null;
        }

        internal void Reset()
        {
            this.properties.Clear();
            this.changedPropertyCount = 0;
        }

        internal void BeginFillData() => this.fillingReadOnlyData = true;

        internal void EndFillData() => this.fillingReadOnlyData = false;

        public int GetValueInt32(string key) => this.GetTypedValue<int>(key);

        public long GetValueInt64(string key) => this.GetTypedValue<long>(key);

        public float GetValueSingle(string key) => this.GetTypedValue<float>(key);

        public double GetValueDouble(string key) => this.GetTypedValue<double>(key);

        public string GetValueString(string key) => Convert.ToString(this.GetProperty(key, false).GetValue(), (IFormatProvider)CultureInfo.InvariantCulture);

        public LeaderboardOutcome GetValueOutcome(string key) => (LeaderboardOutcome)this.GetValueInt32(key);

        public DateTime GetValueDateTime(string key) => new DateTime(this.GetValueInt64(key));

        public TimeSpan GetValueTimeSpan(string key) => new TimeSpan(this.GetValueInt64(key));

        public Stream GetValueStream(string key) => this.GetProperty(key, false).GetValue() as Stream;

        private T GetTypedValue<T>(string key) where T : struct, IEquatable<T>
        {
            PropertyValue property = this.GetProperty(key, false);
            return property is TypedPropertyValue<T> typedPropertyValue ? typedPropertyValue.GetTypedValue() : (T)Convert.ChangeType(property.GetValue(), typeof(T), (IFormatProvider)CultureInfo.InvariantCulture);
        }

        public void SetValue(string key, int value) => this.SetTypedValue<int>(key, value);

        public void SetValue(string key, long value) => this.SetTypedValue<long>(key, value);

        public void SetValue(string key, float value) => this.SetTypedValue<float>(key, value);

        public void SetValue(string key, double value) => this.SetTypedValue<double>(key, value);

        public void SetValue(string key, string value) => this.SetUntypedValue(key, (object)value);

        public void SetValue(string key, LeaderboardOutcome value) => this.SetTypedValue<int>(key, (int)value);

        public void SetValue(string key, DateTime value) => this.SetTypedValue<long>(key, value.Ticks);

        public void SetValue(string key, TimeSpan value) => this.SetTypedValue<long>(key, value.Ticks);

        private void SetUntypedValue(string key, object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (this.isReadOnly && this.fillingReadOnlyData)
                throw new NotSupportedException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Read only", (object)typeof(PropertyDictionary).Name));
            PropertyValue property = this.GetProperty(key, true);
            bool isChanged = property.IsChanged;
            property.SetValue(value);
            if (!property.IsChanged || isChanged)
                return;
            ++this.changedPropertyCount;
        }

        private void SetTypedValue<T>(string key, T value) where T : struct, IEquatable<T>
        {
            if (this.isReadOnly && this.fillingReadOnlyData)
                throw new NotSupportedException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Read only", (object)typeof(PropertyDictionary).Name));
            PropertyValue property = this.GetProperty(key, true);
            bool isChanged = property.IsChanged;
            if (property is TypedPropertyValue<T> typedPropertyValue)
                typedPropertyValue.SetTypedValue(value);
            else
                property.SetValue((object)value);
            if (!property.IsChanged || isChanged || this.fillingReadOnlyData)
                return;
            ++this.changedPropertyCount;
            // Write to leaderboard
        }

        public int Count => this.properties.Count;

        public bool ContainsKey(string key) => this.properties.ContainsKey(key);

        public object this[string key]
        {
            get => this.GetProperty(key, false).GetValue();
            set => this.SetUntypedValue(key, value);
        }

        public bool TryGetValue(string key, out object value)
        {
            PropertyValue propertyValue;
            if (this.properties.TryGetValue(key, out propertyValue))
            {
                value = propertyValue.GetValue();
                return true;
            }
            value = (object)null;
            return false;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            foreach (KeyValuePair<string, PropertyValue> property in this.properties)
                yield return new KeyValuePair<string, object>(property.Key, property.Value.GetValue());
        }

        IEnumerator IEnumerable.GetEnumerator() => (IEnumerator)this.GetEnumerator();

        ICollection<string> IDictionary<string, object>.Keys => (ICollection<string>)this.properties.Keys;

        ICollection<object> IDictionary<string, object>.Values
        {
            get
            {
                List<object> objectList = new List<object>(this.properties.Count);
                foreach (PropertyValue propertyValue in this.properties.Values)
                    objectList.Add(propertyValue.GetValue());
                return (ICollection<object>)objectList;
            }
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(
          KeyValuePair<string, object> item)
        {
            object objB;
            return this.TryGetValue(item.Key, out objB) && object.Equals(item.Value, objB);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(
          KeyValuePair<string, object>[] array,
          int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            foreach (KeyValuePair<string, object> keyValuePair in this)
                array[arrayIndex++] = keyValuePair;
        }

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly => true;

        void IDictionary<string, object>.Add(string key, object value) => throw new NotSupportedException();

        void ICollection<KeyValuePair<string, object>>.Add(
          KeyValuePair<string, object> item)
        {
            throw new NotSupportedException();
        }

        void ICollection<KeyValuePair<string, object>>.Clear() => throw new NotSupportedException();

        bool IDictionary<string, object>.Remove(string key) => throw new NotSupportedException();

        bool ICollection<KeyValuePair<string, object>>.Remove(
          KeyValuePair<string, object> item)
        {
            throw new NotSupportedException();
        }
    }
}
