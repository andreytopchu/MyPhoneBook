using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Bll.Models;
using PhoneBook.Dal;
using PhoneBook.Dal.Models;
using Shared.Bll.Exceptions;

namespace PhoneBook.Bll.Services;

public class PhoneCategoryService
{
    private readonly PhoneBookDbContext _dbContext;
    private readonly IMapper _mapper;

    public PhoneCategoryService(PhoneBookDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PhoneCategoryDto[]> Get(string? searchPhrase, int? page, int? size, CancellationToken cancellationToken)
    {
        var query = _dbContext.PhoneCategories.Where(x => !x.DeletedUtc.HasValue);

        if (!string.IsNullOrWhiteSpace(searchPhrase))
        {
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{searchPhrase}%"));
        }

        if (page > 1 && size > 0)
        {
            query = query.OrderBy(x => x.Name).Skip(size.Value * (page.Value - 1)).Take(size.Value);
        }

        var categories = await query.ToArrayAsync(cancellationToken);

        return _mapper.Map<PhoneCategoryDto[]>(categories);
    }

    public async Task<PhoneCategoryDto> Save(SaveCategoryRequest request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var categoryDb = await GetCategoryById(request.Id, cancellationToken);

        if (categoryDb == null)
        {
            categoryDb = _mapper.Map<PhoneCategoryDb>(request);
            _dbContext.Add(categoryDb);
        }
        else
        {
            _mapper.Map(request, categoryDb);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PhoneCategoryDto>(GetCategoryById(categoryDb.Id, cancellationToken));
    }

    public async Task Delete(Guid categoryId, CancellationToken cancellationToken)
    {
        var categoryDb = await GetCategoryById(categoryId, cancellationToken);

        if (categoryDb == null) throw new EntityNotFoundException<GroupDb>(categoryId.ToString());

        categoryDb.DeletedUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<PhoneCategoryDb?> GetCategoryById(Guid? id, CancellationToken cancellationToken)
    {
        return await _dbContext.PhoneCategories.SingleOrDefaultAsync(x => x.Id == id && !x.DeletedUtc.HasValue, cancellationToken);
    }
}