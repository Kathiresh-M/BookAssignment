using AutoMapper;
using Contract;
using Entities.RequestDto;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace AddressBookAssignment.Controllers
{
    [ApiController]
    [Authorize]
    public class AddressBookController : ControllerBase
    {
        private readonly IAddressBookService _addressBookService;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressBookController> _logger;
        private readonly ILog _log;

        public AddressBookController(IAddressBookService addressBookService, IMapper mapper, ILogger<AddressBookController> logger)
        {
            _addressBookService = addressBookService;
            _mapper = mapper;
            _logger = logger;
            _log = LogManager.GetLogger(typeof(AddressBookController));
        }

        /// <summary>
        /// Method to create an address book
        /// </summary>
        /// <param name="addressBookData">address book data to be created</param>
        /// <returns>Id of the address book created</returns>
        [HttpPost]
        [Route("api/account")]
        public IActionResult CreateAddressBook([FromBody] AddressBookCreateDto addressBookData)
        {
            _log.Info("Create Address Book ");

            Guid tokenUserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644");

            _log.Info("Get UserId");
            //Guid tokenUserId;
            //var isValidToken = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

            try
                {

                _log.Info("Sent data to AddressBook data" + addressBookData + "" + tokenUserId);
                var response = _addressBookService.CreateAddressBook(addressBookData, tokenUserId);

                if (!response.IsSuccess && response.Message.Contains("already exists"))
                    {
                        _log.Error("Data Conflict");
                        return Conflict(response.Message);
                    }
                
                    _log.Info("Address Book was created" + response.AddressBook.Id);
                    return Ok($"Address book created with ID: {response.AddressBook.Id}");
                }
                catch (Exception ex)
                {
                    _log.Debug("Address was not found");
                    return NotFound("Not found exception please check your code" + ex);
                }
        }

        /// <summary>
        /// Method to get a particular address book
        /// </summary>
        /// <param name="addressBookId">Address Book Id</param>
        /// <returns>an address book</returns>
        [HttpGet]
        [Route("api/account/{addressBookId}")]
        public ActionResult GetAnAddressBook(Guid addressBookId)
        {
            _log.Info("Get An AddressBook"+addressBookId);

            Guid tokenUserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644");

            //Guid tokenUserId;
            //var isValidToken = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

                if (addressBookId == null || addressBookId == Guid.Empty)
                {
                    _log.Error("Trying to access address book with not a valid address book id by user: " + tokenUserId);
                    return NotFound("Not a valid address book ID.");
                }

                try
                {
                    _log.Info("Sent data to get AddressBook method");
                    var response = _addressBookService.GetAddressBook(addressBookId, tokenUserId);

                if (!response.IsSuccess)
                {
                    _log.Info("Get An AddressBook : Not Found");
                    return NotFound("Not found data");
                }

                    _log.Info("Get an AddressBook Successfully "+response.addressBook);
                    return Ok(response.addressBook);
                }

                catch (Exception ex)
                {
                    _log.Debug("Address was not found");
                    return BadRequest("Not found exception please check your code" + ex);
                }
        }

        /// <summary>
        /// Method to get a list of address book
        /// </summary>
        /// <param name="resourceParameter">query paramters to get pagination and sorting data</param>
        /// <returns>list of address books</returns>
        [HttpGet]
        [Route("api/account")]
        public IActionResult GetAddressBooks([FromQuery] AddressBookResource resourceParameter)
        {
            _log.Info("Start Address Book ");

            Guid tokenUserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644");

            //Guid tokenUserId;
            //var isValidToken = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);
           
            var addressBooksToReturn = _addressBookService.GetAddressBooks(tokenUserId, resourceParameter);

            var previousPageLink = addressBooksToReturn.HasPrevious ? CreateUri(resourceParameter, UriType.PreviousPage) : null;
            var nextPageLink = addressBooksToReturn.HasNext ? CreateUri(resourceParameter, UriType.NextPage) : null;

            var metaData = new
            {
                totalCount = addressBooksToReturn.TotalCount,
                pageSize = addressBooksToReturn.PageSize,
                currentPage = addressBooksToReturn.CurrentPage,
                totalPages = addressBooksToReturn.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            //Response.Headers.Add("Pagination", JsonSerializer.Serialize(metaData));

            _log.Info("Get All Address Book");
            return Ok(addressBooksToReturn);

        }

        /// <summary>
        /// Method to update an address book
        /// </summary>
        /// <param name="addressBookId">Id of the address book in Database</param>
        /// <param name="addressBook">address book data to be updated</param>
        /// <returns>Return Updated Successful Message</returns>
        [HttpPut]
        [Route("api/account/{addressBookId}")]
        public IActionResult UpdateAddressBook(Guid addressBookId, [FromBody] AddressBookUpdateDto addressBookData)
        {
            _log.Info("Start to Update AddressBook");

            if (!ModelState.IsValid)
            {
                _log.Error("Invalid addressbook details used.");
                return BadRequest("Enter valid addressbook data");
            }
            Guid tokenUserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644");
            //Guid tokenUserId;
            //var isValidToken = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

                var response = _addressBookService.UpdateAddressBook(addressBookData, addressBookId, tokenUserId);

                if (!response.IsSuccess && response.Message.Contains("Additional") || response.Message.Contains("duplication") || response.Message.Contains("not valid"))
                {
                    _log.Error("Given Data is Conflict");
                    return Conflict(response.Message);
                }

            if (!response.IsSuccess && response.Message.Contains("not found"))
            {
                _log.Error("Address was not found");
                return NotFound(response.Message);
            }
            _log.Info("Address was updated successfully");
            return Ok("Address book updated successfully.");
        }

        /// <summary>
        /// Method to get number of address books
        /// </summary>
        /// <returns>Count of the address books</returns>
        [HttpGet]
        [Route("api/account/count")]
        public IActionResult GetAddressBookCount()
        {
            _log.Info("Get Address Book Count start");

            Guid tokenUserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644");

            //Invalid data
            //Guid tokenUserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab664d");

            //Guid tokenUserId;
            //var isValidToken = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);
            try
            {
                var response = _addressBookService.GetCount(tokenUserId);

                _log.Info("Get Address Book Count");
                return Ok(response.Count);
            }
            catch(Exception ex)
            {
                _log.Error("User Not found"+ex);
                return NotFound("user not found"+ex);
            }
        }

        /// <summary>
        /// Method to delete an address book
        /// </summary>
        /// <param name="addressBookId">Id of the address book</param>
        /// <returns>Return Delete Successful Message</returns>
        [HttpDelete]
        [Route("api/account/{AddressBookId}")]
        public IActionResult DeleteAddressBook(Guid addressBookId)
        {
            Guid tokenUserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644");

            //Guid tokenUserId;
            //var isValidToken = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);
            try
            {
                var addressBookResponseData = _addressBookService.GetAddressBook(addressBookId, tokenUserId);

                if (!addressBookResponseData.IsSuccess || addressBookResponseData.Message.Contains("not") || addressBookResponseData.Message.Contains("User"))
                {
                    _log.Info($"Address book with Id: {addressBookId}, does not exists");
                    return NotFound("Address book not found.");
                }

                var deleteResponse = _addressBookService.DeleteAddressBook(addressBookId, tokenUserId);

                

                _log.Info("Address was deleted successfully");
                //return Ok(addressBookResponseData.addressBook);
                return Ok("AddressBook " + addressBookId + " was deleted successfully");
            }
            catch(Exception ex)
            {
                return NotFound("Data is not Found");
            }
        }

        /// <summary>
        /// Method to create link
        /// </summary>
        /// <param name="resourceParameter">Pagiantiona and sorting data</param>
        /// <param name="uriType">type of the uri</param>
        /// <returns>uri or null</returns>
        private string CreateUri(AddressBookResource resourceParameter, UriType uriType)
        {
            switch (uriType)
            {
                case UriType.PreviousPage:
                    return Url.Link("GetAddressBooks", new
                    {
                        pageNumber = resourceParameter.PageNumber - 1,
                        pageSize = resourceParameter.PageSize,
                        sortBy = resourceParameter.SortBy,
                        sortOrder = resourceParameter.SortOrder,
                    });
                case UriType.NextPage:
                    return Url.Link("GetAddressBooks", new
                    {
                        pageNumber = resourceParameter.PageNumber + 1,
                        pageSize = resourceParameter.PageSize,
                        sortBy = resourceParameter.SortBy,
                        sortOrder = resourceParameter.SortOrder,
                    });
                default:
                    return Url.Link("GetAddressBooks", new
                    {
                        pageNumber = resourceParameter.PageNumber,
                        pageSize = resourceParameter.PageSize,
                        sortBy = resourceParameter.SortBy,
                        sortOrder = resourceParameter.SortOrder,
                    });
            }
        }
    }
}
