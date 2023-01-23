using ProtoBuf;

namespace Company.Manager.Sales.Interface.Restaurant
{
    [ProtoContract]
    public class FindCriteria : FindCriteriaBase
    {
        [ProtoMember(1)]
        public string? RestaurantField { get; set; }
    }
}
