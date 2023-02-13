using Company.Manager.Sales.Interface;
using ProtoBuf.Grpc;

namespace Company.Manager.Sales.Service
{
    public class SalesManager : ISalesManager
    {
        public async Task<IResponse<FindResponseBase>> FindItemAsync(FindCriteriaBase criteria, CallContext context = default)
        {
            IResponse<FindResponseBase> response = await UseCaseFactory<FindCriteriaBase, IResponse<FindResponseBase>>.CallAsync(criteria);
            return response;
        }
    }
}
