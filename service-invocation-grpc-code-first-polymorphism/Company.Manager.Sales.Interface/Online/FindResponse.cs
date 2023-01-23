using ProtoBuf;

namespace Company.Manager.Sales.Interface.Online
{
    [ProtoContract]
    public class FindResponse : FindResponseBase
    {
        [ProtoMember(1)]
        public string? OnlineField { get; set; }
    }
}
