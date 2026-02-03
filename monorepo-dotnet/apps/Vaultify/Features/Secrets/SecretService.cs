using Core.Contexts;
using DAL;
using DAL.Dtos;
using DAL.Entities;
using DAL.Extensions;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using SharedDto.Dtos;
using SharedDto.Mappers;
using Vaultify.Core.Mappers;
using Vaultify.Features.Secrets.Dtos.Requests;
using Vaultify.Features.Secrets.Dtos.Response;


namespace Vaultify.Features.Secrets;

public class SecretService(
    SecretRepository secretRepository,
    PasswordManagerDbContext context,
    UserContext userContext,
    ILogger<SecretService> logger
    )
{
    private readonly SecretRepository _secretRepository = secretRepository;
    private readonly PasswordManagerDbContext _context = context;
    private readonly UserContext _userContext = userContext;
    private readonly ILogger<SecretService> _logger = logger;

    public async Task CreateSecret(SecretRequestCreateDto payload)
    {
        var userId = _userContext.GetUserIdOrThrow();
        await _secretRepository.AddAsync(payload.ToSecretEntity(userId));
    }

    public async Task<PageableDto<SecretResponseListDto>> ListSecrets(PaginationDto pagination, string? search)
    {
        var userId = _userContext.GetUserIdOrThrow();
        _logger.LogInformation("[ListSecrets] UserId: {UserId}", userId);

        IQueryable<Secret> query = _context.Secret
            .AsNoTracking()
            .Where((s) => s.UserId == userId && s.Active)
            .OrderByDescending(s => s.CreatedAt);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(s => s.Title.ToLower().Contains(search.ToLower()));

        return await query.WithPagination(s => s.ToSecretReponseListDto(), pagination);
    }

    public async Task<SecretResponseDto> GetSecret(Guid secretId)
    {
        var userId = _userContext.GetUserIdOrThrow();
        var secret = await GetUserSecret(userId, secretId);

        return secret.ToSecretResponseDto();
    }

    public async Task<SecretResponseUpdateDto> UpdateSecret(Guid secretId, SecretRequestUpdateDto payload, CancellationToken cancellationToken)
    {
        //TODO Criar validação condicional
        var secret = await GetUserSecret(_userContext.UserId, secretId);

        // secret.ToSecretRequestUpdateDto(payload);

        await _context.SaveChangesAsync(cancellationToken);
        return new SecretResponseUpdateDto(secret.Title, secret.Username, "*****");
    }


    private async Task<Secret?> GetUserSecret(Guid? userId, Guid secretId)
    {
        if (userId == null) throw new UnauthorizedAccessException("User is not authenticated!");
        return await _secretRepository.GetSecretByIdAndUserId(userId.Value, secretId) ?? throw new InvalidDataException("Secret not found!");
    }
};
