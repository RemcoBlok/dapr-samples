using Company.Manager.Sales.Interface.Restaurant;

namespace Company.Manager.Sales.Service.Restaurant
{
    public class UseCases
    {
        public Task<FindResponse> FindItemAsync(FindCriteria criteria)
        {
            return Task.FromResult(new FindResponse
            {
                Name = criteria.Term,
                RestaurantField = criteria.RestaurantField
            });
        }
    }
}
