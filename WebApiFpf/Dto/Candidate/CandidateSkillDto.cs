using WebApiFpf.Models.Entities.Enums;

namespace WebApiFpf.Dto.Candidate
{
    public class CandidateSkillDto
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public Levels Level { get; set; }
    }
}