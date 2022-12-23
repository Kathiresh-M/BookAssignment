using AutoMapper;
using Contract;
using Entities;
using Entities.Dto;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace AddressBookAssignment.Controllers
{
    [ApiController]
    [Authorize]
    public class MetaDataController : ControllerBase
    {
        private readonly IRefSetService _refSetService;
        private readonly IRefTermService _refTermService;

        private readonly ILog _log;
        private readonly IMapper _mapper;

        public MetaDataController(IRefSetService refSetService,
            IRefTermService refTermService, IMapper mapper)
        {
            _refSetService = refSetService;
            _refTermService = refTermService;
            _mapper = mapper;
            _log = LogManager.GetLogger(typeof(MetaDataController));
        }

        //Refset controllers start
        /// <summary>
        /// Method to get refernce set
        /// </summary>
        /// <param name="Id">Id of reference set</param>
        /// <returns>refernce set data</returns>
        [HttpGet]
        [Route("api/metadata/refsetdetails/{id}")]
        public IActionResult GetRefSet(Guid Id)
        {
            _log.Info("Get RefSet ");

            Guid tokenUserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644");
            //Guid tokenUserId;
            //Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

            if (Id == null || Id == Guid.Empty)
            {
                _log.Info("Invalid ref set id was given in GetRef API by user Id: " + tokenUserId);
                return NotFound("Invalid ref set id");
            }

            try
            {
                _log.Info("Get RefSet By Id");
                var response = _refSetService.GetRefSetById(Id);
               
                var refSetToReturn = _mapper.Map<RefSetToReturnDto>(response.RefSet);

                return Ok(refSetToReturn);
            }
            catch (Exception ex)
            {
                _log.Error("Not Found Exception check you code");
                return NotFound("Not found exception please check your code" + ex);
            }
        }

        /// <summary>
        /// Method to create reference set
        /// </summary>
        /// <param name="refSetData">reference set data</param>
        /// <returns>refernce set data with Id</returns>
        [HttpPost]
        [Route("api/metadata/refset")]
        public IActionResult AddRefSet([FromBody] RefSetCreationDto refSetData)
        {
            if (!ModelState.IsValid)
            {
                _log.Error("Invalid refSet data given in AddRefSet API controller.");
                return BadRequest("The refsetData field is required");
            }

            try
            {
                var refSet = _mapper.Map<RefSet>(refSetData);

                var response = _refSetService.CreateRefSet(refSet);

                if (!response.IsSuccess)
                {
                    return Conflict(new { message = $"Set {refSetData.Set} already exists." });
                }

                var refSetToReturn = _mapper.Map<RefSetToReturnDto>(response.RefSet);

                //return CreatedAtRoute("GetRefSet", new { Id = refSetToReturn.Id }, refSetToReturn);
                CreatedAtRoute("GetRefSet", new { Id = refSetToReturn.Id }, refSetToReturn);

                return Ok(refSetToReturn);
            }
            catch (Exception ex)
            {
                return NotFound("Not found exception please check your code" + ex);
            }
        }

        /// <summary>
        /// Method to delete reference set
        /// </summary>
        /// <param name="Id">reference set Id</param>
        /// <returns>no content</returns>
        [HttpDelete]
        [Route("api/metadata/refset/{Id}")]
        public IActionResult DeleteRefSet(Guid Id)
        {
            Guid tokenUserId;
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

            if (Id == null || Id == Guid.Empty)
            {
                _log.Info("Invalid ref set id was given in GetRef API by user Id: " + tokenUserId);
                return BadRequest("Invalid ref set id");
            }

            var response = _refSetService.DeleteRefSetById(Id);
            if (!response.IsSuccess)
            {
                _log.Info($"RefSet with Id: {Id}, does not exists.");
                return NotFound("RefSet does not exists.");
            }

            return NoContent();
        }
        //Refset controllers end

        //RefTerm controller starts

        /// <summary>
        /// Method to get reference term
        /// </summary>
        /// <param name="Id">reference term Id</param>
        /// <returns>refernce term data with Id</returns>
        [HttpGet]
        [Route("api/metadata/refterm/{Id}")]
        public IActionResult GetRefTerm(Guid Id)
        {
            Guid tokenUserId;
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

            if (Id == null || Id == Guid.Empty)
            {
                _log.Info("Invalid ref set id was given in GetRef API by user Id: " + tokenUserId);
                return BadRequest("Invalid ref set id");
            }

            var response = _refTermService.GetRefTermById(Id);
            if (!response.IsSuccess)
            {
                _log.Info($"Refterm with Id: {Id} does not exists.");
                return NoContent();
            }

            var refTermToReturn = _mapper.Map<RefTermToReturnDto>(response.RefTerm);

            return Ok(refTermToReturn);
        }

        /// <summary>
        /// Method to create reference term
        /// </summary>
        /// <param name="refSetId">reference set Id</param>
        /// <param name="refTermData">refernce term data to create</param>
        /// <returns>refernce term data with Id</returns>
        [HttpPost]
        [Route("api/metadata/refterm/{refsetId}")]
        public IActionResult AddRefTerm(Guid refSetId, [FromBody] RefTermCreationDto refTermData)
        {
            Guid tokenUserId;
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

            if (!ModelState.IsValid)
            {
                _log.Info("Invalid refterm data given in AddRefSet API controller by user ");
                return BadRequest("Invalid Ref Term data.");
            }

            var refTerm = _mapper.Map<RefTerm>(refTermData);

            var response = _refTermService.CreateRefTerm(refTerm, refSetId);

            if (!response.IsSuccess && response.Message.Contains("Refset"))
            {
                _log.Info($"RefSet with Id: {refSetId} does not exists.");
                return NotFound("RefSet for given Id was not found.");
            }

            if (!response.IsSuccess && response.Message.Contains("RefTerm"))
            {
                _log.Info($"user with Id: {tokenUserId}, trying to insert existing term ${refTermData.Key}.");
                return Conflict(new { message = $"Key {refTermData.Key} already exists." });
            }

            var refTermToReturn = _mapper.Map<RefTermToReturnDto>(response.RefTerm);

            _refTermService.AddRefTermMapping(refTermToReturn.Id, refSetId);

            return CreatedAtRoute("GetRefSet", new { Id = refTermToReturn.Id }, refTermToReturn);
        }
        //Refterm controllers end

        //get all refterms under a ref set
        /// <summary>
        /// Method to get list of reference term under a refernce set
        /// </summary>
        /// <param name="refSetId">reference set Id</param>
        /// <returns>list of reference terms</returns>
        [HttpGet]
        [Route("refset/all/{refsetId}")]
        public IActionResult GetAllRefTerm(Guid refSetId)
        {
            Guid tokenUserId;
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

            if (refSetId == null || refSetId == Guid.Empty)
            {
                _log.Info("Invalid ref set id was given in GetAllRefTerm API by user Id: " + tokenUserId);
                return BadRequest("Invalid ref set id");
            }

            var response = _refSetService.GetRefTermsByRefSetId(refSetId);

            if (!response.IsSuccess && response.Message.Contains("Refset"))
            {
                _log.Info($"RefSet with Id: {refSetId} does not exists.");
                return NotFound("RefSet does not exists.");
            }

            if (!response.IsSuccess && response.Message.Contains("RefTerms"))
            {
                _log.Info($"RefTerms with RefSet Id: {refSetId} does not contain any RefTerms.");
                return NoContent();
            }

            var refSetToReturn = _mapper.Map<IEnumerable<RefTermToReturnDto>>(response.RefTerms);

            return Ok(refSetToReturn);
        }

        /// <summary>
        /// Method to get list of reference term under a refernce set
        /// </summary>
        /// <param name="reftermkey">reference set Id</param>
        /// <returns>return key reference</returns>
        [HttpGet]
        [Route("api/metadata/refset/{key}")]
        public IActionResult Mappingdata(string key)
        {
            _log.Info("MetaData Key Reference start");

            //Guid tokenUserId = Guid.Parse("e4229706-9a92-4dfa-8bad-82c88aab6644");

            //Guid tokenUserId;
            //Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out tokenUserId);

            try
            {
                var response = _refSetService.Metadata(key);

                if (response.Id == Guid.Empty)
                {
                    _log.Error("Key is not found");
                    return NotFound("Key not found");
                }

                _log.Info("MetaData Key Reference is executed successfully");
                return Ok(response);
            }
            catch(FormatException ex)
            {
                _log.Error("InValid Data formate"+ex);
                throw new Exception("Given Data is Not Correct Formate"+ex);
            }
        }
        
    }
}
