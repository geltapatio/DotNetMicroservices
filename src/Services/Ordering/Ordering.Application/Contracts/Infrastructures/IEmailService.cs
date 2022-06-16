using Ordering.Application.Models;

namespace Ordering.Application.Contracts.Infrastructures
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}
