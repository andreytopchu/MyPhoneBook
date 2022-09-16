using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Bll.Services;
using Shared.Controllers;

namespace PhoneBook.Controllers;

public class FileController : BaseController
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    /// <summary>
    /// Выгрузка телефонной книги
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ExportReport(CancellationToken cancellationToken)
    {
        return PhysicalFile(await _fileService.Export(cancellationToken),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    /// <summary>
    /// Сохранение картинки
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<string>> AddImage(IFormFile image, CancellationToken cancellationToken)
    {
        return await _fileService.SaveImage(image, cancellationToken);
    }
}