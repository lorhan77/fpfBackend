using System.Text.Json.Serialization;

namespace WebApiFpf.Models.Entities
{
    public class CandidateModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public ICollection <CandidateSkillModel> CandidateSkills { get; set; } = new List<CandidateSkillModel>();
    }
}
