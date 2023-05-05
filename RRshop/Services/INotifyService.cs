namespace RRshop.Services;

public interface INotifyService
{
    public string SecretKey { get; set; }

    public Task Notify(string message);
}