using Xunit;
using AutoFixture;
using Moq;
using FluentAssertions;
using Contract;
using Microsoft.Extensions.Logging;
using AutoMapper;
using AddressBookAssignment.Controllers;
using Entities.Profiles;
using Microsoft.AspNetCore.Mvc;
using Entities.Dto;

namespace AddressBookXUnittest
{
    public class UnitTest1
    {
        private readonly IFixture _fixture;
        private readonly Mock<IAddressBookService> _serviceMock;
        private readonly AddressBookController _cnt;
        private readonly Mock<ILogger<AddressBookController>> _mocklogger;
        private readonly IMapper _mapper;

        public UnitTest1()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IAddressBookService>>();
        }

        /*[Fact]
        public void GetAnAddressBook_IsValidUserId_ReturnAddressBook()
        {
            var _mocklog = new Mock<ILogger<AddressBookController>>();
            var addressbookcontroller = new AddressBookController(_serviceMock.Object, _mapper, _mocklog.Object);
            Guid validId = Guid.Parse("d3913ce0-5643-4b6a-bc8b-e8a520ab5f6a");
            Guid Isvalid = Guid.NewGuid();
            ActionResult<AddressBookDto> validResult = addressbookcontroller.GetAnAddressBook(validId);
            ActionResult<AddressBookDto> notFound = addressbookcontroller.GetAnAddressBook(Isvalid);
            
            
            OkObjectResult item = validResult.Result as OkObjectResult;
            AddressBookDto bookItem = item.Value as AddressBookDto;

            Assert.IsType<NotFoundObjectResult>(notFound.Result);
            Assert.IsType<OkObjectResult>(validResult.Result);
            Assert.IsType<AddressBookDto>(item.Value);
            Assert.Equal(validId, bookItem.Id);
            Assert.Equal("abc", bookItem.FirstName);
        }

        [Fact]
        public void DeleteAddressBook_Test()
        {
            var _mocklog = new Mock<ILogger<AddressBookController>>();
            var addressbookcontroller = new AddressBookController(_serviceMock.Object, _mapper, _mocklog.Object);
            Guid validId = Guid.Parse("42b06592-2622-4531-8a8e-ab9800ef7581");
            Guid Isvalid = Guid.NewGuid();
            var data = addressbookcontroller.DeleteAddressBook(validId);

            Assert.IsType<OkResult>(data);

        }*/

    }
}