using Company.Manager.Sales.Interface;
using ProtoBuf.Grpc;

namespace Company.Manager.Sales.Service
{
    public class SalesManager : ISalesManager
    {
        public async Task<FindResponseBase> FindItemAsync(FindCriteriaBase criteria, CallContext context = default)
        {
            FindResponseBase response = await UseCaseFactory<FindCriteriaBase, FindResponseBase>.CallAsync(criteria);
            return response;
        }
    }
}
