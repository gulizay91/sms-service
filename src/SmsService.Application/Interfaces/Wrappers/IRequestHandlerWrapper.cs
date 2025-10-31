using MediatR;

namespace SmsService.Application.Interfaces.Wrappers;

public interface IRequestHandlerWrapper<TIn, TOut> : IRequestHandler<TIn, TOut>
  where TIn : IRequestWrapper<TOut>
{
}