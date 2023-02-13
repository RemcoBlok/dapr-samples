using ProtoBuf;

namespace Company.Manager.Sales.Interface
{
    [ProtoContract]
    public class Response<T> : IResponse<T> where T : notnull
    {
        [ProtoMember(1)]
        public T? Value { get; set; }
        
        [ProtoMember(2)]
        public ErrorInfo? Error { get; set; }
    }
}
