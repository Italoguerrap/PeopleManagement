using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using PeopleManagement.Infrastructure.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace PeopleManagement.Test.Services
{
    public class TokenServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly TokenService _tokenService;
        private readonly string _secretKey = "ThisIsASecretKeyForTestingPurposesItNeedsToBeLongEnough";
        private readonly string _issuer = "TestIssuer";
        private readonly string _audience = "TestAudience";

        public TokenServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(c => c["Jwt:SecretKey"]).Returns(_secretKey);
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns(_issuer);
            _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns(_audience);
            
            _tokenService = new TokenService(_mockConfiguration.Object);
        }

        [Fact]
        public void GenerateToken_ReturnsValidToken()
        {
            // Arrange
            var userId = 123L;

            // Act
            var result = _tokenService.GenerateToken(userId);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.AccessToken);
            Assert.Equal(userId, result.UserId);
            Assert.True(result.Expiration > DateTime.UtcNow);
            
            // Verify the token can be decoded and has the correct claims
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(result.AccessToken, validationParameters, out _);
            
            Assert.NotNull(principal);
            // Check for sub claim (which is the ClaimTypes.NameIdentifier)
            Assert.Contains(principal.Claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == userId.ToString());
            // Check for the id claim
            Assert.Contains(principal.Claims, c => c.Type == "id" && c.Value == userId.ToString());
            // Check for jti claim (must have some value)
            Assert.Contains(principal.Claims, c => c.Type == "jti" && !string.IsNullOrEmpty(c.Value));
        }

        [Fact]
        public void GenerateToken_TokenHasCorrectExpiration()
        {
            // Arrange
            var userId = 123L;

            // Act
            var result = _tokenService.GenerateToken(userId);

            // Assert
            // The expiration should be approximately 1 hour from now
            var expectedExpiration = DateTime.UtcNow.AddHours(1);
            var difference = result.Expiration - expectedExpiration;
            
            // Allow a small margin of error (a few seconds) due to test execution time
            Assert.True(Math.Abs(difference.TotalSeconds) < 5);
        }

        [Fact]
        public void GenerateToken_WithoutSecretKey_ThrowsException()
        {
            // Arrange
            var mockConfigWithoutKey = new Mock<IConfiguration>();
            mockConfigWithoutKey.Setup(c => c["Jwt:SecretKey"]).Returns((string)null);
            mockConfigWithoutKey.Setup(c => c["Jwt:Issuer"]).Returns(_issuer);
            mockConfigWithoutKey.Setup(c => c["Jwt:Audience"]).Returns(_audience);
            
            var tokenService = new TokenService(mockConfigWithoutKey.Object);
            var userId = 123L;

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => tokenService.GenerateToken(userId));
            Assert.Equal("A chave JWT não está configurada.", exception.Message);
        }
    }
}