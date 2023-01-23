using ProtoBuf;

namespace Company.Manager.Sales.Interface
{
    [ProtoInclude(11, typeof(Online.FindCriteria))]
    [ProtoInclude(12, typeof(Restaurant.FindCriteria))]
    [ProtoContract]
    public abstract class FindCriteriaBase
    {
        [ProtoMember(1)]
        public string? Term { get; set; }
    }
}
