using Company.Manager.Sales.Interface.Online;

namespace Company.Manager.Sales.Service.Online
{
    public class UseCases
    {        
        public Task<FindResponse> FindItemAsync(FindCriteria criteria)
        {
            return Task.FromResult(new FindResponse
            {
                Name = criteria.Term,
                OnlineField = criteria.OnlineField
            });
        }
    }
}
