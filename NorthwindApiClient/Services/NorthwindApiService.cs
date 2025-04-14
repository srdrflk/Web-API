using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NorthwindApiClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindApiClient.Services
{
    public class NorthwindApiService : INorthwindApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public NorthwindApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _httpClient.Timeout = TimeSpan.FromSeconds(
                double.Parse(configuration["ApiSettings:TimeoutSeconds"]));
        }

        public async Task<PagedResponse<ProductDto>> GetProductsAsync(int pageNumber = 1, int pageSize = 10, int? categoryId = null)
        {
            var url = $"{_baseUrl}products?pageNumber={pageNumber}&pageSize={pageSize}";
            if (categoryId.HasValue)
            {
                url += $"&categoryId={categoryId}";
            }

            var response = await _httpClient.GetAsync(url);
            return await HandleResponse<PagedResponse<ProductDto>>(response);
        }

        public async Task<ProductDto> GetProductAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}products/{id}");
            return await HandleResponse<ProductDto>(response);
        }

        public async Task<ProductDto> CreateProductAsync(ProductDto product)
        {
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}products", content);
            return await HandleResponse<ProductDto>(response);
        }

        public async Task UpdateProductAsync(int id, ProductDto product)
        {
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}products/{id}", content);
            await HandleResponse<object>(response);
        }

        public async Task DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}products/{id}");
            await HandleResponse<object>(response);
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API request failed: {response.StatusCode} - {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
