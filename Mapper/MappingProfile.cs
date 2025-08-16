using AutoMapper;
using BPIBankSystem.API.DTOs;
using BPIBankSystem.API.DTOs.Requests;
using BPIBankSystem.API.DTOs.ResponseDtos;
using BPIBankSystem.API.Entities;

namespace BPIBankSystem.API.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TransferRequest, TransferRequestDto>().ReverseMap();

            CreateMap<Transaction, TransactionDto>().ReverseMap();

            CreateMap<Transaction, TransactionResponseDto>()
                .ForMember(dest => dest.FromAccountNumber, opt => opt.MapFrom(src => src.FromAccount.AccountNumber))
                .ForMember(dest => dest.ToAccountNumber, opt => opt.MapFrom(src => src.ToAccount.AccountNumber))
                .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
            
            CreateMap<Checks, CheckResponseDto>()
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account.AccountName));

            CreateMap<Address, AddressResponseDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src =>
                        src.User != null 
                        ? $"{src.User.LastName} {src.User.FirstName}"
                        : string.Empty))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User != null ? src.User.Email : string.Empty));

            CreateMap<CheckbookRequests, CheckbookRequestResponseDto>()
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src =>
                        src.User != null
                        ? $"{src.User.LastName} {src.User.FirstName}"
                        : string.Empty))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Account != null ? src.Account.AccountNumber : string.Empty))
                .ForMember(dest => dest.Leaves, opt => opt.MapFrom(src => src.CheckbookNumber));

            CreateMap<AddressChangeRequests, AddressChangeRequestResponseDto>()
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src =>
                        src.User != null
                        ? $"{src.User.LastName} {src.User.FirstName}"
                        : string.Empty))
                .ForMember(dest => dest.NewAddress, opt => opt.MapFrom(src =>
                    src.NewAddress != null
                        ? $"{src.NewAddress.AddressDetail}, {src.NewAddress.District ?? "N/A"}, {src.NewAddress.City}, {src.NewAddress.Country}"
                        : string.Empty));

            CreateMap<StopPaymentRequests, StopPaymentRequestResponseDto>()
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src =>
                        src.User != null
                        ? $"{src.User.LastName} {src.User.FirstName}"
                        : string.Empty))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Account != null ? src.Account.AccountNumber : string.Empty))
                .ForMember(dest => dest.ChequeNumber, opt => opt.MapFrom(src => src.CheckNumber))
                .ForMember(dest => dest.ChequeDate, opt => opt.MapFrom(src => src.ChequeDate)) 
                .ForMember(dest => dest.ReasonForStopPayment, opt => opt.MapFrom(src => src.Reason));

            CreateMap<SupportRequest, SupportRequestResponse>()
                .ForMember(dest => dest.Sender, opt => opt.MapFrom(src =>
                        src.User != null
                        ? $"{src.User.LastName} {src.User.FirstName}"
                        : string.Empty))
                .ForMember(dest => dest.SentDate, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ReplyDate, opt => opt.MapFrom(src => src.Answer != null ? src.UpdatedAt : (DateTime?)null));
        }
    }
}
