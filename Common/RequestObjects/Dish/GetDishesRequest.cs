using Common.Enums;
using Common.RequestObjects.Pagination;
using Common.Status;

namespace Common.RequestObjects.Dish;

public class GetDishesRequest : PaginationRequest
{
    public ModelStatus? Status { get; set; }
    public string? Name { get; set; }
    public decimal? MaxPrice { get; set; }
    public decimal? MinPrice { get; set; }
    public Meal? Meal { get; set; }
}