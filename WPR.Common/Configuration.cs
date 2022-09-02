using System;
using System.IO;
using Newtonsoft.Json;

namespace WPR.Common
{
    public class Configuration
    {
        private class ConfigurationPrivate
        {
            public string DataStorePath;
            public String GamerTag;
        };

        private const string ConfigurationFilePath = "config.json";
        private static Configuration _Current;
        private ConfigurationPrivate? _ConfPrivate;

        public string? DataStorePath
        {
            get => _ConfPrivate!.DataStorePath;
            set => _ConfPrivate!.DataStorePath = value;
        }

        public string? GamerTag
        {
            get => _ConfPrivate!.GamerTag;
            set => _ConfPrivate!.GamerTag = value;
        }

        public static Configuration Current
        {
            get
            {
                if (_Current == null)
                {
                    _Current = new Configuration();
                }

                return _Current;
            }
        }

        private static string ConfigurationFilePathFull => Path.Combine(Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData), "WPR"), ConfigurationFilePath);

        public void RestoreDefaultDataStoragePath()
        {
            DataStorePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WPR");
        }

        public Configuration()
        {
            try
            {
                var seralizer = new JsonSerializer();
                var configurationFilePathFull = 

                _ConfPrivate = JsonConvert.DeserializeObject<ConfigurationPrivate>(File.ReadAllText(ConfigurationFilePathFull));
            } catch (Exception ex)
            {
                Log.Error(LogCategory.Common, $"Failed to load configuration file with error: {ex}");
                _ConfPrivate = new ConfigurationPrivate();
            }

            if (DataStorePath == null)
            {
                DataStorePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WPR");
            }
        }

        ~Configuration()
        {
            Save();
        }

        public string DataPath(string path)
        {
            return Path.Combine(DataStorePath!, path);
        }
        public void Save()
        {
            File.WriteAllText(ConfigurationFilePathFull, JsonConvert.SerializeObject(_ConfPrivate));
        }
    }
}
