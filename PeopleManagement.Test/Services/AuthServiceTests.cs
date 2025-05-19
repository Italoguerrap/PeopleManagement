using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using PeopleManagement.Application.DTOs;
using PeopleManagement.Application.Interfaces;
using PeopleManagement.Application.Services;
using PeopleManagement.Domain.Entities;
using PeopleManagement.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PeopleManagement.Test.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IPersonRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IPasswordHasher<Person>> _mockPasswordHasher;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockRepository = new Mock<IPersonRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockPasswordHasher = new Mock<IPasswordHasher<Person>>();
            _mockTokenService = new Mock<ITokenService>();
            _authService = new AuthService(
                _mockRepository.Object,
                _mockMapper.Object,
                _mockPasswordHasher.Object,
                _mockTokenService.Object);
        }

        [Fact]
        public async Task AuthenticateAsync_WithValidCredentials_ReturnsToken()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var cpf = "12345678901";
            var password = "password123";
            var person = new Person 
            { 
                Id = 1, 
                Name = "Test Person", 
                CPF = cpf,
                Email = "test@example.com",
                PasswordHash = "hashedPassword123"
            };
            var personDto = new PersonDto 
            { 
                Name = "Test Person", 
                CPF = cpf,
                Email = "test@example.com"
            };
            var token = new Token 
            { 
                AccessToken = "test-token",
                Expiration = DateTime.UtcNow.AddHours(1),
                UserId = person.Id
            };
            var tokenDto = new TokenDto 
            { 
                AccessToken = token.AccessToken,
                Expiration = token.Expiration,
                Person = personDto
            };

            _mockRepository.Setup(repo => repo.GetByCpfAsync(cpf, cancellationToken))
                .ReturnsAsync(person);

            _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(person, person.PasswordHash, password))
                .Returns(PasswordVerificationResult.Success);

            _mockTokenService.Setup(ts => ts.GenerateToken(person.Id))
                .Returns(token);

            _mockMapper.Setup(m => m.Map<PersonDto>(person))
                .Returns(personDto);

            // Act
            var result = await _authService.AuthenticateAsync(cpf, password, cancellationToken);

            // Assert
            Assert.Equal(token.AccessToken, result.AccessToken);
            Assert.Equal(token.Expiration, result.Expiration);
            Assert.Equal(personDto.Name, result.Person.Name);
            Assert.Equal(personDto.CPF, result.Person.CPF);
            
            _mockRepository.Verify(repo => repo.GetByCpfAsync(cpf, cancellationToken), Times.Once);
            _mockPasswordHasher.Verify(ph => ph.VerifyHashedPassword(person, person.PasswordHash, password), Times.Once);
            _mockTokenService.Verify(ts => ts.GenerateToken(person.Id), Times.Once);
            _mockMapper.Verify(m => m.Map<PersonDto>(person), Times.Once);
        }

        [Fact]
        public async Task AuthenticateAsync_WithInvalidPassword_ThrowsException()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var cpf = "12345678901";
            var password = "wrongPassword";
            var person = new Person 
            { 
                Id = 1, 
                Name = "Test Person", 
                CPF = cpf,
                Email = "test@example.com",
                PasswordHash = "hashedPassword123"
            };

            _mockRepository.Setup(repo => repo.GetByCpfAsync(cpf, cancellationToken))
                .ReturnsAsync(person);

            _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(person, person.PasswordHash, password))
                .Returns(PasswordVerificationResult.Failed);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<PeopleManagementException>(
                () => _authService.AuthenticateAsync(cpf, password, cancellationToken));
            
            Assert.Equal("Credenciais inválidas", exception.Message);
            
            _mockRepository.Verify(repo => repo.GetByCpfAsync(cpf, cancellationToken), Times.Once);
            _mockPasswordHasher.Verify(ph => ph.VerifyHashedPassword(person, person.PasswordHash, password), Times.Once);
            _mockTokenService.Verify(ts => ts.GenerateToken(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async Task AuthenticateAsync_WithNonExistentUser_ThrowsException()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var cpf = "12345678901";
            var password = "password123";

            _mockRepository.Setup(repo => repo.GetByCpfAsync(cpf, cancellationToken))
                .ReturnsAsync((Person)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<PeopleManagementException>(
                () => _authService.AuthenticateAsync(cpf, password, cancellationToken));
            
            Assert.Equal("Credenciais inválidas", exception.Message);
            
            _mockRepository.Verify(repo => repo.GetByCpfAsync(cpf, cancellationToken), Times.Once);
            _mockPasswordHasher.Verify(ph => ph.VerifyHashedPassword(It.IsAny<Person>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockTokenService.Verify(ts => ts.GenerateToken(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async Task AuthenticateAsync_WhenPasswordVerificationThrowsException_ThrowsCustomException()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var cpf = "12345678901";
            var password = "password123";
            var person = new Person 
            { 
                Id = 1, 
                Name = "Test Person", 
                CPF = cpf,
                Email = "test@example.com",
                PasswordHash = "hashedPassword123"
            };

            _mockRepository.Setup(repo => repo.GetByCpfAsync(cpf, cancellationToken))
                .ReturnsAsync(person);

            _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(person, person.PasswordHash, password))
                .Throws(new Exception("Internal error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<PeopleManagementException>(
                () => _authService.AuthenticateAsync(cpf, password, cancellationToken));
            
            Assert.Equal("Credenciais inválidas", exception.Message);
            
            _mockRepository.Verify(repo => repo.GetByCpfAsync(cpf, cancellationToken), Times.Once);
            _mockPasswordHasher.Verify(ph => ph.VerifyHashedPassword(person, person.PasswordHash, password), Times.Once);
            _mockTokenService.Verify(ts => ts.GenerateToken(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void IsValidToken_ThrowsNotImplementedException()
        {
            // Arrange
            var token = "test-token";

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => _authService.IsValidToken(token));
        }
    }
}