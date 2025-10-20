using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebApiFpf.Dto.Candidate;
using WebApiFpf.Dto.Skill;
using WebApiFpf.Models.Entities;
using WebApiFpf.Services.Skill;

namespace WebApiFpf.Controllers
{
    [Route("api/")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;
        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpPost("Skills")]
        public async Task<ActionResult<ResponseModel<List<SkillModel>>>> CreateSkill(CreateSkillDto createSkillDto)
        {
            var response = await _skillService.CreateSkill(createSkillDto);
            return Ok(response);
        }

        [HttpGet("Skills")]
        public async Task<ActionResult<ResponseModel<List<SkillModel>>>> GetSkills()
        {
            var response = await _skillService.GetSkills();
            return Ok(response);
        }

        [HttpPost("Candidates/{id}/Skills")]
        public async Task<ActionResult<ResponseModel<CandidateModel>>> AssignSkillToCandidate(int id, [FromBody] JsonElement payload)
        {
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new JsonStringEnumConverter());

            List<AssignSkillDto> assignSkillDtos;
            try
            {
                if (payload.ValueKind == JsonValueKind.Array)
                {
                    assignSkillDtos = JsonSerializer.Deserialize<List<AssignSkillDto>>(payload.GetRawText(), options)!;
                }
                else
                {
                    var single = JsonSerializer.Deserialize<AssignSkillDto>(payload.GetRawText(), options)!;
                    assignSkillDtos = new List<AssignSkillDto> { single };
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<CandidateModel> { Status = false, Mensagem = "Payload inválido: " + ex.Message });
            }

            var response = await _skillService.AssignSkillsToCandidate(assignSkillDtos, id);
            return Ok(response);
        }

        [HttpGet("Candidates/{id}/Skills")]
        public async Task<ActionResult<ResponseModel<List<CandidateSkillDto>>>> GetCandidateSkills(int id)
        {
            var response = await _skillService.GetCandidateSkills(id);
            return Ok(response);
        }
    }
}
