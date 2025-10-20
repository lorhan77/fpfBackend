using WebApiFpf.Models.Entities.Enums;

namespace WebApiFpf.Dto.Candidate
{
    public class AssignSkillDto
    {
        public int SkillId { get; set; }
        public Levels Level { get; set; }
    }
}