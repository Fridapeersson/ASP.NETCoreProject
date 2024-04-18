using Infrastructure.Entities;
using Infrastructure.Models.Dtos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Infrastructure.Services;

public class CategoryService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public CategoryService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    public async Task<IEnumerable<CategoryEntity>> GetCategoriesAsync(/*string category = "", string searchQuery=""*/)
    {
        try
        {
            var response = await _http.GetAsync(_config["ApiUris:Categories"]);
            if (response.IsSuccessStatusCode)
            {
                var categories = JsonConvert.DeserializeObject<IEnumerable<CategoryEntity>>(await response.Content.ReadAsStringAsync());

                if (categories != null)
                {
                    return categories;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return null!;

    }
}
