using AutoMapper;
using BabsKitapEvi.Entities.Models;
using BabsKitapEvi.Entities.DTOs.BookDTOs;
using BabsKitapEvi.Entities.DTOs.UserDTOs;
using BabsKitapEvi.Entities.DTOs.AuthDTOs;
using BabsKitapEvi.Entities.DTOs.CategoryDTOs;
using BabsKitapEvi.Entities.DTOs.CartDTOs;

namespace BabsKitapEvi.Business.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserForRegisterDto, AppUser>();
            CreateMap<AppUser, UserDto>();
            CreateMap<AppUser, UserResponseDto>();
            CreateMap<UserForUpdateDto, AppUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(options => options.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UserResponseDto, UserForUpdateDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<UserResponseDto, UserForChangePasswordDto>()
                .ForMember(dest => dest.OldPassword, opt => opt.Ignore())
                .ForMember(dest => dest.NewPassword, opt => opt.Ignore());

            CreateMap<LoginInfoForMappingDto, AuthResponseDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserResponseDto
                {
                    Id = src.User.Id,
                    Email = src.User.Email ?? string.Empty,
                    FirstName = src.User.FirstName,
                    LastName = src.User.LastName,
                    Role = src.Role,
                    CreatedAt = src.User.CreatedAt
                }))
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.Token))
                .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken))
                .ForMember(dest => dest.ExpiresIn, opt => opt.MapFrom(src => (long)(src.ExpiryTime - DateTime.UtcNow).TotalSeconds));

            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.BookCategories!.Select(bc => bc.Category)));
            CreateMap<CreateBookDto, Book>()
                .ForMember(dest => dest.BookCategories, opt => opt.Ignore());

            CreateMap<UpdateBookDto, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id alanını eşleme
                .ForAllMembers(options => options.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();

            // Cart mappings
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Book.Price));

            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Items.Sum(item => item.Quantity * item.Book.Price)));

            // DTO -> Entity for write operations
            CreateMap<AddCartItemDto, CartItem>();
            CreateMap<UpdateCartItemDto, CartItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CartId, opt => opt.Ignore())
                .ForMember(dest => dest.Cart, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.Ignore());
        }
    }
}