using AutoMapper;
using Discount.gRPC.Entities;
using Discount.gRPC.Protos;
using Discount.gRPC.Repository.Interfaces;
using Grpc.Core;
using System.Linq.Expressions;

namespace Discount.gRPC.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DiscountService> _logger;

    public DiscountService(
        IDiscountRepository discountRepository, 
        IMapper mapper, 
        ILogger<DiscountService> logger)
    {
        _discountRepository = discountRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public override async Task<CouponModel> GetDiscount(
        GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _discountRepository.GetDiscount(request.ProductName) ?? 
            throw new RpcException(new Status(
                StatusCode.NotFound, 
                $"Discount with ProductName = {request.ProductName} not found"));

        _logger.LogInformation("Discount retrieved for ProductName: {productName}, " +
            "Amout: {amount}", coupon.ProductName, coupon.Amount);

        var couponModel = _mapper.Map<CouponModel>(coupon);
        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(
        CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);
        await _discountRepository.CreateDiscount(coupon);

        var couponModel = _mapper.Map<CouponModel>(coupon);
        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(
        UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);
        await _discountRepository.UpdateDiscount(coupon);

        var couponModel = _mapper.Map<CouponModel>(coupon);
        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(
        DeleteDiscountRequest request, ServerCallContext context)
    {
        var deleted = await _discountRepository.DeleteDiscount(request.ProductName);
        var response = new DeleteDiscountResponse
        {
            Success = deleted,
        };
        return response;
    }
}
