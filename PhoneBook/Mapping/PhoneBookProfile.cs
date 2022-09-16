using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using PhoneBook.Bll.Models;
using PhoneBook.Dal.Models;
using Shared.Extensions;

namespace PhoneBook.Mapping;

public class PhoneBookProfile : Profile
{
    public PhoneBookProfile()
    {
        CreateMap<GroupDb, GroupDto>(MemberList.Destination);

        CreateMap<UserDb, UserData>(MemberList.Destination);
        CreateMap<AddressDb, AddressDto>(MemberList.Destination);
        CreateMap<PhoneDataDb, PhoneData>(MemberList.Destination);
        CreateMap<PhoneCategoryDb, PhoneCategoryDto>(MemberList.Destination);

        CreateMap<UserDb, UserShortDataDto>(MemberList.Destination)
            .ForMember(x => x.FullName, expression => expression.MapFrom(x => $"{x.LastName} {x.FirstName} {x.MiddleName}"))
            .ForMember(x => x.UserId, expression => expression.MapFrom(x => x.Id));

        CreateMap<SaveUserRequest, UserDb>(MemberList.Source)
            .ForMember(x => x.Groups, expression => expression.MapFrom(x => x.GroupIds.Select(groupId => new GroupDb {Id = groupId}).ToList()))
            .ForMember(x => x.Phones, expression => expression.MapFrom(x => x.PhoneNumbers))
            .ForSourceMember(x => x.GroupIds, expression => expression.DoNotValidate());
        CreateMap<AddressDto, AddressDb>(MemberList.Source);
        CreateMap<SavePhoneNumberRequest, PhoneDataDb>(MemberList.Source)
            .ForMember(x => x.CategoryId, expression => expression.MapFrom(x => x.PhoneCategoryId));

        CreateMap<UserDb, ExportUserDataDto>(MemberList.Destination)
            .ForMember(x => x.PhoneNumbers, expression => expression.MapFrom(x => string.Join(", ", x.Phones.Select(p => p.PhoneNumber))))
            .ForMember(x => x.Address,
                expression => expression.MapFrom(x => x.Address != null
                        ? string.Join(", ", new {x.Address.Region, x.Address.City, x.Address.Street, x.Address.House, x.Address.Block, x.Address.Flat})
                        : string.Empty))
            .ForMember(x => x.Groups, expression => expression.MapFrom(x => string.Join(", ", x.Groups.Select(p => p.Name))))
            .ForMember(x => x.Gender, expression => expression.MapFrom(x => x.Gender.GetAttribute<DisplayAttribute>()!.Name));
    }
}