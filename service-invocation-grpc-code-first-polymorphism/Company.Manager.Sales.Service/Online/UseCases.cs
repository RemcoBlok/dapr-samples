using Company.Manager.Sales.Interface;
using Company.Manager.Sales.Interface.Online;

namespace Company.Manager.Sales.Service.Online
{
    public class UseCases
    {
        public Task<IResponse<FindResponse>> FindItemAsync(FindCriteria criteria)
        {
            return Task.FromResult<IResponse<FindResponse>>(new Response<FindResponse>()
            {
                Value = new()
                {
                    Name = criteria.Term,
                    OnlineField = criteria.OnlineField
                }
            });
        }
    }
}
