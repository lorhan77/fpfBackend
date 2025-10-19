namespace WebApiFpf.Models.Entities
{
    public class SkillModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection <CandidateSkillModel>  CandidateSkills { get; set; } = new List<CandidateSkillModel>();
    }
}
