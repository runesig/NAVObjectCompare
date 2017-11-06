using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;

namespace NAVObjectCompareWinClient.Configurations
{
    public class ConfigurationAppSettings
    {
        private Configuration _configuration;

        private const string AppSettingsSection = "appSettings";
        private const string EditorPathKey = "EditorPath";
        private const string ShowFullExceptionKey = "ShowFullException";
        private const string ImportFromServerKeyA = "ImportFromServerA";
        private const string InportFromFileKeyA = "InportFromFileA";

        public ConfigurationAppSettings()
        {
            _configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public void Save()
        {
            _configuration.Save(ConfigurationSaveMode.Minimal, true);
            ConfigurationManager.RefreshSection(AppSettingsSection);
        }

        public string EditorPath
        {
            get { return GetAppSettingStringValue(EditorPathKey); }
            set { SetAppSettingValue(EditorPathKey, value); }
        }

        public bool ShowFullException
        {
            get { return GetAppSettingBoolValue(ShowFullExceptionKey); }
            set { SetAppSettingValue(ShowFullExceptionKey, value); }
        }

        public bool InportFromServerA
        {
            get { return GetAppSettingBoolValue(ImportFromServerKeyA); }
            set { SetAppSettingValue(ImportFromServerKeyA, value); }
        }

        public bool InportFromFileA
        {
            get { return GetAppSettingBoolValue(InportFromFileKeyA); }
            set { SetAppSettingValue(InportFromFileKeyA, value); }
        }

        // Private
        private void SetAppSettingValue(string key, object value)
        {
            _configuration.AppSettings.Settings[key].Value = value.ToString();
        }

        private bool GetAppSettingBoolValue(string key)
        {
            if (bool.TryParse(_configuration.AppSettings.Settings[key].Value, out bool boolValue))
                return boolValue;

            return false;
        }

        private int GetAppSettingIntValue(string key)
        {
            if (int.TryParse(_configuration.AppSettings.Settings[key].Value, out int intValue))
                return intValue;

            return 0;
        }

        private string GetAppSettingStringValue(string key)
        {
            return _configuration.AppSettings.Settings[key].Value;
        }
    }
}
