using ProtoBuf;

namespace Company.Manager.Sales.Interface.Restaurant
{
    [ProtoContract]
    public class FindResponse : FindResponseBase
    {
        [ProtoMember(1)]
        public string? RestaurantField { get; set; }
    }
}
