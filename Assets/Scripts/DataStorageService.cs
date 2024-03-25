using Interfaces;
using Zenject;

public class DataStorageService
{
    private readonly IDataStorage _dataStorage;

    [Inject]
    public DataStorageService(IDataStorage dataStorage)
    {
        _dataStorage = dataStorage;
    }
    
    public void SetInt(string key, int value)
    {
        _dataStorage.SetInt(key, value);
    }

    public int GetInt(string key)
    {
        return _dataStorage.GetInt(key);
    }

    public void SetString(string key, string value)
    {
        _dataStorage.SetString(key, value);
    }

    public string GetString(string key)
    {
        return _dataStorage.GetString(key);
    }

    public bool HasKey(string key)
    {
        return _dataStorage.HasKey(key);
    }

    public void DeleteKey(string key)
    {
        _dataStorage.DeleteKey(key);
    }

    public void DeleteAll()
    {
        _dataStorage.DeleteAll();
    }
}
