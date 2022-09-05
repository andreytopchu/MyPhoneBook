using PhoneBook.Bll.Models;

namespace PhoneBook.Bll.Services;

public interface IUserDataService
{
    //Получение данных об абонентах с пагинацией
    //Получение полных данных об абоненте
    //Добавление данных об абонементе
    //Обновление абонента
    //Удаление абонента

    public Task<UserShortDataDto> GetUsers();
    public Task<UserData> GetUserData(Guid userId, CancellationToken cancellationToken);
    
}