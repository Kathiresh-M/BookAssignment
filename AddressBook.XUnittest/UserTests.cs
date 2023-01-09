using AddressBookAssignment.Controllers;
using AutoMapper;
using Entities.Dto;
using Entities.RequestDto;
using Entities;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository;
using Services.Helper;
using Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using AddressBookAssignment;
using Services.Helper.Contractconnect;

namespace AddressBookXUnittest
{
    public class UserTests
    {
        private readonly Mock<ILogger<Usercontroller>> _logger;
        private readonly IMapper _mapper;

        public UserTests()
        {
            _logger = new Mock<ILogger<Usercontroller>>();
            _mapper = new Mapper(new MapperConfiguration(map =>
            {
                map.CreateMap<User, UserReturnDto>();
            }));
        }

        public BookRepository GetDBContext()
        {
            var option = new DbContextOptionsBuilder<BookRepository>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var context = new BookRepository(option);
            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            User userdata = new User
            {
                Id = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644"),
                UserName = "kathir",
                FirstName = "kathir",
                LastName = "M",
                Password = "Kathir@123"
            };

            context.Users.Add( userdata );
            context.SaveChanges();

            return context;
        }

        /// <summary>
        /// Method to Test Get Valid Token API
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void GetValidToken_ValidId_ReturnToken()
        {
            var DbContextConnection = GetDBContext();

            var usesrrepository = new UserRepository(DbContextConnection);
            var password = new Password();

            var servicefile = new Userservice(usesrrepository, password);

            var controllerfile = new Usercontroller(servicefile, _mapper, _logger.Object);

            UserDto userdata = new UserDto()
            {
                UserName = "kathir",
                Password = "Kathir@123"
            };

            var result = controllerfile.AuthUser(userdata);

            Assert.NotNull(result);
            Assert.IsNotType<UnauthorizedObjectResult>(result);
            Assert.IsType<OkObjectResult>(result);
        }

        /// <summary>
        /// Method to Test Get Valid Token API
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void GetValidToken_InValid_ReturnUnauthorized()
        {
            var DbContextConnection = GetDBContext();

            var usesrrepository = new UserRepository(DbContextConnection);
            var password = new Password();

            var servicefile = new Userservice(usesrrepository, password);

            var controllerfile = new Usercontroller(servicefile, _mapper, _logger.Object);

            UserDto userDto = new UserDto()
            {
                UserName = "kathiresh",
                Password = "Kathir@1234"
            };

            var result = controllerfile.AuthUsers(userDto);

            Assert.NotNull(result);
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}

