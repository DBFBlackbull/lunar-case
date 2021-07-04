using System.Collections.Generic;
using System.Threading.Tasks;
using RestfulServer.Core.DataAccess;
using RestfulServer.Core.DataAccess.Models;

namespace RestfulServer.DataAccess
{
    public class InMemoryCountableResource : ICountableResource
    {
        private readonly IDictionary<string, int> _dictionary;

        public InMemoryCountableResource()
        {
            _dictionary = new Dictionary<string, int>();
        }

        public async Task<ValueDto> GetValue(string id)
        {
            var dto = new ValueDto();
            if (string.IsNullOrWhiteSpace(id))
            {
                dto.ErrorMessage = "Id is not valid";
                return dto;
            }

            _dictionary.TryGetValue(id, out var value);
            dto.Value = value;
            return dto;
        }

        public async Task<ValueDto> ChangeValueBy(string id, int value)
        {
            var dto = new ValueDto();
            if (string.IsNullOrWhiteSpace(id))
            {
                dto.ErrorMessage = "Id is not valid";
                return dto;
            }

            if (!_dictionary.ContainsKey(id))
                _dictionary[id] = 0;

            _dictionary[id] += value;
            return dto;
        }
    }
}