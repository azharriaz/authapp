using AuthApp.Application.Common.Models;
using AuthApp.Domain.Enums;

namespace AuthApp.Application.Common.Interfaces;

public interface IHttpClientHandler
{
    Task<ServiceResult<TResult>> GenericRequest<TRequest, TResult>(string clientApi, string url,
        CancellationToken cancellationToken,
        MethodType method = MethodType.Get,
        TRequest requestEntity = null)
        where TResult : class where TRequest : class;
}