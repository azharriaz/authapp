using AuthApp.Domain.Models;
using MediatR;

namespace AuthApp.Application.Common.Interfaces;

public interface IRequestWrapper<T> : IRequest<ServiceResult<T>>
{

}

public interface IRequestHandlerWrapper<TIn, TOut> : IRequestHandler<TIn, ServiceResult<TOut>> where TIn : IRequestWrapper<TOut>
{

}
