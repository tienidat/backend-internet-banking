using AutoMapper;
using BPIBankSystem.API.Data;
using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;
using BPIBankSystem.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPIBankSystem.API.Services.Impl
{
    public class AddressService : IAddressService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AddressService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AddressResponseDto>> GetAllAsync()
        {
            var addresses = await _context.Addresses
                .Include(a => a.User)
                .OrderByDescending(a => a.UpdatedAt)
                .ToListAsync();

            return _mapper.Map<List<AddressResponseDto>>(addresses);
        }


        public async Task<List<AddressDto>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.UpdatedAt)
                .Select(a => new AddressDto
                {
                    Id = a.Id,
                    AddressDetail = a.AddressDetail,
                    District = a.District,
                    City = a.City,
                    Country = a.Country,
                    IsCurrent = a.IsCurrent,
                    UpdatedAt = a.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<AddressDto> GetCurrentAddressAsync(int userId)
        {
            var current = await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId && a.IsCurrent);

            if (current == null) return null;

            return new AddressDto
            {
                Id = current.Id,
                AddressDetail = current.AddressDetail,
                District = current.District,
                City = current.City,
                Country = current.Country,
                IsCurrent = current.IsCurrent,
                UpdatedAt = current.UpdatedAt
            };
        }

        public async Task<bool> AddAddressAsync(AddressCreateDto dto)
        {
            var address = new Address
            {
                UserId = dto.UserId,
                AddressDetail = dto.AddressDetail,
                District = dto.District,
                City = dto.City,
                Country = dto.Country,
                IsCurrent = false,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAddressAsync(int addressId, AddressUpdateDto dto)
        {
            var address = await _context.Addresses.FindAsync(addressId);
            if (address == null) return false;

            address.AddressDetail = dto.AddressDetail;
            address.District = dto.District;
            address.City = dto.City;
            address.Country = dto.Country;
            address.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAddressAsOldAsync(int addressId)
        {
            var address = await _context.Addresses.FindAsync(addressId);
            if (address == null) return false;

            address.IsCurrent = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
