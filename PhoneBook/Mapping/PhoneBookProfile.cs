using AutoMapper;
using PhoneBook.Bll.Models;
using PhoneBook.Dal.Models;

namespace PhoneBook.Mapping;

public class PhoneBookProfile : Profile
{
    public PhoneBookProfile()
    {
        CreateMap<GroupDb, GroupDto>(MemberList.Destination);
    }
}