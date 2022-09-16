using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PhoneBook.Bll.Models;
using PhoneBook.Dal;
using Shared.Constants;
using Shared.Exceptions;
using Shared.Services;
using FileOptions = PhoneBook.Bll.Options.FileOptions;

namespace PhoneBook.Bll.Services;

public class FileService : IFileService
{
    private readonly PhoneBookDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IDataProvider _dataProvider;
    private readonly FileOptions _fileOptions;

    public FileService(PhoneBookDbContext dbContext, IMapper mapper, IDataProvider dataProvider, IOptions<FileOptions> fileOptions)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _dataProvider = dataProvider;
        _fileOptions = fileOptions.Value;
    }

    public async Task<string> SaveImage(IFormFile formFile, CancellationToken cancellationToken)
    {
        if (formFile == null) throw new ArgumentNullException(nameof(formFile));

        var fileExtension = Path.GetExtension(formFile.FileName);

        if (!FileExtensions.ImageExtensions.Contains(fileExtension)) throw new FileBadFormatException(fileExtension);

        var fileName = Guid.NewGuid() + fileExtension;
        var filePath = Path.Combine(new[] {_fileOptions.BasePath, fileName});

        await using Stream fileStream = new FileStream(filePath, FileMode.Create);
        await formFile.CopyToAsync(fileStream, cancellationToken);

        return filePath;
    }

    public async Task<string> Export(CancellationToken cancellationToken)
    {
        var userDbs = await _dbContext.Users.Where(x => !x.DeletedUtc.HasValue)
            .Include(x => x.Phones)
            .Include(x => x.Groups)
            .ToArrayAsync(cancellationToken);

        var exportUsers = _mapper.Map<ExportUserDataDto[]>(userDbs);
        var fileName = Guid.NewGuid() + ".xls";
        var filePath = Path.Combine(new[] {_fileOptions.BasePath, fileName});

        await _dataProvider.SaveToExelFile(exportUsers, filePath);
        return filePath;
    }
}