using ProtoBuf;

namespace Company.Manager.Sales.Interface
{
    [ProtoInclude(11, typeof(Response<Online.FindResponse>))]
    [ProtoInclude(12, typeof(Response<Restaurant.FindResponse>))]
    [ProtoContract]
    public interface IResponse<out T> where T : notnull
    {
        [ProtoMember(1)]
        T? Value { get; }

        [ProtoMember(2)]
        ErrorInfo? Error { get; }
    }
}
