using Company.Manager.Sales.Interface;
using Company.Manager.Sales.Interface.Online;

namespace Company.Manager.Sales.Service.Online
{
    public class UseCases
    {
        public Task<IResponse<FindResponse>> FindItemAsync(FindCriteria criteria)
        {
            IResponse<FindResponse> response = new Response<FindResponse>()
            {
                Value = new()
                {
                    Name = criteria.Term,
                    OnlineField = criteria.OnlineField
                }
            };

            return Task.FromResult(response);
        }
    }
}
