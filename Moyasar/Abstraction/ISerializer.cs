namespace Moyasar.Abstraction
{
    /// <summary>
    /// Interface for serializing and deserializing JSON strings, used to decouple the library from any specific implementation
    /// </summary>
    public interface ISerializer
    {
        string Serialize(object obj);
        TType Deserialize<TType>(string json) where TType : class, new();
        void PopulateObject(string json, object obj);
    }
}