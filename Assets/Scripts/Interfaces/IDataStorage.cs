namespace Interfaces
{
    public interface IDataStorage
    {
        void SetInt(string key, int value);
        int GetInt(string key);
        void SetString(string key, string value);
        string GetString(string key);
        bool HasKey(string key);
        void DeleteKey(string key);
        void DeleteAll();
    }
}