using AuthApp.Api.Filters;
using AuthApp.API.Controllers;
using AuthApp.Application.ApplicationUser.Commands.CreateUser;
using AuthApp.Application.Common.Behaviours;
using AuthApp.Application.Common.Interfaces;
using AuthApp.Application.Dto;
using AuthApp.Domain.Models;
using AuthApp.Tests.Common;

using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;

using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace ExchangeCore.Tests.Controller;
public class UsersControllerTests
{
    private HttpClient _httpClient;
    private ServiceCollection _serviceCollection;
    private Mock<IIdentityService> _mockIdentityService = new();

    private const string _testDataPath = "./TestData/Users.json";

    public UsersControllerTests()
    {
        _serviceCollection = new ServiceCollection();

        _serviceCollection.AddSingleton(GetConfiguredMappingConfig());
        _serviceCollection.AddScoped<IMapper, ServiceMapper>();

        _serviceCollection.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
        _serviceCollection.AddMediatR(typeof(CreateUserCommandHandler).Assembly);

        // Add behaviors via service collection for older versions
        _serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        _serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

        _serviceCollection.AddHttpContextAccessor();
    }

    #region CreateUser TestCases

    [Fact]
    public async Task CreateUser_WhenValidUserDetails_ReturnsOkResult()
    {
        // Arrange
        var requestUri = "api/users";
        var cancellationToken = new CancellationToken();

        var request = await JsonHelper.GetRequestModelAsync<CreateUserCommand>(_testDataPath, ActionTypeEnum.Create);

        CreateUsersConfigureServices(false);

        // Act
        var response = await _httpClient.PostAsync(requestUri,
            new StringContent(JsonSerializer.Serialize(request),
            Encoding.UTF8,
            TestConstants.JSON_CONTENT_TYPE),
            cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();

        var apiResponse = JsonSerializer.Deserialize<ServiceResult<ApplicationUserDto>>(content, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        Assert.NotNull(apiResponse);

        Assert.IsType<ApplicationUserDto>(apiResponse.Data);

        Assert.True(apiResponse.Succeeded);
    }

    [Fact]
    public async Task CreateUser_MultipleValidationErrors_ReturnsAllErrorMessages()
    {
        // Arrange
        var requestUri = "api/users";
        var cancellationToken = new CancellationToken();

        var request = await JsonHelper.GetRequestModelAsync<CreateUserCommand>(_testDataPath, ActionTypeEnum.Create);
        request.Username = null; // Missing username
        request.Email = "invalid"; // Invalid email

        CreateUsersConfigureServices(false);

        // Act
        var response = await _httpClient.PostAsync(requestUri,
            new StringContent(JsonSerializer.Serialize(request),
            Encoding.UTF8,
            TestConstants.JSON_CONTENT_TYPE),
            cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<ServiceResult<Dictionary<string, List<string>>>>(content, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        Assert.NotNull(apiResponse);
        Assert.False(apiResponse.Succeeded);

        // Validate error structure
        Assert.NotNull(apiResponse.Error);
        Assert.Equal("One or more validation errors occurred.", apiResponse.Error.Message);
        Assert.Equal(900, apiResponse.Error.Code);

        // Validate all error fields exist
        Assert.NotNull(apiResponse.Data);
        Assert.True(apiResponse.Data.ContainsKey("Username"));
        Assert.True(apiResponse.Data.ContainsKey("Email"));

        // Validate specific error messages
        Assert.Contains("username.required", apiResponse.Data["Username"]);
        Assert.Contains("email.invalid", apiResponse.Data["Email"]);
    }

    private void CreateUsersConfigureServices(bool invalidDetails)
    {
        _mockIdentityService
                .Setup(d => d.CreateUserAsync(It.IsAny<CreateUserCommand>()))
                .ReturnsAsync((Result.Success(), It.IsAny<string>()));

        _mockIdentityService
                .Setup(d => d.AssignRolesToUserAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns(Task.CompletedTask);

        _serviceCollection.AddSingleton(_mockIdentityService.Object);

        ConfigureServices();
    }
    #endregion

    private void ConfigureServices(bool isAdminRole = false)
    {
        using var host = new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder
                    .UseTestServer()
                    .ConfigureTestServices(services =>
                    {

                        services
                            .AddAuthentication("TestScheme")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });

                        services.AddAuthorization(options =>
                        {
                            options.AddPolicy("AppUserRole", policy =>
                                policy.RequireRole("Administrator"));
                        });

                        services.AddRouting();

                        foreach (var serviceDescriptor in _serviceCollection)
                        {
                            services.Add(serviceDescriptor);
                        }

                        var feature = new ControllerFeature();
                        var assembly = typeof(UsersController).Assembly;
                        var manager = new ApplicationPartManager();
                        manager.ApplicationParts.Add(new AssemblyPart(assembly));
                        manager.FeatureProviders.Add(new ControllerFeatureProvider());
                        manager.PopulateFeature(feature);

                        services.AddSingleton(feature);
                        services.AddControllers(options =>
                            options.Filters.Add<ApiExceptionFilterAttribute>())
                        .ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(new AssemblyPart(assembly)));

                        services.AddFluentValidationAutoValidation(); // Enables automatic validation
                        services.AddFluentValidationClientsideAdapters(); // For client-side validation integration

                        // Customise default API behaviour
                        services.Configure<ApiBehaviorOptions>(options =>
                        {
                            options.SuppressModelStateInvalidFilter = true;
                        });
                    })
                    .Configure(app =>
                    {

                        app.UseRouting();

                        app.UseAuthentication();
                        app.UseAuthorization();

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    });
            })
            .StartAsync();

        _httpClient = host.Result.GetTestClient();
    }

    /// <summary>
    /// Mapster(Mapper) global configuration settings
    /// To learn more about Mapster,
    /// see https://github.com/MapsterMapper/Mapster
    /// </summary>
    /// <returns></returns>
    private static TypeAdapterConfig GetConfiguredMappingConfig()
    {
        var config = TypeAdapterConfig.GlobalSettings;

        IList<IRegister> registers = config.Scan(Assembly.GetExecutingAssembly());

        config.Apply(registers);

        return config;
    }
}