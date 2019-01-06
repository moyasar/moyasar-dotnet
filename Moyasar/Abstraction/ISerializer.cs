namespace Moyasar.Abstraction
{
    public interface ISerializer
    {
        string Serialize(object obj);
        TType Deserialize<TType>(string json) where TType : class, new();
        void PopulateObject(string json, object obj);
    }
}