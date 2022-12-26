using AutoMapper;
using Entities.Dto;
using Entities.RequestDto;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository;
using Services;
using Services.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using AddressBookAssignment.Controllers;
using Microsoft.AspNetCore.Http;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AddressBookXUnittest
{
    public class AssetTests
    {
        private readonly IMapper _mapper;
        private IConfiguration config;

        public AssetTests()
        {
            _mapper = new Mapper(new MapperConfiguration(map =>
            {
                map.CreateMap<AssetRequestDto, Asset>();
                map.CreateMap<Asset, AssetReturnDto>();

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
            context.SaveChanges();

            AddressBookDatabase addressbookdata = new AddressBookDatabase
            {
                Id = Guid.Parse("04b112f1-d649-4a07-8de5-3ac77918c0fe"),
                FirstName = "Kathir",
                LastName = "M",
                UserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644")
            };

            context.AddressBooks.Add(addressbookdata);
            context.SaveChanges();

            Email emaildata = new Email
            {
                Id = Guid.Parse("4628e6a9-4a1e-42a9-b7b5-e734e1b679e4"),
                EmailAddress = "kathiresh@gmail.com",
                EmailTypeId = Guid.Parse("4e11a79e-3d5b-4ef2-907e-0deafc19085d"),
                AddressBookId = Guid.Parse("04b112f1-d649-4a07-8de5-3ac77918c0fe"),
                UserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644")
            };

            context.Emails.Add(emaildata);
            context.SaveChanges();

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
            context.SaveChanges();

            Phone phonedata = new Phone
            {
                Id = Guid.Parse("a70fcbb6-4f76-4dd3-8158-18cceecc4fa0"),
                PhoneNumber = "9877743212",
                PhoneTypeId = Guid.Parse("c059f6e4-8743-4209-a240-1f0961fa027d"),
                AddressBookId = Guid.Parse("04b112f1-d649-4a07-8de5-3ac77918c0fe"),
                UserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644")
            };

            context.Phones.Add(phonedata);
            context.SaveChanges();

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

            //context.RefSets.AddRange(refsetdata);
            context.SaveChanges();

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

            context.SaveChanges();

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

            Asset assetdata = new Asset
            {
                Id = Guid.Parse("cdacf151-8f59-445f-898a-a8e5fa0ecac9"),
                FileName = "sample.pdf",
                DownloadUrl = "https://localhost:7258/api/asset/04b112f1-d649-4a07-8de5-3ac77918c0fe",
                FileType = "application/pdf",
                Size = 30985,
                Content = "JVBERi0xLjcNCiW1tbW1DQoxIDAgb2JqDQo8PC9UeXBlL0NhdGFsb2cvUGFnZXMgMiAwIFIvTGFuZyhlbi1JTikgL1N0cnVjdFRyZWVSb290IDEwIDAgUi9NYXJrSW5mbzw8L01hcmtlZCB0cnVlPj4vTWV0YWRhdGEgMjAgMCBSL1ZpZXdlclByZWZlcmVuY2VzIDIxIDAgUj4+DQplbmRvYmoNCjIgMCBvYmoNCjw8L1R5cGUvUGFnZXMvQ291bnQgMS9LaWRzWyAzIDAgUl0gPj4NCmVuZG9iag0KMyAwIG9iag0KPDwvVHlwZS9QYWdlL1BhcmVudCAyIDAgUi9SZXNvdXJjZXM8PC9Gb250PDwvRjEgNSAwIFI+Pi9FeHRHU3RhdGU8PC9HUzcgNyAwIFIvR1M4IDggMCBSPj4vUHJvY1NldFsvUERGL1RleHQvSW1hZ2VCL0ltYWdlQy9JbWFnZUldID4+L01lZGlhQm94WyAwIDAgNTk1LjMyIDg0MS45Ml0gL0NvbnRlbnRzIDQgMCBSL0dyb3VwPDwvVHlwZS9Hcm91cC9TL1RyYW5zcGFyZW5jeS9DUy9EZXZpY2VSR0I+Pi9UYWJzL1MvU3RydWN0UGFyZW50cyAwPj4NCmVuZG9iag0KNCAwIG9iag0KPDwvRmlsdGVyL0ZsYXRlRGVjb2RlL0xlbmd0aCAxOTY+Pg0Kc3RyZWFtDQp4nK2OvQ6CMACE9yZ9hxtbE0pbQNqEMPCj0ehgwDAYB2KQBfD3/SOoi7u33eVy98EtrvWAKHK36SqDdDf10II1g7MveBwjyVLcKJFCTjImVJAIbCA8DeMrYTXuDSXVDAMlSUmJu1BQSkgf5ZmSqS2hEGohtY8wsMI3KPuxtyxCtI9xGu3bma9bUnJgVcMdzboTV5pduBOwns9ZA+547MntJ0LddfyIck1JPh7vKPkDqPKs8H5A33xfLPz+Id+meAHxiUFSDQplbmRzdHJlYW0NCmVuZG9iag0KNSAwIG9iag0KPDwvVHlwZS9Gb250L1N1YnR5cGUvVHJ1ZVR5cGUvTmFtZS9GMS9CYXNlRm9udC9CQ0RFRUUrQ2FsaWJyaS9FbmNvZGluZy9XaW5BbnNpRW5jb2RpbmcvRm9udERlc2NyaXB0b3IgNiAwIFIvRmlyc3RDaGFyIDMyL0xhc3RDaGFy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bS9wZGYvMS4zLyI+CjxwZGY6UHJvZHVjZXI+TWljcm9zb2Z0wq4gV29yZCAyMDIxPC9wZGY6UHJvZHVjZXI+PC9yZGY6RGVzY3JpcHRpb24+CjxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiICB4bWxuczpkYz0iaHR0cDovL3B1cmwub3JnL2RjL2VsZW1lbnRzLzEuMS8iPgo8ZGM6Y3JlYXRvcj48cmRmOlNlcT48cmRmOmxpPmthdGhpcmVzaDEyMDJAb3V0bG9vay5jb208L3JkZjpsaT48L3JkZjpTZXE+PC9kYzpjcmVhdG9yPjwvcmRmOkRlc2NyaXB0aW9uPgo8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiAgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIj4KPHhtcDpDcmVhdG9yVG9vbD5NaWNyb3NvZnTCriBXb3JkIDIwMjE8L3htcDpDcmVhdG9yVG9vbD48eG1wOkNyZWF0ZURhdGU+MjAyMi0xMi0xNlQxMjowNjoxMCswNTozMDwveG1wOkNyZWF0ZURhdGU+PHhtcDpNb2RpZnlEYXRlPjIwMjItMTItMTZUMTI6MDY6MTArMDU6MzA8L3htcDpNb2RpZnlEYXRlPjwvcmRmOkRlc2NyaXB0aW9uPgo8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiAgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iPgo8eG1wTU06RG9jdW1lbnRJRD51dWlkOkNGOTIwQTkzLUNBQTctNEFDMS05NjZFLUU1RUYzQ0RGMEY5MjwveG1wTU06RG9jdW1lbnRJRD48eG1wTU06SW5zdGFuY2VJRD51dWlkOkNGOTIwQTkzLUNBQTctNEFDMS05NjZFLUU1RUYzQ0RGMEY5MjwveG1wTU06SW5zdGFuY2VJRD48L3JkZjpEZXNjcmlwdGlvbj4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCjwvcmRmOlJERj48L3g6eG1wbWV0YT48P3hwYWNrZXQgZW5kPSJ3Ij8+DQplbmRzdHJlYW0NCmVuZG9iag0KMjEgMCBvYmoNCjw8L0Rpc3BsYXlEb2NUaXRsZSB0cnVlPj4NCmVuZG9iag0KMjIgMCBvYmoNCjw8L1R5cGUvWFJlZi9TaXplIDIyL1dbIDEgNCAyXSAvUm9vdCAxIDAgUi9JbmZvIDkgMCBSL0lEWzw5MzBBOTJDRkE3Q0FDMTRBOTY2RUU1RUYzQ0RGMEY5Mj48OTMwQTkyQ0ZBN0NBQzE0QTk2NkVFNUVGM0NERjBGOTI+XSAvRmlsdGVyL0ZsYXRlRGVjb2RlL0xlbmd0aCA4NT4+DQpzdHJlYW0NCnicY2AAgv//GYGkIAMDiFoGoe6BKcY3YIrpF5hiXgSmWCZCqGMQ6jdQHqydCUIxQygWCMUKoRghFFQlG1Af60uwdvZSMMXhAaYyAsBUCdARAF7rDLQNCmVuZHN0cmVhbQ0KZW5kb2JqDQp4cmVmDQowIDIzDQowMDAwMDAwMDEwIDY1NTM1IGYNCjAwMDAwMDAwMTcgMDAwMDAgbg0KMDAwMDAwMDE2NiAwMDAwMCBuDQowMDAwMDAwMjIyIDAwMDAwIG4NCjAwMDAwMDA0OTIgMDAwMDAgbg0KMDAwMDAwMDc2MiAwMDAwMCBuDQowMDAwMDAwOTMwIDAwMDAwIG4NCjAwMDAwMDExNjkgMDAwMDAgbg0KMDAwMDAwMTIyMiAwMDAwMCBuDQowMDAwMDAxMjc1IDAwMDAwIG4NCjAwMDAwMDAwMTEgNjU1MzUgZg0KMDAwMDAwMDAxMiA2NTUzNSBmDQowMDAwMDAwMDEzIDY1NTM1IGYNCjAwMDAwMDAwMTQgNjU1MzUgZg0KMDAwMDAwMDAxNSA2NTUzNSBmDQowMDAwMDAwMDE2IDY1NTM1IGYNCjAwMDAwMDAwMTcgNjU1MzUgZg0KMDAwMDAwMDAwMCA2NTUzNSBmDQowMDAwMDAxOTA5IDAwMDAwIG4NCjAwMDAwMDIxMjAgMDAwMDAgbg0KMDAwMDAyNjcwNCAwMDAwMCBuDQowMDAwMDI5ODYyIDAwMDAwIG4NCjAwMDAwMjk5MDcgMDAwMDAgbg0KdHJhaWxlcg0KPDwvU2l6ZSAyMy9Sb290IDEgMCBSL0luZm8gOSAwIFIvSURbPDkzMEE5MkNGQTdDQUMxNEE5NjZFRTVFRjNDREYwRjkyPjw5MzBBOTJDRkE3Q0FDMTRBOTY2RUU1RUYzQ0RGMEY5Mj5dID4+DQpzdGFydHhyZWYNCjMwMTkxDQolJUVPRg0KeHJlZg0KMCAwDQp0cmFpbGVyDQo8PC9TaXplIDIzL1Jvb3QgMSAwIFIvSW5mbyA5IDAgUi9JRFs8OTMwQTkyQ0ZBN0NBQzE0QTk2NkVFNUVGM0NERjBGOTI+PDkzMEE5MkNGQTdDQUMxNEE5NjZFRTVFRjNDREYwRjkyPl0gL1ByZXYgMzAxOTEvWFJlZlN0bSAyOTkwNz4+DQpzdGFydHhyZWYNCjMwODA3DQolJUVPRg==",
                UserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644")
            };

            context.Assets.Add(assetdata);
            context.SaveChanges();

            return context;
        }

        /// <summary>
        /// Method to Test Download File API
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void DownloadFile_ValidId_ReturnOkStatus()
        {
            var DbContextConnection = GetDBContext();

            var assetrepository = new AssetRepo(DbContextConnection);
            var assdressbookrepository = new AddressBookRepo(DbContextConnection);
            var assetservice = new AssetService(assetrepository, assdressbookrepository);

            var assetcontrollerfile = new AssetController(assetservice, _mapper);

            Guid validdownloadfileId = Guid.Parse("cdacf151-8f59-445f-898a-a8e5fa0ecac9");

            var result = assetcontrollerfile.DownloadAsset(validdownloadfileId);

            var finalresult = Assert.IsType<OkObjectResult>(result);
            FileContentResult filedata = finalresult.Value as FileContentResult;

            Assert.Equal("sample.pdf", filedata.FileDownloadName);

        }

        /// <summary>
        /// Method to Test Download File API
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void DownloadFile_ValidId_ReturnNotFound()
        {
            var DbContextConnection = GetDBContext();

            var assetrepository = new AssetRepo(DbContextConnection);
            var assdressbookrepository = new AddressBookRepo(DbContextConnection);
            var assetservice = new AssetService(assetrepository, assdressbookrepository);

            var assetcontrollerfile = new AssetController(assetservice, _mapper);

            Guid validdownloadfileId = Guid.Parse("d8717962-8a03-4d7f-8442-794d8d114300");

            var result = assetcontrollerfile.DownloadAsset(validdownloadfileId);

            var finalresult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal("Asset not found", finalresult.Value);
        }

        /// <summary>
        /// Method to Test Upload File API
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void UplaodFile_ValidId_ReturnFileDetails()
        {
            var DbContextConnection = GetDBContext();

            var assetrepository = new AssetRepo(DbContextConnection);
            var addressbookrepository = new AddressBookRepo(DbContextConnection);

           // var userrepository = new UserRepo(DbContextConnection);
            //var passwordhas = new Password();

            var assetservice = new AssetService(assetrepository, addressbookrepository);

            var assetcontrollerfile = new AssetController(assetservice, _mapper);

            Guid ValidAssetId = Guid.Parse("04b112f1-d649-4a07-8de5-3ac77918c0fe");
            IFormFile file = A.Fake<IFormFile>();

            var result = assetcontrollerfile.UploadAsset(ValidAssetId, file);

            var finalresult = Assert.IsType<OkObjectResult>(result);

            AssetReturnDto filedata = finalresult.Value as AssetReturnDto;

            Assert.Equal("https://localhost:7258/api/asset/04b112f1-d649-4a07-8de5-3ac77918c0fe", filedata.DownloadURL);
        }

        /// <summary>
        /// Method to Test Upload File API
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void UplaodFile_ValidId_ReturnInValid()
        {
            var DbContextConnection = GetDBContext();

            var assetrepository = new AssetRepo(DbContextConnection);
            var addressbookrepository = new AddressBookRepo(DbContextConnection);

            // var userrepository = new UserRepo(DbContextConnection);
            //var passwordhas = new Password();

            var assetservice = new AssetService(assetrepository, addressbookrepository);

            var assetcontrollerfile = new AssetController(assetservice, _mapper);

            Guid InValidAssetId = Guid.Parse("04b112f1-d649-4a07-8de5-3ac77918c000");

            IFormFile file = A.Fake<IFormFile>();

            var servicefile = assetcontrollerfile.UploadAsset(InValidAssetId, file);

            var finalresult = Assert.IsType<NotFoundObjectResult>(servicefile);

            Assert.Equal("AddressBook not found", finalresult.Value);
        }
    }
}
