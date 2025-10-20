using Microsoft.EntityFrameworkCore;
using WebApiFpf.Data;
using WebApiFpf.Dto.Candidate;
using WebApiFpf.Dto.Skill;
using WebApiFpf.Models.Entities;

namespace WebApiFpf.Services.Skill
{
    public class SkillService : ISkillService
    {
        private readonly AppDbContext _context;
        public SkillService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<List<SkillModel>>> CreateSkill(CreateSkillDto createSkillDto)
        {
            ResponseModel<List<SkillModel>> response = new ResponseModel<List<SkillModel>>();
            try
            {
                var nameNormalized = createSkillDto.Name?.Trim();
                if (string.IsNullOrWhiteSpace(nameNormalized))
                {
                    response.Mensagem = "Nome da habilidade inválido.";
                    response.Status = false;
                    return response;
                }

                var exists = await _context.Skills.AnyAsync(s => s.Name.ToLower() == nameNormalized.ToLower());
                if (exists)
                {
                    response.Mensagem = "Habilidade já existe.";
                    response.Status = false;
                    return response;
                }

                var skill = new SkillModel
                {
                    Name = nameNormalized
                };

                _context.Skills.Add(skill);
                await _context.SaveChangesAsync();

                response.Dados = await _context.Skills.ToListAsync();
                response.Mensagem = "Habilidade criada com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<SkillModel>>> GetSkills()
        {
            ResponseModel<List<SkillModel>> response = new ResponseModel<List<SkillModel>>();
            try
            {
                response.Dados = await _context.Skills.ToListAsync();
                response.Mensagem = "Habilidades retornadas com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<CandidateModel>> AssignSkillToCandidate(AssignSkillDto assignSkillDto, int candidateId)
        {
            ResponseModel<CandidateModel> response = new ResponseModel<CandidateModel>();
            try
            {
                var candidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Id == candidateId);
                if (candidate == null)
                {
                    response.Mensagem = "Candidato não encontrado.";
                    response.Status = false;
                    return response;
                }

                var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == assignSkillDto.SkillId);
                if (skill == null)
                {
                    response.Mensagem = "Habilidade não encontrada.";
                    response.Status = false;
                    return response;
                }

                var already = await _context.CandidateSkills.AnyAsync(cs => cs.CandidateId == candidateId && cs.SkillId == assignSkillDto.SkillId);
                if (already)
                {
                    response.Mensagem = "O candidato já possui essa habilidade.";
                    response.Status = false;
                    return response;
                }

                var candidateSkill = new CandidateSkillModel
                {
                    CandidateId = candidateId,
                    SkillId = assignSkillDto.SkillId,
                    Level = assignSkillDto.Level
                };

                _context.CandidateSkills.Add(candidateSkill);
                await _context.SaveChangesAsync();

                response.Dados = candidate;
                response.Mensagem = "Habilidade atribuída ao candidato com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<CandidateSkillDto>>> GetCandidateSkills(int candidateId)
        {
            ResponseModel<List<CandidateSkillDto>> response = new ResponseModel<List<CandidateSkillDto>>();
            try
            {
                var candidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Id == candidateId);
                if (candidate == null)
                {
                    response.Mensagem = "Candidato não encontrado.";
                    response.Status = false;
                    return response;
                }

                var skills = await _context.CandidateSkills
                    .Where(cs => cs.CandidateId == candidateId)
                    .Include(cs => cs.Skill)
                    .Select(cs => new CandidateSkillDto
                    {
                        SkillId = cs.SkillId,
                        SkillName = cs.Skill.Name,
                        Level = cs.Level
                    })
                    .ToListAsync();

                response.Dados = skills;
                response.Mensagem = "Habilidades do candidato retornadas com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<CandidateModel>> AssignSkillsToCandidate(List<AssignSkillDto> assignSkillDtos, int candidateId)
        {
            ResponseModel<CandidateModel> response = new ResponseModel<CandidateModel>();
            try
            {
                if (assignSkillDtos == null || !assignSkillDtos.Any())
                {
                    response.Mensagem = "Nenhuma habilidade informada.";
                    response.Status = false;
                    return response;
                }

                var candidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Id == candidateId);
                if (candidate == null)
                {
                    response.Mensagem = "Candidato não encontrado.";
                    response.Status = false;
                    return response;
                }

                var skillIds = assignSkillDtos.Select(s => s.SkillId).Distinct().ToList();

                var skillsFound = await _context.Skills.Where(s => skillIds.Contains(s.Id)).Select(s => s.Id).ToListAsync();
                var missing = skillIds.Except(skillsFound).ToList();
                if (missing.Any())
                {
                    response.Mensagem = $"As seguintes skills não foram encontradas: {string.Join(',', missing)}";
                    response.Status = false;
                    return response;
                }

               
                var existing = await _context.CandidateSkills
                    .Where(cs => cs.CandidateId == candidateId && skillIds.Contains(cs.SkillId))
                    .Select(cs => cs.SkillId)
                    .ToListAsync();

                var toAddDtos = assignSkillDtos
                    .Where(a => !existing.Contains(a.SkillId))
                    .GroupBy(a => a.SkillId) 
                    .Select(g => g.First())
                    .ToList();

                if (!toAddDtos.Any())
                {
                    response.Dados = candidate;
                    response.Mensagem = "Nenhuma nova habilidade para adicionar. As skills já estavam atribuídas.";
                    return response;
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var newEntries = toAddDtos.Select(a => new CandidateSkillModel
                    {
                        CandidateId = candidateId,
                        SkillId = a.SkillId,
                        Level = a.Level
                    }).ToList();

                    _context.CandidateSkills.AddRange(newEntries);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    response.Dados = candidate;
                    response.Mensagem = $"{newEntries.Count} habilidade(s) atribuída(s) ao candidato. {existing.Count} já existiam.";
                    return response;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
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