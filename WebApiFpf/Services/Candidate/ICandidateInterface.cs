using WebApiFpf.Dto.Candidate;
using WebApiFpf.Models.Entities;

namespace WebApiFpf.Services.Candidate
{
    public interface ICandidateInterface
    {
        Task<ResponseModel<List<CandidateModel>>> GetCandidates();
        Task<ResponseModel<CandidateModel>> GetCandidateById(int idCandidate);
        Task<ResponseModel<List<CandidateModel>>> CreateCandidate(CreateCandidateDto createCandidateDto);
        Task<ResponseModel<List<CandidateModel>>> UpdateCandidate(UpdateCandidateDto updateCandidateDto);
        Task<ResponseModel<List<CandidateModel>>> DeleteCandidate(int idCandidate);


    }
}
