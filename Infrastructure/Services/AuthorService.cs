//using Infrastructure.Entities;
//using Infrastructure.Repositories;
//using System.Diagnostics;

//namespace Infrastructure.Services;

//public class AuthorService
//{
//    private readonly AuthorRepository _authorRepository;

//    public AuthorService(AuthorRepository authorRepository)
//    {
//        _authorRepository = authorRepository;
//    }


//    public async Task<AuthorsEntity> CreateAuthorAsync(AuthorsEntity entity)
//    {
//        try
//        {
//            var result = await _authorRepository.CreateOneAsync(entity);
//            if(result != null)
//            {
//                return result;
//            }
//        }
//        catch (Exception ex) { Debug.WriteLine(ex); }
//        return null!;
//    }

//    public async Task<AuthorsEntity> AuthorExisting(AuthorsEntity entity)
//    {
//        try
//        {
//            var existingAuthor = await _authorRepository.ExistsAsync(x => x.AuthorName == entity.AuthorName);
//            if (existingAuthor)
//            {
//                // Returnera befintlig författare om den hittas
//                return entity;
//            }

//            // Skapa en ny författare om inte en befintlig hittas
//            var newAuthor = await _authorRepository.CreateOneAsync(new AuthorsEntity
//            {
//                Title = entity.Title,
//                AuthorName = entity.AuthorName,
//                AuthorImageUrl = entity.AuthorImageUrl,
//                YoutubeSubs = entity.YoutubeSubs,
//                FacebookSubs = entity.FacebookSubs,
//            });
//            return newAuthor;
//        }
//        catch (Exception ex) { Debug.WriteLine(ex); }
//            return null!;
//    }
//}
