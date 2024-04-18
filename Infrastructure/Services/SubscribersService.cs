using Infrastructure.Entities;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics;
using System.Linq.Expressions;


namespace Infrastructure.Services;

public class SubscribersService
{
    private readonly SubscribersRepository _subscribersRepository;

    public SubscribersService(SubscribersRepository subscribersRepository)
    {
        _subscribersRepository = subscribersRepository;
    }

    /// <summary>
    ///     Creates a new subscriber async
    /// </summary>
    /// <param name="model">the model containing the info about the new subscriber</param>
    /// <returns>true if the subscriber was successfully created and doens't already exists, else false</returns>
    public async Task<bool> CreateSubscriberAsync(SubscribeModel model)
    {
        try
        {
            var exists = await _subscribersRepository.ExistsAsync(x => x.Email == model.Email);
            if (!exists)
            {
                var subscribersEntity = new SubscribersEntity { 
                    Email = model.Email,
                    DailyNewsletter = model.DailyNewsletter,
                    EventUpdates = model.EventUpdates,
                    AdvertisingUpdates = model.AdvertisingUpdates,
                    StartupsWeekly = model.StartupsWeekly,
                    WeekInReview = model.WeekInReview,
                    Podcasts = model.Podcasts,
                };
                var subscribeEntity = await _subscribersRepository.CreateOneAsync(subscribersEntity);
                if(subscribeEntity != null)
                {
                    return true;
                }
            }

        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return false;
    }

    /// <summary>
    ///     gets one subscriber async
    /// </summary>
    /// <param name="id">the id of the subscriber to be retrieved</param>
    /// <returns>the subscriber entity if a subscriber with the specified id exists, else null</returns>
    public async Task<SubscribersEntity> GetOneSubscriberAsync(int id)
    {
        try
        {
            var subscribersEntity = await _subscribersRepository.GetOneAsync(x => x.Id == id);
            if(subscribersEntity != null)
            {
                return subscribersEntity;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return null!;
    }

    /// <summary>
    ///     gets all subscribers async
    /// </summary>
    /// <returns>an IEnumerable of subscribersEntity containing all subscribers in database, else a empty list</returns>
    public async Task<IEnumerable<SubscribersEntity>> GetAllSubscribersAsync()
    {
        try
        {
            var subscriberEntity = await _subscribersRepository.GetAllAsync();
            if(subscriberEntity != null)
            {
                return subscriberEntity.ToHashSet();
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return [];
    }


    public async Task<bool> UpdateSubscriberAsync(SubscribersEntity entity)
    {
        try
        {
            var existing = await _subscribersRepository.ExistsAsync(x => x.Id == entity.Id);
            if(existing)
            {
                await _subscribersRepository.UpdateAsync(x => x.Id == entity.Id, entity);
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return false;
    }

    /// <summary>
    ///     deletes a subscriber async
    /// </summary>
    /// <param name="predicate">the predicate that filters wich subscriber to be deleted</param>
    /// <returns>true if deleted successfully, else false</returns>
    public async Task<bool> DeleteSubscriberAsync(Expression<Func<SubscribersEntity, bool>> predicate)
    {
        try
        {
            var result = await _subscribersRepository.DeleteAsync(predicate);
            if(result)
            {
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return false;
    }

}
