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
using AddressBookAssignment.Memoryconnectionfactory;

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

            Asset assetdata = new Asset
            {
                Id = Guid.Parse("cdacf151-8f59-445f-898a-a8e5fa0ecac9"),
                FileName = "sample.pdf",
                DownloadUrl = "https://localhost:7258/api/asset/04b112f1-d649-4a07-8de5-3ac77918c0fe",
                FileType = "application/pdf",
                Size = 30985,
            };

            context.Assets.Add(assetdata);
            context.SaveChanges();

            return context;
        }

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

            Assert.IsType<OkObjectResult>(result);

        }

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

            Assert.IsType<NotFoundObjectResult>(result);

        }

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

            var servicefile = assetcontrollerfile.UploadAsset(ValidAssetId, file);

            Assert.IsType<OkObjectResult>(servicefile);

        }

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

            Assert.IsType<NotFoundObjectResult>(servicefile);

        }
    }
}
