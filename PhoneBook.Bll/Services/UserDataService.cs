using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Bll.Models;
using PhoneBook.Dal;
using PhoneBook.Dal.Models;
using Shared.Exceptions;

namespace PhoneBook.Bll.Services;

public class UserDataService : IUserDataService
{
    private readonly PhoneBookDbContext _dbContext;
    private readonly IMapper _mapper;

    public UserDataService(PhoneBookDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public Task<UserShortDataDto> GetUsers(FilterRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<UserData> GetUserData(Guid userId, CancellationToken cancellationToken)
    {
        var userDb = await _dbContext.Users
            .Include(x => x.Address)
            .Include(x => x.Groups)
            .Include(x => x.Phones).ThenInclude(x => x.Category)
            .SingleOrDefaultAsync(x => x.Id == userId && !x.DeletedUtc.HasValue, cancellationToken);

        if (userDb == null) throw new EntityNotFoundException<UserDb>(userId.ToString());

        return _mapper.Map<UserData>(userDb);
    }

    public Task<UserData> Save(SaveUserRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(Guid userId, CancellationToken cancellationToken)
    {
        var userDb = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId && !x.DeletedUtc.HasValue, cancellationToken);

        if (userDb == null) throw new EntityNotFoundException<UserDb>(userId.ToString());

        userDb.DeletedUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}