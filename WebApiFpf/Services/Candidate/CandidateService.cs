using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApiFpf.Data;
using WebApiFpf.Dto.Candidate;
using WebApiFpf.Models.Entities;

namespace WebApiFpf.Services.Candidate
{
    public class CandidateService : ICandidateInterface
    {
        private readonly AppDbContext _context;
        public CandidateService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<List<CandidateModel>>> CreateCandidate(CreateCandidateDto createCandidateDto)
        {
            ResponseModel <List<CandidateModel>> response = new ResponseModel<List<CandidateModel>>();

            try
            {
                var emailExists = await _context.Candidates.AnyAsync(c => c.Email.ToLower() == createCandidateDto.Email.Trim().ToLower());
                if (emailExists)
                {
                    response.Mensagem = "Email já cadastrado.";
                    response.Status = false;
                    return response;
                }

                var candidate = new CandidateModel()
                {
                    FullName = createCandidateDto.FullName,
                    Email = createCandidateDto.Email,
                    CreatedAt = DateTime.UtcNow
                };

               _context.Candidates.Add(candidate);
                await  _context.SaveChangesAsync();

                response.Dados = await _context.Candidates.ToListAsync();
                response.Mensagem = "Candidato criado com sucesso.";    

                return response;
            }
            catch(Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<CandidateModel>>> DeleteCandidate(int idCandidate)
        {
            ResponseModel<List<CandidateModel>> response = new ResponseModel<List<CandidateModel>>();
            try
            {
                var candidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Id == idCandidate);

                if(candidate == null)
                {
                    response.Mensagem = "Nenhum candidato localizado!";
                    return response;
                }

                _context.Remove(candidate);
                await _context.SaveChangesAsync();

                response.Dados = await _context.Candidates.ToListAsync();
                response.Mensagem = "Candidato removido com sucesso.";

                return response;
            }
            catch(Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<CandidateModel>> GetCandidateById(int idCandidate)
        {
            ResponseModel<CandidateModel> response = new ResponseModel<CandidateModel>();
            try
            {
                var candidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Id == idCandidate);
                if (candidate == null)
                {
                    response.Mensagem = "Nenhum registro localizado!";
                    return response;
                }

                response.Dados = candidate;
                response.Mensagem = "Candidato retornado com sucesso.";

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<CandidateModel>>> GetCandidates()
        {
            ResponseModel<List<CandidateModel>> response = new ResponseModel<List<CandidateModel>>();
            try
            {
                var candidates = await _context.Candidates.ToListAsync();

                response.Dados = candidates;
                response.Mensagem = "Todos os candidatos retornados com sucesso.";

                return response;    
            }
            catch(Exception ex) 
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<CandidateModel>>> UpdateCandidate(UpdateCandidateDto updateCandidateDto)
        {
            ResponseModel<List<CandidateModel>> response = new ResponseModel<List<CandidateModel>>();

            try
            {
                var candidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Id == updateCandidateDto.Id);

                if (candidate == null)
                {
                    response.Mensagem = "Nenhum registro localizado!";
                    return response;
                }

                candidate.FullName = updateCandidateDto.FullName;
                candidate.Email = updateCandidateDto.Email;

                _context.Candidates.Update(candidate);
                await _context.SaveChangesAsync();

                response.Dados = await _context.Candidates.ToListAsync();
                response.Mensagem = "Candidato atualizado com sucesso.";

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }
    }
}
