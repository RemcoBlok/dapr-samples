using ProtoBuf;

namespace Company.Manager.Sales.Interface
{
    [ProtoInclude(11, typeof(Online.FindResponse))]
    [ProtoInclude(12, typeof(Restaurant.FindResponse))]
    [ProtoContract]
    public abstract class FindResponseBase
    {
        [ProtoMember(1)]
        public string? Name { get; set; }
    }
}
