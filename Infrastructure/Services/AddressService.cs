using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class AddressService
{
    private readonly AddressRepository _addressRepository;

    public AddressService(AddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    /// <summary>
    ///     Creates a new address async
    /// </summary>
    /// <param name="entity">The addressentity to create</param>
    /// <returns>True if creating an address was successfull, else false</returns>
    public async Task<bool> CreateAddressAsync(AddressEntity entity)
    {
        try
        {
            var addressEntity = await _addressRepository.CreateOneAsync(entity);
            if (addressEntity != null)
            {
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return false;
    }

    /// <summary>
    ///     Gets one address based on provided userId async
    /// </summary>
    /// <param name="userId">The userId whose address is being retrieved</param>
    /// <returns>The address asssociated with the specific userId, else null</returns>
    public async Task<AddressEntity> GetAddressAsync(string userId)
    {
        try
        {
            var addressEntity = await _addressRepository.GetOneAsync(x => x.UserId == userId);
            if(addressEntity != null)
            {
                return addressEntity;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return null!;
    }

    /// <summary>
    ///     Gets all addresses async
    /// </summary>
    /// <returns>A list of all addresses, else null</returns>
    public virtual async Task<IEnumerable<AddressEntity>> GetAllAsync()
    {
        try
        {
            var addressEntity = await _addressRepository.GetAllAsync();
            if(addressEntity != null)
            {
                return addressEntity.ToHashSet();
                //var addressList = new HashSet<AddressEntity>();
                //foreach(var address in addressEntity)
                //{
                //    addressList.Add(address);
                //}
                //return addressList;
            }
        }
        catch(Exception ex) { Debug.WriteLine(ex); }
        return null!;
    }


    /// <summary>
    ///     Updates an address in db async
    /// </summary>
    /// <param name="entity">The entity to be updated</param>
    /// <returns>True if update was successfull, else false</returns>
    public async Task<bool> UpdateAddressAsync(AddressEntity entity)
    {
        try
        {
            var existing = await _addressRepository.ExistsAsync(x => x.UserId ==  entity.UserId);
            if(existing)
            {
                await _addressRepository.UpdateAsync(x => x.UserId == entity.UserId, entity);
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return false;
    }

    /// <summary>
    ///     Deletes an address entity async based on the provided predicate
    /// </summary>
    /// <param name="predicate">The predicate used to filter the address entities</param>
    /// <returns>True if address was deleted successfully, else false</returns>
    public virtual async Task<bool> DeleteAddressAsync(Expression<Func<AddressEntity, bool>> predicate)
    {
        try
        {
            var result = await _addressRepository.DeleteAsync(predicate);
            if(result)
            {
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return false;
    }
}
