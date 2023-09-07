using System.Threading.Tasks;

namespace vita.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}