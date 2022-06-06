using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcServiceClient
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;
        public DiscountGrpcServiceClient(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        {
            _discountProtoService = discountProtoServiceClient ?? throw new ArgumentNullException(nameof(discountProtoServiceClient));
        }
        
        public async Task<CouponModel> GetDiscount(string productName)
        {
            return await _discountProtoService.GetDiscountAsync(new GetDiscountRequest { ProductName = productName });
        }
    }
}
