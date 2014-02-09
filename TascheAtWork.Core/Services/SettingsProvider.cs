using System;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
using TascheAtWork.Core.Infrastructure;
using TascheAtWork.Core.Properties;

namespace TascheAtWork.Core.Services
{
    public class SettingsProvider : ISettingsProvider
    {

        private readonly byte[] _salt = Encoding.Unicode.GetBytes("It was a bright cold day in April, and the clocks were striking thirteen");

        public void Save(SettingsKey key, string textToSave)
        {
            Settings.Default[key.ToString()] = EncryptString(textToSave);
            Settings.Default.Save();
        }

        public string Load(SettingsKey key)
        {
            var encryptedString = Settings.Default[key.ToString()] as string;
            return String.IsNullOrEmpty(encryptedString) ? String.Empty : DecryptString(encryptedString);
        }

        private string EncryptString(string input)
        {
            try
            {
                var encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(input), _salt, DataProtectionScope.CurrentUser);
                return Convert.ToBase64String(encryptedData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[EncryptString] failed: " + ex.Message);
                return string.Empty;
            }
        }

        private string DecryptString(string encryptedData)
        {
            try
            {
                var decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), _salt, DataProtectionScope.CurrentUser);
                return Encoding.Unicode.GetString(decryptedData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[DecryptString] failed: " + ex.Message);
                return string.Empty;
            }
        }


    }
}
