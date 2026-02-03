using ITUMiniTwit.Core;
using ITUMiniTwit.Core.Models;
using ITUMiniTwit.Infrastructure.ITUMiniTwit.Repositories;

namespace ITUMiniTwit.Infrastructure.ITUMiniTwit.Service
{
    public class CheepService : ICheepService
    {
        private readonly ICheepRepository _cheepRepository;
        

        public CheepService(ICheepRepository cheepRepository)
        {
            _cheepRepository = cheepRepository;

        }

        public List<CheepDto> GetCheeps(int page, int pageSize)
        {
            return _cheepRepository.GetCheeps(page, pageSize);
        }

        public List<CheepDto> GetCheepsFromAuthor(string author, int page, int pageSize)
        {
            return _cheepRepository.GetCheepsFromAuthor(author, page, pageSize);
        }
        
        public void AddCheep(string authorUserName, string text)
        {
            _cheepRepository.AddCheep(new CheepDto
            {
                Author = authorUserName,
                Text = text
            });
        }
        
        public void LikeCheep(string authorUserName, int cheepId)
        {
            _cheepRepository.LikeCheep(authorUserName, cheepId);
        }

    }
    
}