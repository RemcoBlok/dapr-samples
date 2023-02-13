using ProtoBuf;

namespace Company.Manager.Sales.Interface
{
    [ProtoContract]
    public class ErrorInfo
    {
        [ProtoMember(1)]
        public int Code { get; set; }

        [ProtoMember(2)]
        public required string Description { get; set; }
    }
}
