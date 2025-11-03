using AutoMapper;
using MyInsurancePortal.DtoModels;
using MyInsurancePortal.Exceptions;
using MyInsurancePortal.Interfaces;
using MyInsurancePortal.Models;
using Microsoft.EntityFrameworkCore;

namespace MyInsurancePortal.Repository
{
    public class ClaimServiceRepo : IClaimService
    {
        private readonly InsuranceDbContext _context;
        private readonly ILogger<ClaimServiceRepo> _logger;
        private readonly IMapper _mapper;

        public ClaimServiceRepo(InsuranceDbContext context, ILogger<ClaimServiceRepo> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<ClaimDto>> GetAllClaims()
        {
            var claims = await _context.Claims.ToListAsync();

            if (!claims.Any())
                throw new KeyNotFoundException(ClaimExceptions.NoClaimsFound);

            return _mapper.Map<List<ClaimDto>>(claims);
        }

        public async Task<ClaimDto?> GetClaimByClaimNumber(string claimNumber)
        {
            var claim = await _context.Claims
                .FirstOrDefaultAsync(x => x.ClaimNumber == claimNumber);

            if (claim == null)
                throw new KeyNotFoundException(ClaimExceptions.ClaimNotFound);

            return _mapper.Map<ClaimDto>(claim);
        }

        public async Task<List<ClaimDto>> GetClaimsByPolicyNumber(string policyNumber)
        {
            var claims = await _context.Claims
                .Include(c => c.Policy)
                .Where(c => c.Policy.PolicyNumber == policyNumber)
                .ToListAsync();

            if (!claims.Any())
                throw new KeyNotFoundException(ClaimExceptions.NoClaimsFound);

            return _mapper.Map<List<ClaimDto>>(claims);
        }

        public async Task<ClaimDto> AddNewClaim(ClaimDto claimDto)
        {
            var claim = _mapper.Map<Claim>(claimDto);

            var existingClaim = await _context.Claims
                .FirstOrDefaultAsync(x => x.ClaimNumber == claim.ClaimNumber);

            if (existingClaim != null)
                throw new InvalidOperationException(ClaimExceptions.ClaimAlreadyExist);

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            return _mapper.Map<ClaimDto>(claim);
        }

        public async Task<ClaimDto> UpdateClaim(string claimNumber, ClaimDto claimDto)
        {
            var existingClaim = await _context.Claims
                .FirstOrDefaultAsync(x => x.ClaimNumber == claimNumber);

            if (existingClaim == null)
                throw new KeyNotFoundException(ClaimExceptions.ClaimNotFound);

            _mapper.Map(claimDto, existingClaim);
            await _context.SaveChangesAsync();

            return _mapper.Map<ClaimDto>(existingClaim);
        }

        public async Task<bool> DeleteClaim(string claimNumber)
        {
            var existingClaim = await _context.Claims
                .FirstOrDefaultAsync(c => c.ClaimNumber == claimNumber);

            if (existingClaim == null)
                throw new KeyNotFoundException(ClaimExceptions.ClaimNotFound);

            _context.Claims.Remove(existingClaim);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
