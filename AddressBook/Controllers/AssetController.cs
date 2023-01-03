using AutoMapper;
using Contract;
using Contract.Response;
using Entities;
using Entities.Dto;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AddressBookAssignment.Controllers
{
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly IAssetService _assetService;
        private readonly IMapper _mapper;

        private readonly ILog _log;

        public AssetController(IAssetService assetService, 
             IMapper mapper)
        {
            _assetService = assetService;

            _log = LogManager.GetLogger(typeof(AssetController));
            _mapper = mapper;
        }

        /// <summary>
        /// Method to upload an asset
        /// </summary>
        /// <param name="addressBookId">Address Book Id</param>
        /// <param name="file">asset file</param>
        /// <returns>Return Upload file details</returns>
        [HttpPost]
        [Authorize]
        [Route("api/asset/{addressBookId}")]
        public IActionResult UploadAsset(Guid addressBookId,[FromForm] IFormFile file)
        {
            _log.Info("Upload file");

            Guid tokenUserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644");

            if (addressBookId == null || addressBookId == Guid.Empty)
            {
                _log.Error("Trying to update address book data with not a valid addressbook Id by user: " + tokenUserId);
                return BadRequest("Not a valid address book ID.");
            }

            try
            {
                var asset = new Asset();
                asset.Id = Guid.NewGuid();
                asset.DownloadUrl = GenerateDownloadUrl(addressBookId);

                AssetResponse response = _assetService.AddAsset(addressBookId, tokenUserId, asset, file);

                if (!response.IsSuccess && response.Message.Contains("not found"))
                {
                    return NotFound(response.Message);
                }

                if (!response.IsSuccess && response.Message.Contains("exists"))
                {
                    return Conflict(response.Message);
                }

                var assetToReturn = _mapper.Map<AssetReturnDto>(response.Asset);

                return Ok(assetToReturn);
            }
            catch(NullReferenceException ex)
            {
                _log.Error("Null Reference exception");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method to upload an asset
        /// </summary>
        /// <param name="Id">Id of the Asset</param>
        /// <returns>Return asset file</returns>
        [HttpGet]
        [Authorize]
        [Route("api/asset/{Id}")]
        public IActionResult DownloadAsset(Guid Id)
        {
            Guid tokenUseId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644");
            //Guid tokenUserId;
            //var isValidToken = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

            if (Id == null || Id == Guid.Empty)
            {
                _log.Error("Trying to access asset data with not a valid user id by user: " + tokenUseId);
                return BadRequest("Not a valid user ID.");
            }

            AssetResponse response = _assetService.GetAsset(Id, tokenUseId);

            if (!response.IsSuccess)
            {
                return NotFound(response.Message);
            }

            byte[] bytes = Convert.FromBase64String(response.Asset.Content);

            return File(bytes, response.Asset.FileType, response.Asset.FileName);
        }

        /// <summary>
        /// Method to Generate Download Url
        /// </summary>
        /// <param name="Id">Id of the Asset</param>
        /// <returns>Return asset file</returns>
        private string GenerateDownloadUrl(Guid addressBookId)
        {
            return ("https://localhost:7258/api/asset/"+ addressBookId);
        }

    }

}
