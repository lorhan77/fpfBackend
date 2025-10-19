using WebApiFpf.Models.Entities.Enums;

namespace WebApiFpf.Models.Entities
{
    public class CandidateSkillModel
    {
        public int CandidateId { get; set; }
        public int SkillId { get; set; }
        public Levels Level { get; set; }

        public CandidateModel Candidate { get; set; } = null!;
        public SkillModel Skill { get; set; } = null!;
    }
}
