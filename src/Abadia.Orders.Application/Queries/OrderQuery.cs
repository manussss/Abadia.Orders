using Abadia.Orders.Application.Queries.Interfaces;
using Abadia.Orders.Application.Queries.ViewModels;
using Abadia.Orders.Infra.Scripts;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace Abadia.Orders.Application.Queries;

public class OrderQuery : IOrderQuery
{
    private readonly IConfiguration _configuration;
    //private readonly IMemoryCache _cache;
    //private MemoryCacheEntryOptions _cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(30));
    private readonly IDistributedCache _cache;

    public OrderQuery(IConfiguration configuration, IDistributedCache cache)
    {
        _configuration = configuration;
        _cache = cache;
    }

    public async Task<IEnumerable<OrdersDetailsViewModel>> GetOrdersByContractId(string contractId)
    {
        IEnumerable<OrdersDetailsViewModel> viewModel = new List<OrdersDetailsViewModel>();

        //var cacheKey = $"Orders-{contractId}";
        //if (!_cache.TryGetValue<IEnumerable<OrdersDetailsViewModel>>(cacheKey, out viewModel))
        //{
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@Id", contractId, DbType.String);

        //    using (var connection = new SqlConnection(_configuration.GetConnectionString("OrderConnection")))
        //    {
        //        viewModel = await connection.QueryAsync<OrdersDetailsViewModel>(
        //            OrderScripts.GetOrdersDetailsByContractId,
        //            parameters);
        //    }

        //    _cache.Set(cacheKey, viewModel, _cacheOptions);
        //}

        var cacheKey = $"Orders-{contractId}";
        var json = await _cache.GetStringAsync(cacheKey);

        if (json != null)
        {
            viewModel = JsonSerializer.Deserialize<IEnumerable<OrdersDetailsViewModel>>(json);
        }
        else
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", contractId, DbType.String);

            using (var connection = new SqlConnection(_configuration.GetConnectionString("OrderConnection")))
            {
                viewModel = await connection.QueryAsync<OrdersDetailsViewModel>(
                    OrderScripts.GetOrdersDetailsByContractId,
                    parameters);
            }

            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(20));

            json = JsonSerializer.Serialize(viewModel);
            await _cache.SetStringAsync(cacheKey, json, options);
        }

        return viewModel;
    }
}
