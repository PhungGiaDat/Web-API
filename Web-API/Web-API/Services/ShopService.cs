using Web_API.Services.Interfaces;
using Web_API.Repositories.Interfaces;
using Web_API.Data;
using Web_API.Models;



namespace Web_API.Services
{
    public class ShopService : IShopService
    {

        private readonly IUnitOfWork _unitOfWork;

        public ShopService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<List<ShopResponse>> GetAllShopsAsync()
        {
            try
            {
                var shops = await _unitOfWork.Shops.GetAllAsync();
                
                // Return direct list of shop responses
                return shops.Select(s => new ShopResponse
                {
                    ShopId = s.ShopId,
                    ShopName = s.ShopName,
                    Address = s.Address,
                }).ToList();
            }
            catch (Exception)
            {
                // Return empty list in case of error
                return new List<ShopResponse>();
            }
        }

        public async Task<SimpleApiResponse> CreateShopAsync(CreateShopRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.ShopId) || string.IsNullOrEmpty(request.ShopName))
            {
                return new SimpleApiResponse
                {
                    Status = "Failed",
                    Message = "Dữ liệu cửa hàng không hợp lệ."
                };
            }

            try
            {
                // Check if shop already exists
                var existingShop = await _unitOfWork.Shops.GetByIdAsync(request.ShopId);
                if (existingShop != null)
                {
                    return new SimpleApiResponse
                    {
                        Status = "Failed",
                        Message = "Cửa hàng với mã này đã tồn tại."
                    };
                }

                var shop = new Shop
                {
                    ShopId = request.ShopId,
                    ShopName = request.ShopName,
                    Address = request.Address,
                    ApiKeyHash = Guid.NewGuid().ToString() // Generate a temporary API key hash
                };

                await _unitOfWork.Shops.CreateAsync(shop);
                await _unitOfWork.SaveChangesAsync();

                return new SimpleApiResponse
                {
                    Status = "SUCCESS",
                    Message = "Thêm cửa hàng thành công"
                };
            }
            catch (Exception ex)
            {
                return new SimpleApiResponse
                {
                    Status = "Failed",
                    Message = "Có lỗi xảy ra khi tạo cửa hàng"
                };
            }
        }

        public async Task<SimpleApiResponse> UpdateShopAsync(string shopId, UpdateShopRequest request)
        {
            if (string.IsNullOrEmpty(shopId) || request == null || string.IsNullOrEmpty(request.ShopName))
            {
                return new SimpleApiResponse
                {
                    Status = "Failed",
                    Message = "Dữ liệu cửa hàng không hợp lệ."
                };
            }

            try
            {
                var shop = await _unitOfWork.Shops.GetByIdAsync(shopId);
                if (shop == null)
                {
                    return new SimpleApiResponse
                    {
                        Status = "Failed",
                        Message = "Không tìm thấy cửa hàng."
                    };
                }

                shop.ShopName = request.ShopName;
                shop.Address = request.Address;

                await _unitOfWork.Shops.UpdateAsync(shop);
                await _unitOfWork.SaveChangesAsync();

                return new SimpleApiResponse
                {
                    Status = "SUCCESS",
                    Message = "Cập nhật cửa hàng thành công"
                };
            }
            catch (Exception ex)
            {
                return new SimpleApiResponse
                {
                    Status = "Failed",
                    Message = "Có lỗi xảy ra khi cập nhật cửa hàng"
                };
            }
        }

        public async Task<SimpleApiResponse> DeleteShopAsync(string shopId)
        {
            if (string.IsNullOrEmpty(shopId))
            {
                return new SimpleApiResponse
                {
                    Status = "Failed",
                    Message = "Mã cửa hàng là bắt buộc."
                };
            }

            try
            {
                var shop = await _unitOfWork.Shops.GetByIdAsync(shopId);
                if (shop == null)
                {
                    return new SimpleApiResponse
                    {
                        Status = "Failed",
                        Message = "Không tìm thấy cửa hàng."
                    };
                }

                await _unitOfWork.Shops.DeleteAsync(shopId);
                await _unitOfWork.SaveChangesAsync();

                return new SimpleApiResponse
                {
                    Status = "SUCCESS",
                    Message = "Xóa cửa hàng thành công"
                };
            }
            catch (Exception ex)
            {
                return new SimpleApiResponse
                {
                    Status = "Failed",
                    Message = "Có lỗi xảy ra khi xóa cửa hàng"
                };
            }
        }

        public async Task<bool> ValidateShopApiKeyAsync(string shopId, string apiKey)
        {
            try
            {
                return await _unitOfWork.Shops.ValidateApiKeyAsync(shopId, apiKey);
            }
            catch
            {
                return false;
            }
        }
    }
}
