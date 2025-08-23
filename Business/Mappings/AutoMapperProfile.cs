using AutoMapper;
using BabsKitapEvi.Entities.Models;
using BabsKitapEvi.Common.DTOs.BookDTOs;
using BabsKitapEvi.Common.DTOs.UserDTOs;
using BabsKitapEvi.Common.DTOs.AuthDTOs;
using BabsKitapEvi.Business.DTOs.MappingDTOs;
using BabsKitapEvi.Common.DTOs.CategoryDTOs;
using BabsKitapEvi.Common.DTOs.CartDTOs;
using BabsKitapEvi.Common.DTOs.PublisherDTOs;
using BabsKitapEvi.Common.DTOs.OrderDTOs;
using BabsKitapEvi.Common.DTOs.AddressDTOs;

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
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(options => options.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<CreateAndUpdateCategoryDto, Category>();
            CreateMap<Category, CreateAndUpdateCategoryDto>();

            CreateMap<Publisher, PublisherDto>().ReverseMap();
            CreateMap<Publisher, CreateAndUpdatePublisherDto>().ReverseMap();

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

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.ShippingCity, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.ShippingCountry, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.ShippingZipCode, opt => opt.MapFrom(src => src.ZipCode));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.BookAuthor, opt => opt.MapFrom(src => src.Book.Author))
                .ForMember(dest => dest.BookImageUrl, opt => opt.MapFrom(src => src.Book.ImageUrl))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));

            CreateMap<Address, AddressDto>();

            CreateMap<CreateAddressDto, Address>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<OrderDto, Address>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}