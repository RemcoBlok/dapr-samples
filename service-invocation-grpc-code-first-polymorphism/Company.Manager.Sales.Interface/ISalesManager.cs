﻿using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace Company.Manager.Sales.Interface
{
    [Service]
    public interface ISalesManager
    {
        [Operation]
        Task<FindResponseBase> FindItemAsync(FindCriteriaBase criteria, CallContext context = default);
    }
}