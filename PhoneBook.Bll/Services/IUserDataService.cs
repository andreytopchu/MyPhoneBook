using PhoneBook.Bll.Models;

namespace PhoneBook.Bll.Services;

/// <summary>
/// Интерфейс для управления абонентами
/// </summary>
public interface IUserDataService
{
    /// <summary>
    /// Получить всех абонентов с фильтрацией
    /// </summary>
    /// <param name="filter">Запрос на фильтрацию</param>
    /// <param name="cancellationToken"></param>
    public Task<UserShortDataDto[]> GetUsers(FilterRequest filter, CancellationToken cancellationToken);

    /// <summary>
    /// Получить полные данные об абоненте
    /// </summary>
    /// <param name="userId">Идентификатор абонета</param>
    public Task<UserData> GetUserData(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Сохранить данные абонента
    /// </summary>
    /// <param name="request">Запрос на сохранение данных абонента</param>
    public Task<UserData> Save(SaveUserRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление абонента
    /// </summary>
    /// <param name="userId">Идентификатор абонента</param>
    public Task Delete(Guid userId, CancellationToken cancellationToken);
}