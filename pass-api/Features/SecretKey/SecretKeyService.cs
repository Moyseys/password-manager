using PasswordManager.DAL.Repositories;

namespace PasswordManager.Features.SecretKey;

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