using AutoMapper;
using BPIBankSystem.API.Data;
using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;
using BPIBankSystem.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPIBankSystem.API.Services.Impl
{
    public class AddressChangeRequestService : IAddressChangeRequestService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AddressChangeRequestService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<AddressChangeRequestResponseDto>> GetAllAsync()
        {
            var requests = await _context.AddressChangeRequests
                .Include(r => r.User) 
                .Include(r => r.NewAddress) 
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();

            return _mapper.Map<List<AddressChangeRequestResponseDto>>(requests);
        }


        public async Task<List<AddressChangeRequestDto>> GetRequestsByUserIdAsync(int userId)
        {
            return await _context.AddressChangeRequests
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RequestDate)
                .Select(r => new AddressChangeRequestDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    NewAddressId = r.NewAddressId,
                    RequestDate = r.RequestDate,
                    Status = r.Status,
                    Notes = r.Notes
                })
                .ToListAsync();
        }


        public async Task<AddressChangeRequestDto> GetAddressChangeRequestAsync(int id)
        {
            var request = await _context.AddressChangeRequests
                .FirstOrDefaultAsync(r => r.Id == id);
            if (request == null)
                return null;

            return new AddressChangeRequestDto
            {
                Id = request.Id,
                UserId = request.UserId,
                NewAddressId = request.NewAddressId,
                RequestDate = request.RequestDate,
                Status = request.Status,
                Notes = request.Notes
            };
        }

        public async Task<AddressChangeRequestDto> CreateAddressChangeRequestAsync(CreateAddressChangeRequestDto dto)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == dto.UserId);
            var addressExists = await _context.Addresses.AnyAsync(a => a.Id == dto.NewAddressId && a.UserId == dto.UserId);
            Console.WriteLine($"UserId {dto.UserId} exists: {userExists}, AddressId {dto.NewAddressId} exists for user: {addressExists}");

            if (!userExists || !addressExists)
            {
                return null;
            }

            var request = new AddressChangeRequests
            {
                UserId = dto.UserId,
                NewAddressId = dto.NewAddressId,
                Notes = dto.Notes,
                RequestDate = DateTime.UtcNow,
                Status = "pending"
            };

            _context.AddressChangeRequests.Add(request);
            await _context.SaveChangesAsync();

            return new AddressChangeRequestDto
            {
                Id = request.Id,
                UserId = request.UserId,
                NewAddressId = request.NewAddressId,
                RequestDate = request.RequestDate,
                Status = request.Status,
                Notes = request.Notes
            };
        }

        public async Task<bool> UpdateAddressChangeRequestStatusAsync(int id, string status)
        {
            var request = await _context.AddressChangeRequests.FindAsync(id);
            if (request == null)
                return false;

            // Kiểm tra trạng thái hợp lệ
            var allowedStatuses = new[] { "pending", "approved", "rejected" };
            if (!allowedStatuses.Contains(status))
                return false;

            request.Status = status;

            if (status == "approved")
            {
                var newAddress = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.Id == request.NewAddressId && a.UserId == request.UserId);
                if (newAddress == null)
                    return false;

                var currentAddress = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.UserId == request.UserId && a.IsCurrent);
                if (currentAddress != null)
                {
                    currentAddress.IsCurrent = false;
                }

                newAddress.IsCurrent = true;
                newAddress.UpdatedAt = DateTime.UtcNow;
            }

            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAddressChangeRequestAsync(int id)
        {
            var request = await _context.AddressChangeRequests.FindAsync(id);
            if (request == null)
                return false;

            _context.AddressChangeRequests.Remove(request);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}


