using AutoMapper;
using PhoneBook.Bll.Models;
using PhoneBook.Dal.Models;

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
    }
}