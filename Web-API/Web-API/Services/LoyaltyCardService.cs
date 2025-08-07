using System.Globalization;
using Web_API.Data;
using Web_API.Models;
using Web_API.Repositories.Interfaces;
using Web_API.Services.Interfaces;

namespace Web_API.Services
{
    public class LoyaltyCardService : ILoyaltyCardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoyaltyCardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse> RegisterLoyaltyCardAsync(RegisterLoyaltyCardRequest request)
        {
            try
            {
                // Validate initial points (>0)
                if (request.InitialPoints <= 0)
                {
                    return new ApiResponse
                    {
                        Status = "FAIL",
                        Message = "Số thẻ đã tồn tại hoặc căn cước hết hạn hoặc điểm khởi tạo không hợp lệ"
                    };
                }

                // Check if card number already exists
                var existingCard = await _unitOfWork.LoyaltyCards.GetByCardNumberAsync(request.CardNumber);
                if (existingCard != null)
                {
                    return new ApiResponse
                    {
                        Status = "FAIL",
                        Message = "Số thẻ đã tồn tại hoặc căn cước hết hạn hoặc điểm khởi tạo không hợp lệ"
                    };
                }

                // Check if citizen ID already exists
                var existingCustomer = await _unitOfWork.Customers.GetByCitizenIdAsync(request.CitizenId);
                if (existingCustomer != null)
                {
                    return new ApiResponse
                    {
                        Status = "FAIL",
                        Message = "Số thẻ đã tồn tại hoặc căn cước hết hạn hoặc điểm khởi tạo không hợp lệ"
                    };
                }

                // Parse and validate expiry date
                if (!DateOnly.TryParseExact(request.ExpiryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly expiryDate))
                {
                    return new ApiResponse
                    {
                        Status = "FAIL",
                        Message = "Số thẻ đã tồn tại hoặc căn cước hết hạn hoặc điểm khởi tạo không hợp lệ"
                    };
                }

                // Check if citizen ID is expired
                if (expiryDate <= DateOnly.FromDateTime(DateTime.Now))
                {
                    return new ApiResponse
                    {
                        Status = "FAIL",
                        Message = "Số thẻ đã tồn tại hoặc căn cước hết hạn hoặc điểm khởi tạo không hợp lệ"
                    };
                }

                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    // Create new customer
                    var customer = new Customer
                    {
                        CustomerId = Guid.NewGuid(),
                        FullName = request.CustomerName,
                        PhoneNumber = request.Phone,
                        CitizenId = request.CitizenId,
                        CitizenIdExpiry = expiryDate
                    };

                    // Create new loyalty card
                    var loyaltyCard = new LoyaltyCard
                    {
                        CardNumber = request.CardNumber,
                        CustomerId = customer.CustomerId,
                        AvailablePoints = request.InitialPoints,
                        Status = "Active"
                    };

                    // Save using repositories
                    await _unitOfWork.Customers.CreateAsync(customer);
                    await _unitOfWork.LoyaltyCards.CreateAsync(loyaltyCard);

                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransactionAsync();

                    return new ApiResponse
                    {
                        Status = "SUCCESS",
                        CardNumber = request.CardNumber,
                        Message = "Đăng ký thành công"
                    };
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Status = "FAIL",
                    Message = "Có lỗi xảy ra trong quá trình "
                };
            }
        }

        public async Task<ApiResponse> GetByCardNumberAsync(string cardNumber)
        {
            try
            {
                var card = await _unitOfWork.LoyaltyCards.GetByCardNumberAsync(cardNumber);
                
                // kiểm tra xem thẻ có tồn tại không
                if (card == null)
                {
                    return new ApiResponse
                    {
                        Status = "FAIL",
                        Message = "Thẻ không tồn tại"
                    };
                }

                // trả về thông tin thẻ với cấu trúc đầy đủ
                return new ApiResponse
                {
                    Status = "SUCCESS",
                    Message = "Thông tin thẻ thành công",
                    CardNumber = card.CardNumber,
                    CustomerName = card.Customer.FullName,
                    Phone = card.Customer.PhoneNumber,
                    CitizenId = card.Customer.CitizenId,
                    ExpiryDate = card.Customer.CitizenIdExpiry.ToString("yyyy-MM-dd"),
                    AvailablePoints = card.AvailablePoints
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Status = "FAIL",
                    Message = "Có lỗi xảy ra trong quá trình xử lý"
                };
            }
        }

        public async Task<SimpleApiResponse> UpdateCardInfoAsync(string cardNumber, UpdateCustomerInfoRequest request)
        {
            try
            {
                // Parse and validate expiry date
                if (!DateOnly.TryParseExact(request.ExpiryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly expiryDate))
                {
                    return new SimpleApiResponse
                    {
                        Status = "FAIL",
                        Message = "Định dạng ngày hết hạn không hợp lệ. Sử dụng định dạng yyyy-MM-dd"
                    };
                }

                // Check if citizen ID is expired
                if (expiryDate <= DateOnly.FromDateTime(DateTime.Now))
                {
                    return new SimpleApiResponse
                    {
                        Status = "FAIL",
                        Message = "Ngày hết hạn căn cước không được trong quá khứ"
                    };
                }

                var card = await _unitOfWork.LoyaltyCards.GetByCardNumberAsync(cardNumber);

                if (card == null)
                {
                    return new SimpleApiResponse
                    {
                        Status = "FAIL",
                        Message = "Thẻ không tồn tại"
                    };
                }

                // Check if new citizen ID already exists for another customer
                if (request.CitizenId != card.Customer.CitizenId)
                {
                    var existingCustomer = await _unitOfWork.Customers.GetByCitizenIdAsync(request.CitizenId);
                    if (existingCustomer != null)
                    {
                        return new SimpleApiResponse
                        {
                            Status = "FAIL",
                            Message = "Số căn cước đã được sử dụng bởi khách hàng khác"
                        };
                    }
                }

                // Check if new phone number already exists for another customer
                if (request.Phone != card.Customer.PhoneNumber)
                {
                    var existingCustomer = await _unitOfWork.Customers.GetByPhoneNumberAsync(request.Phone);
                    if (existingCustomer != null)
                    {
                        return new SimpleApiResponse
                        {
                            Status = "FAIL",
                            Message = "Số điện thoại đã được sử dụng bởi khách hàng khác"
                        };
                    }
                }

                // Update customer information
                card.Customer.FullName = request.CustomerName;
                card.Customer.PhoneNumber = request.Phone;
                card.Customer.CitizenId = request.CitizenId;
                card.Customer.CitizenIdExpiry = expiryDate;

                await _unitOfWork.Customers.UpdateAsync(card.Customer);
                await _unitOfWork.SaveChangesAsync();

                return new SimpleApiResponse
                {
                    Status = "SUCCESS",
                    Message = "Cập nhật thông tin thành công"
                };
            }
            catch (Exception ex)
            {
                return new SimpleApiResponse
                {
                    Status = "FAIL",
                    Message = "Có lỗi xảy ra trong quá trình xử lý"
                };
            }
        }

        public async Task<SimpleApiResponse> DeleteCardAsync(string cardNumber)
        {
            try
            {
                var card = await _unitOfWork.LoyaltyCards.GetByCardNumberAsync(cardNumber);

                if (card == null)
                {
                    return new SimpleApiResponse
                    {
                        Status = "FAIL",
                        Message = "Thẻ không tồn tại"
                    };
                }

                await _unitOfWork.LoyaltyCards.DeleteAsync(cardNumber);
                await _unitOfWork.SaveChangesAsync();

                return new SimpleApiResponse
                {
                    Status = "SUCCESS",
                    Message = "Xóa thẻ thành công"
                };
            }
            catch (Exception ex)
            {
                return new SimpleApiResponse
                {
                    Status = "FAIL",
                    Message = "Có lỗi xảy ra trong quá trình xử lý"
                };
            }
        }
    }
}