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
using Microsoft.AspNetCore.Mvc;
using AddressBookAssignment.Controllers;
using System.Net.Sockets;
using AddressBookAssignment;
using AddressBookAssignment.Memoryconnectionfactory;

namespace AddressBookXUnittest
{
    public class MetaDataTest
    {
        private readonly Mock<ILogger<AddressBookController>> _logger;
        private readonly IMapper _mapper;
        //private MockBookRepository _mockdb;

        public MetaDataTest()
        {
            _mapper = new Mapper(new MapperConfiguration(map =>
            {
                map.CreateMap<RefSetCreationDto, RefSet>();
                map.CreateMap<RefSet, RefSetToReturnDto>();

                map.CreateMap<RefTermCreationDto, RefTerm>();
                map.CreateMap<RefTerm, RefTermToReturnDto>();

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

            RefSet refsetdata = new RefSet
            {
                Id = Guid.Parse("7c3b9028-3afc-4862-a058-91085f5c6217"),
                Set = "ADDRESS_TYPE",
                Description = "address details"
            };

            context.RefSets.Add(refsetdata);
            context.SaveChanges();

            RefTerm reftermdata = new RefTerm
            {
                Id = Guid.Parse("6fba0b8f-a5b3-48f7-bece-ca4bd8d3cb90"),
                Key = "personal",
                Description = "personal email"
            };

            context.RefTerms.Add(reftermdata);
            context.SaveChanges();

            RefSetTerm refsettermdata = new RefSetTerm
            {
                Id = Guid.Parse("4e11a79e-3d5b-4ef2-907e-0deafc19085d"),
                RefSetId = Guid.Parse("7c3b9028-3afc-4862-a058-91085f5c6217"),
                RefTermId = Guid.Parse("6fba0b8f-a5b3-48f7-bece-ca4bd8d3cb90")
            };
            context.RefSetTerm.Add(refsettermdata);
            context.SaveChanges();

            return context;
        }

        [Fact]
        public void GetReferenceKey_ValidKey_ReturnKeyDetails()
        {
            var DbContextConnection = GetDBContext();

            var refsetrepository = new RefSetRepo(DbContextConnection);
            var refsettermrepository = new RefSetTermRepo(DbContextConnection);
            var reftermrepositiry = new RefTermRepo(DbContextConnection);
            var option = new DbContextOptionsBuilder<BookRepository>().UseInMemoryDatabase(databaseName: "AddressBookDB").Options;
            var bookrepo = new BookRepository(option);

            var refsetservicefile = new RefSetService(refsetrepository, refsettermrepository, reftermrepositiry,
                bookrepo);

            

            var reftermservicefile = new RefTermService(refsetrepository, refsettermrepository, reftermrepositiry);


            /*var mock = new Mock<DbSet<AddressBookDatabase>>();

            var mockContext = new Mock<BookRepository>();
            mockContext.Setup(m => m.AddressBooks).Returns(mock.Object);

            var service = new BlogService(mockContext.Object);
            */
            var controllerf = new MetaDataController(refsetservicefile, reftermservicefile, _mapper);

            //InValid Data
            //string InValidmap = "person";

            //var result = controllerfile.Mappingdata(map);

            //Assert.IsType<NotFoundObjectResult>(result);

            //Valid Data
            string map = "personal";

            var result = controllerf.Mappingdata(map);

            Assert.IsType<OkObjectResult>(result);

            
        }

        [Fact]
        public void GetReferenceKey_ValidKey_ReturnNotFound()
        {
            var DbContextConnection = GetDBContext();

            var refsetrepository = new RefSetRepo(DbContextConnection);
            var refsettermrepository = new RefSetTermRepo(DbContextConnection);
            var reftermrepositiry = new RefTermRepo(DbContextConnection);
            var option = new DbContextOptionsBuilder<BookRepository>().UseInMemoryDatabase(databaseName: "AddressBookDB").Options;
            var bookrepo = new BookRepository(option);

            var refsetservicefile = new RefSetService(refsetrepository, refsettermrepository, reftermrepositiry,
                bookrepo);


            var reftermservicefile = new RefTermService(refsetrepository, refsettermrepository, reftermrepositiry);


            /*var mock = new Mock<DbSet<AddressBookDatabase>>();

            var mockContext = new Mock<BookRepository>();
            mockContext.Setup(m => m.AddressBooks).Returns(mock.Object);

            var service = new BlogService(mockContext.Object);
            */

            var controllerfile = new MetaDataController(refsetservicefile, reftermservicefile, _mapper);

            //InValid Data
            string InValidmap = "person";

            var result = controllerfile.Mappingdata(InValidmap);

            Assert.IsType<NotFoundObjectResult>(result);

        }
    }
}
