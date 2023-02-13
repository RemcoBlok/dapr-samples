using Company.Manager.Sales.Interface;
using Company.Manager.Sales.Interface.Restaurant;

namespace Company.Manager.Sales.Service.Restaurant
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
                    RestaurantField = criteria.RestaurantField
                }
            });
        }
    }
}
