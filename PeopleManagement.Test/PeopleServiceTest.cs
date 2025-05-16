using AutoMapper;
using Moq;
using PeopleManagement.Application.DTOs;
using PeopleManagement.Application.Interfaces;
using PeopleManagement.Application.Services;
using PeopleManagement.Application.Validations;
using PeopleManagement.Domain.Entites;
using PeopleManagement.Domain.Enums;

namespace PeopleManagement.Test
{
    public class PeopleServiceTest
    {
        private readonly Mock<IPersonRepository> _personRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PeopleService _peopleService;

        public PeopleServiceTest()
        {
            _personRepositoryMock = new Mock<IPersonRepository>();
            _mapperMock = new Mock<IMapper>();
            var validator = new PersonEntityValidator(_personRepositoryMock.Object);
            _peopleService = new PeopleService(_personRepositoryMock.Object, _mapperMock.Object, validator);
        }


        [Fact]
        public async Task SearchAsync_ShouldReturnMappedDtos()
        {
            // Arrange
            var query = "John";
            var cancellationToken = CancellationToken.None;

            var people = new List<Person>
            {
                new Person
                {
                    Name = "John Doe",
                    Email = "john@example.com",
                    Gender = GenderType.Male,
                    DateOfBirth = new DateTime(1990, 1, 1)
                },
                new Person
                {
                    Name = "Johnny Smith",
                    Email = "johnny@example.com",
                    Gender = GenderType.Male,
                    DateOfBirth = new DateTime(1995, 5, 10)
                }
            };

            var personDtos = new List<PersonDto>
            {
                new PersonDto
                {
                    Name = "John Doe",
                    Email = "john@example.com",
                    Gender = GenderType.Male,
                    DateOfBirth = new DateTime(1990, 1, 1)
                },
                new PersonDto
                {
                    Name = "Johnny Smith",
                    Email = "johnny@example.com",
                    Gender = GenderType.Male,
                    DateOfBirth = new DateTime(1995, 5, 10)
                }
            };

            _personRepositoryMock
                .Setup(repo => repo.SearchAsync(query, cancellationToken))
                .ReturnsAsync(people);

            _mapperMock
                .Setup(m => m.Map<PersonDto>(It.IsAny<Person>()))
                .Returns<Person>(p => personDtos.FirstOrDefault(dto =>
                    dto.Name == p.Name &&
                    dto.Email == p.Email));

            // Act
            var result = await _peopleService.SearchAsync(query, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("John Doe", result[0].Name);
            Assert.Equal("Johnny Smith", result[1].Name);

            _personRepositoryMock.Verify(repo => repo.SearchAsync(query, cancellationToken), Times.Once);
            _mapperMock.Verify(m => m.Map<PersonDto>(It.IsAny<Person>()), Times.Exactly(2));
        }

        [Fact]
        public async Task SearchAsync_WithEmptyResult_ShouldReturnEmptyList()
        {
            // Arrange
            var query = "NonExistentPerson";
            var cancellationToken = CancellationToken.None;

            _personRepositoryMock
                .Setup(repo => repo.SearchAsync(query, cancellationToken))
                .ReturnsAsync(new List<Person>());

            // Act
            var result = await _peopleService.SearchAsync(query, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _personRepositoryMock.Verify(repo => repo.SearchAsync(query, cancellationToken), Times.Once);
            _mapperMock.Verify(m => m.Map<PersonDto>(It.IsAny<Person>()), Times.Never);
        }

    }
}