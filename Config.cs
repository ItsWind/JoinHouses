using System;
using System.Collections.Generic;
using System.IO;

namespace JoinHouses {
    public class Config {
        private string configFilePath = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.Length - 26) + "Modules\\JoinHouses\\config.txt";

        private Dictionary<string, double> configValues = new();
        private string configFileString =
@"-- GENERAL CONFIG --

baseRelationNeeded=35.0
> Set the base relation needed for acceptance of joining houses proposal. 35 by default
The math done to calculate relation needed proceeds as follows with 35;
- If the player is a ruler, subtract 40
- If the other hero is a ruler, add 50
- For every tier in the player's clan, subtract 8
- For every tier in the other hero's clan, add 10
";

        private void CreateConfigFile() {
            StreamWriter sw = new(configFilePath);
            sw.WriteLine(configFileString);
            sw.Close();
        }

        public void LoadConfig() {
            StreamReader sr = new(configFilePath);
            string line;
            // Read and display lines from the file until the end of
            // the file is reached.
            while ((line = sr.ReadLine()) != null) {
                int indexOfEqualSign = line.IndexOf('=');
                if (indexOfEqualSign != -1) {
                    string key = line.Substring(0, indexOfEqualSign);
                    string value = line.Substring(indexOfEqualSign + 1);
                    configValues[key] = Convert.ToDouble(value);
                }
            }
            sr.Close();
        }

        public Config() {
            if (!File.Exists(configFilePath))
                CreateConfigFile();
            LoadConfig();
        }

        private object GetValue(string key) {
            try {
                return configValues[key];
            } catch (KeyNotFoundException) {
                File.Delete(configFilePath);
                CreateConfigFile();
                LoadConfig();
                return configValues[key];
            }
        }

        public int GetValueInt(string key) {
            object value = GetValue(key);
            return Convert.ToInt32(value);
        }

        public double GetValueDouble(string key) {
            object value = GetValue(key);
            return Convert.ToDouble(value);
        }

        public bool GetValueBool(string key) {
            object value = GetValue(key);
            return Convert.ToBoolean(value);
        }
    }
}
