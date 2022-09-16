﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Bll.Enums;
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

    public async Task<UserShortDataDto[]> GetUsers(FilterRequest filter, CancellationToken cancellationToken)
    {
        var query = _dbContext.Users.Where(x => !x.DeletedUtc.HasValue);

        if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
        {
            var pattern = $"%{filter.SearchPhrase}%";
            switch (filter.SearchPlaceType)
            {
                case SearchPlaceType.None:
                case SearchPlaceType.LastName:
                    query = query.Where(x => EF.Functions.ILike(x.LastName, pattern));
                    break;
                case SearchPlaceType.PhoneNumber:
                    query = query.Where(x => x.Phones.Any(p=>EF.Functions.ILike(p.PhoneNumber, pattern)));
                    break;
                case SearchPlaceType.Street:
                    query = query.Where(x => x.Address != null && x.Address.Street != null && EF.Functions.ILike(x.Address.Street, pattern));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(filter.SearchPlaceType));
            }
        }

        if (filter.Page > 1 && filter.Size > 0)
        {
            query = query.OrderBy(x => x.LastName).ThenBy(x=>x.FirstName).Skip(filter.Size.Value * (filter.Page.Value - 1)).Take(filter.Size.Value);
        }

        var userDbs = await query.ToArrayAsync(cancellationToken);

        return _mapper.Map<UserShortDataDto[]>(userDbs);
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

    public async Task<UserData> Save(SaveUserRequest request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var userDb = await _dbContext.Users
            .Include(x => x.Address)
            .Include(x => x.Groups)
            .Include(x => x.Phones).ThenInclude(x => x.Category)
            .SingleOrDefaultAsync(x => x.Id == request.Id && !x.DeletedUtc.HasValue, cancellationToken);

        if (userDb == null)
        {
            userDb = _mapper.Map<UserDb>(request);
            await _dbContext.AddAsync(userDb, cancellationToken);
        }
        else
        {
            _mapper.Map(request, userDb);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserData>(userDb);
    }

    public async Task Delete(Guid userId, CancellationToken cancellationToken)
    {
        var userDb = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId && !x.DeletedUtc.HasValue, cancellationToken);

        if (userDb == null) throw new EntityNotFoundException<UserDb>(userId.ToString());

        userDb.DeletedUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}