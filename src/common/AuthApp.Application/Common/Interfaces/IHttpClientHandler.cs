using AuthApp.Domain.Enums;
using AuthApp.Domain.Models;

namespace AuthApp.Application.Common.Interfaces;

public interface IHttpClientHandler
{
    Task<ServiceResult<TResult>> GenericRequest<TRequest, TResult>(string clientApi, string url,
        CancellationToken cancellationToken,
        MethodType method = MethodType.Get,
        TRequest requestEntity = null)
        where TResult : class where TRequest : class;
}