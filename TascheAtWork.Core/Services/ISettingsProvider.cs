using TascheAtWork.Core.Infrastructure;

namespace TascheAtWork.Core.Services
{
    public interface ISettingsProvider
    {
        void Save(SettingsKey key, string textToSave);
        string Load(SettingsKey key);
    }
}