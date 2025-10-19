using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiFpf.Dto.Candidate;
using WebApiFpf.Models.Entities;
using WebApiFpf.Services.Candidate;

namespace WebApiFpf.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateInterface _candidateInterface;
        public CandidateController(ICandidateInterface candidateInterface)
        {
            _candidateInterface = candidateInterface;
        }

        [HttpGet("Candidates")]
        public async Task<ActionResult<ResponseModel<List<CandidateModel>>>> GetCandidates()
        {
            var response = await _candidateInterface.GetCandidates();
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("Candidates/{id}")]
        public async Task<ActionResult<ResponseModel<CandidateModel>>> GetCandidateById(int id)
        {
            var response = await _candidateInterface.GetCandidateById(id);
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("Candidates")]
        public async Task<ActionResult<ResponseModel<List<CandidateModel>>>> CreateCandidate(CreateCandidateDto createCandidateDto)
        {
            var response = await _candidateInterface.CreateCandidate(createCandidateDto);
            return Ok(response);
        }

        [HttpPut("Candidates")]
        public async Task<ActionResult<ResponseModel<List<CandidateModel>>>> UpdateCandidate(UpdateCandidateDto updateCandidateDto)
        {
            var response = await _candidateInterface.UpdateCandidate(updateCandidateDto);
            return Ok(response);
        }

        [HttpDelete("Candidates/{id}")]
        public async Task<ActionResult<ResponseModel<List<CandidateModel>>>> DeleteCandidate(int id)
        {
            var response = await _candidateInterface.DeleteCandidate(id);
            return Ok(response);
        }


    }
}
