using Infrastructure.Entities;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Linq.Expressions;
using WebApi.Filters;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[UseApiKey] 
public class SubscribersController : ControllerBase
{
    private readonly SubscribersService _subscribersService;

    public SubscribersController(SubscribersService subscribersService)
    {
        _subscribersService = subscribersService;
    }


    #region CREATE SUBSCRIBER
    [HttpPost]
    public async Task<IActionResult> CreateSubscriber(SubscribeModel model)
    {
        try
        {
            if(ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Email))
                {
                    //var subscriberEntity = new SubscribersEntity
                    //{
                    //    Email = model.Email,
                    //    DailyNewsletter = model.DailyNewsletter,
                    //    EventUpdates = model.EventUpdates,
                    //    AdvertisingUpdates = model.AdvertisingUpdates,
                    //    StartupsWeekly = model.StartupsWeekly,
                    //    WeekInReview = model.WeekInReview,
                    //    Podcasts = model.Podcasts,
                    //};
                    var result = await _subscribersService.CreateSubscriberAsync(model);
                    if (result)
                    {
                        return Created();
                    }

                }
                    return Conflict("You are already a subscriber");
            }
                return BadRequest();
        }
        catch(Exception ex) { Debug.Write(ex); }
        return Problem();
    }
    #endregion


    #region GET ALL SUBSCRIBERS
    [HttpGet]
    public async Task<IActionResult> GetAllSubscribers()
    {
        try
        {
            if(ModelState.IsValid)
            {
                var subscribers = await _subscribersService.GetAllSubscribersAsync();
                if (subscribers != null)
                {
                    return Ok(subscribers);
                }
                return NotFound();
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return BadRequest();
    }
    #endregion


    #region GET ONE SUBSCRIBER
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOneSubscriber(int id)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var subscribersEntity = await _subscribersService.GetOneSubscriberAsync(id);
                if (subscribersEntity != null)
                {
                    return Ok(subscribersEntity);
                }
            }
            return NotFound($"No user found with id {id}");
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return BadRequest();
    }
    #endregion


    #region UPDATE SUBSCRIBER
    //[HttpPut("{id}")]
    //public async Task<IActionResult> Update(int id, string email)
    //{
    //    try
    //    {
    //        if(ModelState.IsValid) 
    //        {
    //            var subscriberEntity = await _subscribersService.GetOneSubscriberAsync(id);
    //            if (subscriberEntity != null)
    //            {
    //                var existingSubscriber = await _subscribersService.GetOneSubscriberAsync(id);
    //                if(existingSubscriber != null)
    //                {
    //                    return Conflict("Subscriber already exists");
    //                }
    //                subscriberEntity.Email = email;
    //                var result = await _subscribersService.UpdateSubscriberAsync(subscriberEntity);
    //                if (result)
    //                {
    //                    return Ok();
    //                }
    //                return NotFound();
    //            }
    //        }
    //        return BadRequest();
    //    }
    //    catch (Exception ex) { Debug.WriteLine(ex); }
    //    return Problem();
    //}
    #endregion


    #region DELETE SUBSCRIPTION
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOne(int id)
    {
        try
        {
            if(ModelState.IsValid)
            {
                var subscribeEntity = await _subscribersService.GetOneSubscriberAsync(id);
                if (subscribeEntity != null)
                {
                    var result = await _subscribersService.DeleteSubscriberAsync(x => x.Id == id);
                    if (result)
                    {
                        return Ok("Successfully deleted subscription");
                    }
                }
            }
            return NotFound();
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return BadRequest();
    }
    #endregion

}


//try
//{

//}
//catch (Exception ex) { Debug.WriteLine(ex); }