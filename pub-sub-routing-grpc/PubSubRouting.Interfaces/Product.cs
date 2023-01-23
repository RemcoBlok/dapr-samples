using System.Text.Json.Serialization;

namespace PubSubRouting.Interfaces
{
    [JsonDerivedType(typeof(Widget), nameof(Widget))]
    [JsonDerivedType(typeof(Gadget), nameof(Gadget))]
    [JsonDerivedType(typeof(Thingamajig), nameof(Thingamajig))]
    public abstract class Product
    {
        public string? Description { get; set; }

        public decimal? Price { get; set; } 
    }
}
