namespace lfiApi.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository {get;}
        IMessageRepository MessageRepository {get;}
        ILikesRepository LikesRepository {get;}
        IDonationRepository DonationRepository {get;}
        Task<bool> Complete();
        bool HasChanges();
    }
}