namespace WASMChat.Pages.Chat.Storage.Abstraction;

public interface IRemoteDataProxy<T>
{
    public Task<T> GetAsync();
    public event Action<T> DataUpdated;
}