using DAL.Repositories;

namespace Vaultify.Features.SecretKeyF;

public class SecretKeyService
{
    private SecretKeyRepository secretKeyRepository;
    private UserResitory userResitory;
    
    public SecretKeyService(SecretKeyRepository secretKeyRepository, UserResitory userResitory)
    {   
        this.secretKeyRepository = secretKeyRepository;
        this.userResitory = userResitory;
    }

}