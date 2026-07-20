using Monocle;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Celeste.Mod.Aqua.Miscellaneous
{
    public class DataContainer
    {
        public static DataContainer For(Entity obj)
        {
            DataContainer data;
            if (!_data.TryGetValue(obj, out data))
            {
                data = new DataContainer(32);
                _data.Add(obj, data);
            }
            return data;
        }

        public static bool Has(Entity obj)
        {
            return _data.TryGetValue(obj, out _);
        }

        static ConditionalWeakTable<Entity, DataContainer> _data = new ConditionalWeakTable<Entity, DataContainer>();

        public bool Has(string key)
        {
            return _values.ContainsKey(key);
        }

        public T Get<T>(string key, T defaultValue = default)
        {
            if (_values.TryGetValue(key, out object value))
            {
                if (value is T tValue)
                    return tValue;
                else
                    return defaultValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public void Set(string key, object value)
        {
            _values[key] = value;
        }

        public void Remove(string key)
        {
            _values.Remove(key);
        }

        public IReadOnlyDictionary<string, object> GetValues()
        {
            return _values;
        }

        public DataContainer() : this(32)
        { }

        public DataContainer(int capacity)
        {
            _values = new Dictionary<string, object>(capacity);
        }

        Dictionary<string, object> _values;
    }
}
