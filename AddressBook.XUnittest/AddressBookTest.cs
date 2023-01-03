using AddressBookAssignment.Controllers;
using AutoMapper;
using Contract;
using Contract.IHelper;
using Entities.Dto;
using Entities.RequestDto;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Repository;
using Services;
using Services.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Repository.Hierarchy;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace AddressBookXUnittest
{
    public class AddressBookTest
    {
        private readonly Mock<ILogger<AddressBookController>> _logger;
        private readonly IMapper _mapper;
        private readonly IAddressBookRepo _addressbookrepository;
        private readonly AddressBookController _controller;
        private readonly IRefSetRepo _refsetRepository;
        private readonly IRefSetTermRepo _refsettermrepository;
        private readonly IAssetRepo _assetrepository;
        private readonly IAddressRepo _addressrepository;
        private readonly IRefTermRepo _reftermrepository;
        private readonly IEmailRepo _emailrepository;
        private readonly IPhoneRepo _phonerepository;
        private readonly IUserRepo _userrepository;
        private readonly IAddressBookService addressBookService;
        public AddressBookTest()
        {
            _logger= new Mock<ILogger<AddressBookController>>();
            var DbContextConnection = GetDBContextupdate();
            _addressbookrepository = new AddressBookRepository(DbContextConnection);
            _refsetRepository= new RefSetRepository(DbContextConnection);
            _refsettermrepository = new RefSetTermRepository(DbContextConnection);
            _assetrepository = new AssetRepository(DbContextConnection);
            _addressrepository = new AddressRepository(DbContextConnection);
            _reftermrepository = new RefTermRepository(DbContextConnection);
            _emailrepository= new EmailRepository(DbContextConnection);
            _phonerepository= new PhoneRepository(DbContextConnection);
            _userrepository= new UserRepository(DbContextConnection);
            addressBookService = new AddressBookService(_addressbookrepository, _refsetRepository, _refsettermrepository, _assetrepository,
                _addressrepository, _reftermrepository, _emailrepository, _phonerepository, _userrepository);

            _controller = new AddressBookController(addressBookService, _mapper, _logger.Object);

            _mapper = new Mapper(new MapperConfiguration(map =>
            {
                map.CreateMap<AddressBookCreateDto, AddressBookDto>();
                map.CreateMap<EmailDto, Email>();
                map.CreateMap<PhoneDto, Phone>();
                map.CreateMap<AddressDto, Address>();

                map.CreateMap<AssetRequestDto, Asset>();
                map.CreateMap<Asset, AssetReturnDto>();

                map.CreateMap<RefSetCreationDto, RefSet>();
                map.CreateMap<RefSet, RefSetToReturnDto>();

                map.CreateMap<RefTermCreationDto, RefTerm>();
                map.CreateMap<RefTerm, RefTermToReturnDto>();

                map.CreateMap<User, UserReturnDto>();

            }));
        }

        public BookRepository GetDBContextupdate()
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
                Id= Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644"),
                UserName = "kathir",
                FirstName= "kathir",
                LastName = "M",
                Password = "Kathir@123"
            };

            context.Users.Add(userdata);
            context.SaveChanges();

            AddressBookDatabase addressbookdata = new AddressBookDatabase
            {
                Id = Guid.Parse("c137b1cd-c374-4da6-a098-fb06b0df03aa"),
                FirstName = "Kathir",
                LastName = "M",
                UserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644")
            };

            context.AddressBooks.Add(addressbookdata);

            Email emaildata = new Email
            {
                Id = Guid.Parse("dc57a504-d9a8-42a2-ad4c-83303f11e21a"),
                EmailAddress = "kathiresh@gmail.com",
                EmailTypeId = Guid.Parse("4e11a79e-3d5b-4ef2-907e-0deafc19085d"),
                AddressBookId = Guid.Parse("c137b1cd-c374-4da6-a098-fb06b0df03aa"),
                UserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644")
            };

            context.Emails.Add(emaildata);

            Address addressdata = new Address
            {
                Id = Guid.Parse("57c0d845-b430-4b68-a229-ec10397a6605"),
                Line1 = "massstreet",
                Line2 = "100 feet street",
                City = "Tiruppur",
                StateName = "TN",
                ZipCode = "641603",
                AddressTypeId = Guid.Parse("1287279b-2613-47ca-9bd2-9ed3b51cae86"),
                CountryTypeId = Guid.Parse("3aac0854-de67-4bee-8e95-b48e6bc76b17"),
                AddressBookId = Guid.Parse("c137b1cd-c374-4da6-a098-fb06b0df03aa"),
                UserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644")
            };

            context.Addresses.Add(addressdata);

            Phone phonedata = new Phone
            {
                Id = Guid.Parse("36d15017-5b28-4692-9e7a-a730bacda282"),
                PhoneNumber = "9877743212",
                PhoneTypeId = Guid.Parse("c059f6e4-8743-4209-a240-1f0961fa027d"),
                AddressBookId = Guid.Parse("c137b1cd-c374-4da6-a098-fb06b0df03aa"),
                UserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644")
            };

            context.Phones.Add(phonedata);

            Asset assetdata = new Asset
            {
                Id = Guid.Parse("cdacf151-8f59-445f-898a-a8e5fa0ecac9"),
                FileName = "sample.pdf",
                DownloadUrl = "https://localhost:7258/api/asset/04b112f1-d649-4a07-8de5-3ac77918c0fe",
                FileType = "application/pdf",
                Size = 30985,
                Content = "null"
            };

            context.Assets.Add(assetdata);

            context.RefSets.AddRange(new RefSet
            {
                Id = Guid.Parse("7c3b9028-3afc-4862-a058-91085f5c6217"),
                Set = "ADDRESS_TYPE",
                Description = "address details"
            },
            new RefSet{
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

            //context.RefSets.AddRange(refsetdata);

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
            new RefTerm{
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
                RefSetId = Guid.Parse("610f8eaa-fe3b-4124-9887-88768b424b8b"),
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
        /// Method to Test Get Address Book Count API
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void GetAddressBookCount_CheckUser_ReturnOkStatus()
        {
            IActionResult result = _controller.GetAddressBookCount();

            var finalresult = Assert.IsType<OkObjectResult>(result);
            CountDto countdata = finalresult.Value as CountDto;

            Assert.NotNull(result);
            Assert.Equal(1, countdata.Count);
        }

        /// <summary>
        /// Method to Test Delete Address Book API
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void DeleteAddressBook_CheckAddressBookId_ReturnOkStatus()
        {
            Guid ValidAddressBookId = Guid.Parse("c137b1cd-c374-4da6-a098-fb06b0df03aa");

            IActionResult result = _controller.DeleteAddressBook(ValidAddressBookId);

            var finalresult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal("AddressBook c137b1cd-c374-4da6-a098-fb06b0df03aa was deleted successfully", finalresult.Value);
        }

        /// <summary>
        /// Method to Test Delete Address Book API
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void DeleteAddressBook_CheckAddressBookId_ReturnNotFoundStatus()
        {
            Guid ValidAddressBookId = Guid.NewGuid();

            IActionResult result = _controller.DeleteAddressBook(ValidAddressBookId);

            var finaldata = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal("Address book not found.",finaldata.Value);
        }

        AddressBookResource getpagination = new AddressBookResource()
        {
            PageNumber = 1,
            PageSize = 1,
            SortBy = "FirstName",
            SortOrder = "ASC",
        };

        /// <summary>
        /// Method to Test Get All Address Book API
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void GetAllAddressBook_Valid_ReturnOkStatus()
        {
            IActionResult result = _controller.GetAddressBooks(getpagination);

            var resultvalue = Assert.IsType<OkObjectResult>(result);
            PagedList<AddressBookReturnDto> listresult = resultvalue.Value as PagedList<AddressBookReturnDto>;

            Assert.Equal(1, listresult.Count);
        }

        /// <summary>
        /// Method to Test Get An Address Book API
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void GetAnAddressBook_Valid_ReturnOkStatus()
        {
            Guid valid = Guid.Parse("c137b1cd-c374-4da6-a098-fb06b0df03aa");
                
            var result = _controller.GetAnAddressBook(valid);

            var finalresult = Assert.IsType<OkObjectResult>(result);
            AddressBookReturnDto addresslist = finalresult.Value as AddressBookReturnDto;

            Assert.Equal("Kathir", addresslist.FirstName);
        }

        /// <summary>
        /// Method to Test Get Address Book API
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void GetAnAddressBook_InValid_ReturnNotFoundStatus()
        {
            

            Guid valid = Guid.NewGuid();

            var result = _controller.GetAnAddressBook(valid);

            var finalresult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Not found data", finalresult.Value);
        }

        /// <summary>
        /// Method to Test Create Address Book API
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void CreateAddressBook_Valid_ReturnOkStatus()
        {
            var addressbookcreatedtofile = new AddressBookCreateDto();

            Keyrefence keyvalueemail = new Keyrefence()
            {
                Key = "personal"
            };

            Addresstype addresstype = new Addresstype()
            {
                Key = "home"
            };

            Countrytype countrytype = new Countrytype()
            {
                Key = "India"
            };

            Phonereference phoneref = new Phonereference()
            {
                Key = "workphone"
            };

            AddressBookCreateDto createdata = new AddressBookCreateDto()
            {
                FirstName = "Karthick",
                LastName = "L",
                Emails = new List<EmailDto>()
                {
                    new EmailDto()
                    {
                        EmailAddress = "shivan@gmail.com",
                        type = keyvalueemail
                    }
                },
                Addresses = new List<AddressDto>()
                {
                   new AddressDto()
                   {
                       Line1 = "shiva street",
                       Line2 = "70 feet road",
                       City = "cbe",
                       StateName = "TN",
                       ZipCode = "456777",
                       type = addresstype,
                       country = countrytype
                   }
                },
                Phones = new List<PhoneDto>()
                {
                    new PhoneDto()
                    {
                        PhoneNumber = "9557743212",
                        type = phoneref
                    }
                }
            };

            var result = _controller.CreateAddressBook(createdata);

            Assert.IsType<OkObjectResult>(result);
        }

        /// <summary>
        /// Method to Test Create Address Book API
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void CreateAddressBook_Valid_ReturnConflictStatus()
        {
           
            var addressbookcreatedtofile = new AddressBookCreateDto();

            Keyrefence keyvalueemail = new Keyrefence()
            {
                Key = "personal"
            };

            Addresstype addresstype = new Addresstype()
            {
                Key = "home"
            };

            Countrytype countrytype = new Countrytype()
            {
                Key = "India"
            };

            Phonereference phoneref = new Phonereference()
            {
                Key = "workphone"
            };

            AddressBookCreateDto createdata = new AddressBookCreateDto()
            {
                FirstName = "kathir",
                LastName = "M",
                Emails = new List<EmailDto>()
                {
                    new EmailDto()
                    {
                        EmailAddress = "shivan@gmail.com",

                        type = keyvalueemail
                    }
                },
                Addresses = new List<AddressDto>()
                {
                   new AddressDto()
                   {
                       Line1 = "shivan street",
                       Line2 = "70 feet road",
                       StateName = "TN",
                       ZipCode = "456777",
                       type = addresstype,
                       country = countrytype
                   }
                },
                Phones = new List<PhoneDto>()
                {
                    new PhoneDto()
                    {
                        PhoneNumber = "9557743212",
                        type = phoneref
                    }
                }
            };

            var result = _controller.CreateAddressBook(createdata);

            Assert.IsType<ConflictObjectResult>(result);

        }

        [Fact]
        public void CreateAddressBook_Valid_ReturnBadRequestStatus()
        {
            var addressbookcreatedtofile = new AddressBookCreateDto();

            Keyrefence keyvalueemail = new Keyrefence()
            {
                Key = "personal"
            };

            Addresstype addresstype = new Addresstype()
            {
                Key = "home"
            };

            Countrytype countrytype = new Countrytype()
            {
                Key = "India"
            };

            Phonereference phoneref = new Phonereference()
            {
                Key = "workphone"
            };

            AddressBookCreateDto createdata = new AddressBookCreateDto()
            {
                FirstName = "kathir",
                LastName = null,
                Emails = new List<EmailDto>()
                {
                    new EmailDto()
                    {
                        EmailAddress = "shivan@gmail.com",

                        type = keyvalueemail
                    }
                },
                Addresses = new List<AddressDto>()
                {
                   new AddressDto()
                   {
                       Line1 = "shivan street",
                       Line2 = "70 feet road",
                       StateName = "TN",
                       ZipCode = "456777",
                       type = addresstype,
                       country = countrytype
                   }
                },
                Phones = new List<PhoneDto>()
                {
                    new PhoneDto()
                    {
                        PhoneNumber = "9557743212",
                        type = phoneref
                    }
                }
            };

            var result = _controller.CreateAddressBook(createdata);

            Assert.IsType<BadRequestObjectResult>(result);

        }

        /// <summary>
        /// Method to Test Update Address Book API
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void UpdateAddressBook_Valid_ReturnOkStatus()
        {
            
            Keyrefence keyvalueemail = new Keyrefence()
            {
                Key = "personal"
            };

            Addresstype addresstype = new Addresstype()
            {
                Key = "home"
            };

            Countrytype countrytype = new Countrytype()
            {
                Key = "India"
            };

            Phonereference phoneref = new Phonereference()
            {
                Key = "workphone"
            };

            AddressBookUpdateDto updatadata = new AddressBookUpdateDto
            {
                FirstName = "kathiresh1202",
                LastName = "M",
                Emails = new List<EmailUpdationDto>()
                {
                    new EmailUpdationDto()
                    {
                        EmailAddress = "kathir1202@gmail.com",
                        type = keyvalueemail
                    }
                },
                Addresses = new List<AddressUpdationDto>()
                {
                    new AddressUpdationDto()
                    {
                        Line1 = "kathir street",
                        Line2 = "90 feet road",
                        City = "cbe",
                        StateName = "TN",
                        ZipCode = "456557",
                        type = addresstype,
                        country = countrytype
                    }
                },
                Phones = new List<PhoneUpdationDto>()
                {
                    new PhoneUpdationDto()
                    {
                        PhoneNumber = "9557711212",
                        type = phoneref
                    }
                },
                Asset = new AssetIdDto()
                {
                    FileId = Guid.Parse("cdacf151-8f59-445f-898a-a8e5fa0ecac9")
                }
            };

            Guid ValidAddressBookId = Guid.Parse("c137b1cd-c374-4da6-a098-fb06b0df03aa");

            var result = _controller.UpdateAddressBook(ValidAddressBookId , updatadata);

            Assert.NotNull(result);
            var finalresult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Address book updated successfully.", finalresult.Value);
        }
    }
}




