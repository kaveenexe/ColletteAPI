using System.Text.Json.Serialization;

namespace ColletteAPI.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProductCategory
    {
        Shirts,
        TShirts,
        Trousers,
        Shorts,
        Pants
    }
}