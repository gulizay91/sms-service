using MediatR;

namespace SmsService.Application.Interfaces.Wrappers;

public interface IRequestWrapper<T> : IRequest<T>
{
}