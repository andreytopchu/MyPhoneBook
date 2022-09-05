﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Bll.Models;
using PhoneBook.Bll.Services;
using Shared.Bll.Controllers;

namespace PhoneBook.Controllers;

public class GroupController : BaseController
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    /// <summary>
    /// Получить все группы
    /// </summary>
    /// <param name="request">Дто для фитрации и пагинации</param>
    public async Task<ActionResult<GroupDto[]>> Get(FilterRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _groupService.Get(request, cancellationToken));
    }

    /// <summary>
    /// Сохранить данные о группе
    /// </summary>
    /// <param name="request">Запрос на сохранение данных о группе</param>
    public async Task<ActionResult<GroupDto>> Save(SaveGroupRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _groupService.Save(request, cancellationToken));
    }

    /// <summary>
    /// Удалить группу
    /// </summary>
    /// <param name="groupId">Id группы</param>
    public async Task<IActionResult> Delete([FromQuery] Guid groupId, CancellationToken cancellationToken)
    {
        await _groupService.Delete(groupId, cancellationToken);
        return Ok();
    }
}