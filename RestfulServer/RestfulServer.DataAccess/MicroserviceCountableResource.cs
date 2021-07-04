using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RestfulServer.Core.DataAccess;
using RestfulServer.Core.DataAccess.Models;

namespace RestfulServer.DataAccess
{
    public class MicroserviceCountableResource : ICountableResource
    {
        private readonly HttpClient _client;

        public MicroserviceCountableResource(HttpClient client)
        {
            _client = client;
        }

        public async Task<ValueDto> GetValue(string id)
        {
            var dto = new ValueDto();
            try
            {
                using var response = await _client.GetAsync(new Uri($"CountableResource/{id}", UriKind.Relative));
                using var content = response.Content;
                
                var message = await content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode && int.TryParse(message, out var value))
                {
                    dto.Value = value;
                    return dto;
                }

                dto.ErrorMessage = message;
            }
            catch (Exception e)
            {
                dto.ErrorMessage = "Service is currently not available";
                dto.ExceptionThrown = true;
            }

            return dto;
        }

        public async Task<ValueDto> ChangeValueBy(string id, int value)
        {
            var dto = new ValueDto();
            try
            {
                var requestUri = new Uri($"CountableResource/{id}/ChangeValueBy/{value}", UriKind.Relative);
                var emptyBody = new StringContent("");
                using var response = await _client.PutAsync(requestUri, emptyBody);
                using var content = response.Content;
                
                if (response.IsSuccessStatusCode)
                    return dto;
                    
                dto.ErrorMessage = await content.ReadAsStringAsync();
                
                // The response failing and the message empty. For instance, HTTP status 405 method not allowed. 
                if (dto.Success)
                    dto.ErrorMessage = "Internal error. Contact support";
            }
            catch (Exception e)
            {
                dto.ErrorMessage = "Service is currently not available";
                dto.ExceptionThrown = true;
            }

            return dto;
        }
    }
}