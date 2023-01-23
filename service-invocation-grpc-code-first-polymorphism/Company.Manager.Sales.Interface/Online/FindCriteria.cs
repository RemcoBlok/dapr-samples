using ProtoBuf;

namespace Company.Manager.Sales.Interface.Online
{
    [ProtoContract]
    public class FindCriteria : FindCriteriaBase
    {
        [ProtoMember(1)]
        public string? OnlineField { get; set; }
    }
}
