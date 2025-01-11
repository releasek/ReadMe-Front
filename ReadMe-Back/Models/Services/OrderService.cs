using ReadMe_Back.Models.EFModels;
using ReadMe_Back.Models.Repositories;
using ReadMe_Back.Models.ViewModels;

namespace ReadMe_Back.Models.Services
{
    public class OrderService
    {
        private readonly OrderEFRepo _orderEFRepo;

        public OrderService(OrderEFRepo orderEFRepo)
        {
            _orderEFRepo = orderEFRepo;
        }

        public IQueryable<OrderVm> GetOrder(OrderQueryParameters parameters)
        {
            return _orderEFRepo.GetOrder(parameters);
        }
    }
}
