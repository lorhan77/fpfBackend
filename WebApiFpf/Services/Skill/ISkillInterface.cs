using WebApiFpf.Dto.Candidate;
using WebApiFpf.Dto.Skill;
using WebApiFpf.Models.Entities;

namespace WebApiFpf.Services.Skill
{
    public interface ISkillService
    {
        Task<ResponseModel<List<SkillModel>>> CreateSkill(CreateSkillDto createSkillDto);
        Task<ResponseModel<List<SkillModel>>> GetSkills();

        Task<ResponseModel<CandidateModel>> AssignSkillToCandidate(AssignSkillDto assignSkillDto, int candidateId);
        Task<ResponseModel<List<CandidateSkillDto>>> GetCandidateSkills(int candidateId);

        Task<ResponseModel<CandidateModel>> AssignSkillsToCandidate(List<AssignSkillDto> assignSkillDtos, int candidateId);
    }
}