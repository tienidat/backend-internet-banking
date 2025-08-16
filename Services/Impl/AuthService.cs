using BPIBankSystem.API.Data;
using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;
using BPIBankSystem.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BPIBankSystem.API.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthService(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (existingUser != null)
            {
                return "Username already exists.";
            }

            var passwordHasher = new PasswordHasher<User>();

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Phone = dto.Phone,
                IsLocked = false,
                Role = string.IsNullOrEmpty(dto.Role) ? "User" : dto.Role
            };

            user.PasswordHash = passwordHasher.HashPassword(user, dto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var address = new Address
            {
                UserId = user.Id,
                AddressDetail = dto.AddressDetail ?? string.Empty,
                District = dto.District,
                City = dto.City ?? string.Empty,
                Country = dto.Country ?? "Vietnam",
                IsCurrent = true,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return "User registered successfully.";
        }

        public async Task<(UserResponseDto user, string token, string message)> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null)
            {
                await SaveLoginAttempt(dto.Username, false);
                return (null, null, "Account does not exist.");
            }

            if (user.IsLocked)
            {
                return (null, null, "Your account has been locked due to 3 failed login attempts.");
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result != PasswordVerificationResult.Success)
            {
                await SaveLoginAttempt(user.Username, false);

                var last3Attempts = await _context.UserLoginAttempts
                    .Where(x => x.Username == user.Username)
                    .OrderByDescending(x => x.AttemptTime)
                    .Take(3)
                    .ToListAsync();

                if (last3Attempts.Count == 3 && last3Attempts.All(x => !x.IsSuccessful))
                {
                    user.IsLocked = true;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    return (null, null, "You have entered the wrong password more than 3 times. Your account has been locked..");
                }

                return (null, null, "Password is incorrect.");
            }

            await SaveLoginAttempt(user.Username, true);

            var token = _jwtService.GenerateToken(user.Username);

            var userDto = new UserResponseDto
            {
                Id = user.Id.ToString(),
                FullName = $"{user.LastName} {user.FirstName}",
                Email = user.Email,
                Phone = user.Phone,
                Address = string.Empty,
                Role = user.Role
            };

            var address = await _context.Addresses
                .Where(a => a.UserId == user.Id && a.IsCurrent)
                .FirstOrDefaultAsync();

            if (address != null)
            {
                userDto.Address = $"{address.AddressDetail}, {address.District ?? ""}, {address.City}, {address.Country}";
            }

            return (userDto, token, "Log in successfully.");
        }


        public async Task<List<UserResponseDto>> GetAllAsync()
        {
            var users = await _context.Users
                .GroupJoin(_context.Addresses,
                    user => user.Id,
                    address => address.UserId,
                    (user, addresses) => new { user, addresses })
                .SelectMany(
                    x => x.addresses.Where(a => a.IsCurrent).DefaultIfEmpty(),
                    (x, address) => new UserResponseDto
                    {
                        Id = x.user.Id.ToString(),
                        FullName = $"{x.user.LastName} {x.user.FirstName}",
                        Email = x.user.Email,
                        Phone = x.user.Phone,
                        Address = address != null
                            ? $"{address.AddressDetail}, {address.District ?? ""}, {address.City}, {address.Country}"
                            : string.Empty,
                        Role = x.user.Role,
                        IsLocked = x.user.IsLocked
                    })
                .ToListAsync();

            return users;
        }

        public async Task<string> UpdateAsync(string userId, UpdateUserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null)
            {
                return "Người dùng không tồn tại.";
            }

            user.FirstName = dto.FirstName ?? user.FirstName;
            user.LastName = dto.LastName ?? user.LastName;
            user.Email = dto.Email ?? user.Email;
            user.Phone = dto.Phone ?? user.Phone;
            user.Role = dto.Role ?? user.Role;
            user.IsLocked = dto.IsLocked ?? user.IsLocked;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                var passwordHasher = new PasswordHasher<User>();
                user.PasswordHash = passwordHasher.HashPassword(user, dto.Password);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            if (dto.AddressDetail != null || dto.District != null || dto.City != null || dto.Country != null)
            {
                var oldAddresses = await _context.Addresses
                    .Where(a => a.UserId == user.Id && a.IsCurrent)
                    .ToListAsync();
                foreach (var oldAddress in oldAddresses)
                {
                    oldAddress.IsCurrent = false;
                    _context.Addresses.Update(oldAddress);
                }

                var newAddress = new Address
                {
                    UserId = user.Id,
                    AddressDetail = dto.AddressDetail ?? string.Empty,
                    District = dto.District,
                    City = dto.City ?? string.Empty,
                    Country = dto.Country ?? "Vietnam",
                    IsCurrent = true,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Addresses.Add(newAddress);
                await _context.SaveChangesAsync();

            }

            return "Cập nhật thông tin người dùng thành công.";
        }

        public async Task<string> DeleteAsync(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null)
            {
                return "Người dùng không tồn tại.";
            }

            var addresses = await _context.Addresses.Where(a => a.UserId.ToString() == userId).ToListAsync();
            if (addresses.Any())
            {
                _context.Addresses.RemoveRange(addresses);
            }

            var addressChangeRequests = await _context.AddressChangeRequests.Where(a => a.UserId.ToString() == userId).ToListAsync();
            if (addressChangeRequests.Any())
            {
                _context.AddressChangeRequests.RemoveRange(addressChangeRequests);
            }

            var loginAttempts = await _context.UserLoginAttempts.Where(a => a.Username == user.Username).ToListAsync();
            if (loginAttempts.Any())
            {
                _context.UserLoginAttempts.RemoveRange(loginAttempts);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return "Xóa người dùng thành công.";
        }

        private async Task SaveLoginAttempt(string username, bool isSuccessful)
        {
            var attempt = new UserLoginAttempt
            {
                Username = username,
                IsSuccessful = isSuccessful,
                AttemptTime = DateTime.UtcNow
            };

            _context.UserLoginAttempts.Add(attempt);
            await _context.SaveChangesAsync();
        }
    }
}