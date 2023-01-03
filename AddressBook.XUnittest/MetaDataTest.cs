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

            User userdata = new User
            {
                Id = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644"),
                UserName = "kathir",
                FirstName = "kathir",
                LastName = "M",
                Password = "Kathir@123"
            };

            context.Users.Add(userdata);

            AddressBookDatabase addressbookdata = new AddressBookDatabase
            {
                Id = Guid.Parse("04b112f1-d649-4a07-8de5-3ac77918c0fe"),
                FirstName = "Kathir",
                LastName = "M",
                UserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644")
            };

            context.AddressBooks.Add(addressbookdata);

            Email emaildata = new Email
            {
                Id = Guid.Parse("4628e6a9-4a1e-42a9-b7b5-e734e1b679e4"),
                EmailAddress = "kathiresh@gmail.com",
                EmailTypeId = Guid.Parse("4e11a79e-3d5b-4ef2-907e-0deafc19085d"),
                AddressBookId = Guid.Parse("04b112f1-d649-4a07-8de5-3ac77918c0fe"),
                UserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644")
            };

            context.Emails.Add(emaildata);

            Address addressdata = new Address
            {
                Id = Guid.Parse("339222e5-72d3-4d35-9260-ab1bc92468de"),
                Line1 = "massstreet",
                Line2 = "100 feet street",
                City = "Tiruppur",
                StateName = "TN",
                ZipCode = "641603",
                AddressTypeId = Guid.Parse("1287279b-2613-47ca-9bd2-9ed3b51cae86"),
                CountryTypeId = Guid.Parse("3aac0854-de67-4bee-8e95-b48e6bc76b17"),
                AddressBookId = Guid.Parse("04b112f1-d649-4a07-8de5-3ac77918c0fe"),
                UserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644")
            };

            context.Addresses.Add(addressdata);

            Phone phonedata = new Phone
            {
                Id = Guid.Parse("a70fcbb6-4f76-4dd3-8158-18cceecc4fa0"),
                PhoneNumber = "9877743212",
                PhoneTypeId = Guid.Parse("c059f6e4-8743-4209-a240-1f0961fa027d"),
                AddressBookId = Guid.Parse("04b112f1-d649-4a07-8de5-3ac77918c0fe"),
                UserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644")
            };

            context.Phones.Add(phonedata);

            context.RefSets.AddRange(new RefSet
            {
                Id = Guid.Parse("7c3b9028-3afc-4862-a058-91085f5c6217"),
                Set = "ADDRESS_TYPE",
                Description = "address details"
            },
           new RefSet
           {
               Id = Guid.Parse("af50fd89-c762-4429-ab7a-88f78f32a72c"),
               Set = "PHONE_TYPE",
               Description = "phone details"
           },
           new RefSet
           {
               Id = Guid.Parse("edd5df89-09c2-44f8-8b7c-60e8a62c97e7"),
               Set = "COUNTRY_TYPE",
               Description = "country details"

           },
           new RefSet
           {
               Id = Guid.Parse("610f8eaa-fe3b-4124-9887-88768b424b8b"),
               Set = "EMAIL_TYPE",
               Description = "email details"
           });


            context.RefTerms.AddRange(new RefTerm
            {
                Id = Guid.Parse("6fba0b8f-a5b3-48f7-bece-ca4bd8d3cb90"),
                Key = "personal",
                Description = "personal email"
            },
            new RefTerm
            {
                Id = Guid.Parse("17aafb64-e1fe-4a08-b715-4f13982d845e"),
                Key = "personalphone",
                Description = "personal phone"
            },
            new RefTerm
            {
                Id = Guid.Parse("2cb9cef8-ba8d-4f31-acf1-122aa77e6a94"),
                Key = "workphone",
                Description = "work phone"
            },
            new RefTerm
            {
                Id = Guid.Parse("7137ad9a-c64a-4d5a-b219-d44e6e3418f8"),
                Key = "home",
                Description = "home address"
            },
            new RefTerm
            {
                Id = Guid.Parse("b8d42201-2d62-42f7-982a-8cf8e7b4a1e2"),
                Key = "India",
                Description = "country India"
            },
            new RefTerm
            {
                Id = Guid.Parse("d0abe194-0e69-4973-a54a-866946361397"),
                Key = "alternate",
                Description = "alternate phone"
            },
            new RefTerm
            {
                Id = Guid.Parse("db0b30a3-405a-4e99-a1ea-c5bab30e266c"),
                Key = "work",
                Description = "work email"
            });


            context.RefSetTerm.AddRange(new RefSetTerm
            {
                Id = Guid.Parse("4e11a79e-3d5b-4ef2-907e-0deafc19085d"),
                RefSetId = Guid.Parse("7c3b9028-3afc-4862-a058-91085f5c6217"),
                RefTermId = Guid.Parse("6fba0b8f-a5b3-48f7-bece-ca4bd8d3cb90")
            },
            new RefSetTerm
            {
                Id = Guid.Parse("1287279b-2613-47ca-9bd2-9ed3b51cae86"),
                RefSetId = Guid.Parse("7c3b9028-3afc-4862-a058-91085f5c6217"),
                RefTermId = Guid.Parse("7137ad9a-c64a-4d5a-b219-d44e6e3418f8")
            },
            new RefSetTerm
            {
                Id = Guid.Parse("3aac0854-de67-4bee-8e95-b48e6bc76b17"),
                RefSetId = Guid.Parse("edd5df89-09c2-44f8-8b7c-60e8a62c97e7"),
                RefTermId = Guid.Parse("b8d42201-2d62-42f7-982a-8cf8e7b4a1e2")
            },
            new RefSetTerm
            {
                Id = Guid.Parse("41c7cb1b-eed8-4e81-997d-9ff1ebd2eec2"),
                RefSetId = Guid.Parse("af50fd89-c762-4429-ab7a-88f78f32a72c"),
                RefTermId = Guid.Parse("17aafb64-e1fe-4a08-b715-4f13982d845e")
            },
            new RefSetTerm
            {
                Id = Guid.Parse("731ccd76-5edb-4dfe-a456-d1dd12e7a985"),
                RefSetId = Guid.Parse("af50fd89-c762-4429-ab7a-88f78f32a72c"),
                RefTermId = Guid.Parse("6fba0b8f-a5b3-48f7-bece-ca4bd8d3cb90")
            },
            new RefSetTerm
            {
                Id = Guid.Parse("839eac77-f6c5-4eb5-a712-e183d43521eb"),
                RefSetId = Guid.Parse("610f8eaa-fe3b-4124-9887-88768b424b8b"),
                RefTermId = Guid.Parse("db0b30a3-405a-4e99-a1ea-c5bab30e266c")
            },
            new RefSetTerm
            {
                Id = Guid.Parse("c059f6e4-8743-4209-a240-1f0961fa027d"),
                RefSetId = Guid.Parse("af50fd89-c762-4429-ab7a-88f78f32a72c"),
                RefTermId = Guid.Parse("2cb9cef8-ba8d-4f31-acf1-122aa77e6a94")
            },
            new RefSetTerm
            {
                Id = Guid.Parse("df2f0977-be15-47b1-8d03-70c86f353edf"),
                RefSetId = Guid.Parse("af50fd89-c762-4429-ab7a-88f78f32a72c"),
                RefTermId = Guid.Parse("d0abe194-0e69-4973-a54a-866946361397")
            });

            //context.RefSetTerm.Add(refsettermdata);
            context.SaveChanges();

            return context;
        }

        /// <summary>
        /// Method to Test Get Reference Key API
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void GetReferenceKey_ValidKey_ReturnKeyDetails()
        {
            var DbContextConnection = GetDBContext();

            var refsetrepository = new RefSetRepository(DbContextConnection);
            var refsettermrepository = new RefSetTermRepository(DbContextConnection);
            var reftermrepositiry = new RefTermRepository(DbContextConnection);
            var option = new DbContextOptionsBuilder<BookRepository>().UseInMemoryDatabase(databaseName: "AddressBookDB").Options;
            var bookrepo = new BookRepository(option);

            var refsetservicefile = new RefSetService(refsetrepository, refsettermrepository, reftermrepositiry,
                bookrepo);

            var reftermservicefile = new RefTermService(refsetrepository, refsettermrepository, reftermrepositiry);

            var controllerf = new MetaDataController(refsetservicefile, reftermservicefile, _mapper);

            string map = "work";

            var result = controllerf.Mappingdta(map);

            var finalresult = Assert.IsType<OkObjectResult>(result);
            var convertstring = (finalresult.Value).ToString();
            Assert.Equal("db0b30a3-405a-4e99-a1ea-c5bab30e266c", convertstring);
            
        }

        /// <summary>
        /// Method to Test Get Reference Key API
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void GetReferenceKey_ValidKey_ReturnNotFound()
        {
            var DbContextConnection = GetDBContext();

            var refsetrepository = new RefSetRepository(DbContextConnection);
            var refsettermrepository = new RefSetTermRepository(DbContextConnection);
            var reftermrepositiry = new RefTermRepository(DbContextConnection);
            var option = new DbContextOptionsBuilder<BookRepository>().UseInMemoryDatabase(databaseName: "AddressBookDB").Options;
            var bookrepo = new BookRepository(option);

            var refsetservicefile = new RefSetService(refsetrepository, refsettermrepository, reftermrepositiry,
                bookrepo);


            var reftermservicefile = new RefTermService(refsetrepository, refsettermrepository, reftermrepositiry);

            var controllerfile = new MetaDataController(refsetservicefile, reftermservicefile, _mapper);

            string InValidmap = "person";

            var result = controllerfile.Mappingdata(InValidmap);

            var finalresult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Key not found",finalresult.Value);

        }
    }
}
