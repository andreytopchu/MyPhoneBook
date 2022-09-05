using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Bll.Models;
using PhoneBook.Bll.Services;
using Shared.Bll.Controllers;

namespace PhoneBook.Controllers;

public class PhoneCategoryController : BaseController
{
    private readonly IPhoneCategoryService _categoryService;

    public PhoneCategoryController(IPhoneCategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Получить все категории
    /// </summary>
    /// <param name="request">Дто для фитрации и пагинации</param>
    public async Task<ActionResult<PhoneCategoryDto[]>> Get(FilterRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _categoryService.Get(request, cancellationToken));
    }

    /// <summary>
    /// Сохранить данные о категории
    /// </summary>
    /// <param name="request">Запрос на сохранение данных о категории</param>
    public async Task<ActionResult<PhoneCategoryDto>> Save(SaveCategoryRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _categoryService.Save(request, cancellationToken));
    }

    /// <summary>
    /// Удалить категорию
    /// </summary>
    /// <param name="categoryId">Id категории</param>
    public async Task<IActionResult> Delete([FromQuery] Guid categoryId, CancellationToken cancellationToken)
    {
        await _categoryService.Delete(categoryId, cancellationToken);
        return Ok();
    }
}