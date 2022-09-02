using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;

using WPR.Common;

namespace WPR.WindowsCompability
{
    public sealed class IsolatedStorageSettings : IDictionary<string, object>, IDictionary,
        ICollection<KeyValuePair<string, object>>, ICollection,
        IEnumerable<KeyValuePair<string, object>>, IEnumerable
    {
        private static IsolatedStorageSettings _ApplicationSettings;
        private const string LocalSettingsName = "__LocalSettings";

        private IsolatedStorageFile _Holder;
        private Dictionary<string, object> _Settings;

        internal IsolatedStorageSettings(IsolatedStorageFile file)
        {
            _Holder = file;
            if (!file.FileExists(LocalSettingsName))
            {
                _Settings = new Dictionary<string, object>();
            } else
            {
                using (IsolatedStorageFileStream fs = file.OpenFile(LocalSettingsName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        DataContractSerializer reader = new DataContractSerializer(typeof(Dictionary<string, object>));
                        try
                        {
                            _Settings = (reader.ReadObject(fs) as Dictionary<string, object>)!;
                        }
                        catch (Exception ex)
                        {
                            Log.Error(LogCategory.Common, $"Failed to deserialize isolated settings. Error\n {ex}");
                        }

                        if (_Settings == null)
                        {
                            _Settings = new Dictionary<string, object>();
                        }
                    }
                }
            }
        }

        ~IsolatedStorageSettings()
        {
            Save();
        }

        public void Save()
        {
            using (IsolatedStorageFileStream storage = _Holder.CreateFile(LocalSettingsName))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, object>));
                serializer.WriteObject(storage, _Settings);
            }
        }


        public static IsolatedStorageSettings ApplicationSettings
        {
            get
            {
                if (_ApplicationSettings == null)
                {
                    _ApplicationSettings = new IsolatedStorageSettings(IsolatedStorageFile.GetUserStoreForApplication());
                }

                return _ApplicationSettings;
            }
        }

        public object this[string key] { 
            get => _Settings[key];
            set => _Settings[key] = value;
        }

        public object? this[object key]
        {
            get
            {
                string? keyString = key as string;
                if (keyString == null)
                {
                    return null;
                }
                if (!_Settings.ContainsKey(keyString))
                {
                    return null;
                }

                return _Settings[keyString];
            }
            set
            {
                string? keyString = key as string;
                if (keyString == null)
                {
                    return;
                }
                if (!_Settings.ContainsKey(keyString))
                {
                    return;
                }

                _Settings[keyString] = value!;
            }
        }

        public ICollection<string> Keys => _Settings.Keys;

        public ICollection<object> Values => _Settings.Values;

        public int Count => _Settings.Count;

        public bool IsReadOnly => false;

        public bool IsSynchronized => (_Settings as ICollection).IsSynchronized;

        public object SyncRoot => (_Settings as ICollection).SyncRoot;

        public bool IsFixedSize => false;

        ICollection IDictionary.Keys => _Settings.Keys;

        ICollection IDictionary.Values => _Settings.Values;

        public void Add(string key, object value)
        {
            _Settings.Add(key, value);
        }

        public void Add(KeyValuePair<string, object> item)
        {
            _Settings.Add(item.Key, item.Value);
        }

        public void Add(object key, object? value)
        {
            _Settings.Add((key as string)!, value!);
        }

        public void Clear()
        {
            _Settings.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return Contains(item.Key);
        }

        public bool Contains(object key)
        {
            return Contains((key as string)!);
        }

        public bool Contains(string key)
        {
            return _Settings.ContainsKey(key);
        }

        public bool ContainsKey(string key)
        {
            return _Settings.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _Settings.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return _Settings.Remove(key);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return Remove(item.Key);
        }

        public void Remove(object key)
        {
            Remove((key as string)!);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out object value)
        {
            return _Settings.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Settings.GetEnumerator();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return _Settings.GetEnumerator();
        }
    }
}