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

    }
}