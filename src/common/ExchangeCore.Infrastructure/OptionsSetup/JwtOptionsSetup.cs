using AuthApp.Domain.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AuthApp.Infrastructure.OptionsSetup;

public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
{
    private readonly IConfiguration _configuration;

    public JwtOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(JwtOptions options)
    {
        _configuration.GetSection(JwtOptions.SectionName).Bind(options);
    }
}

