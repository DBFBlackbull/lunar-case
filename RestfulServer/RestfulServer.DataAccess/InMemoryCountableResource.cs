using System;
using System.Collections.Generic;
using RestfulServer.Core.DataAccess;

namespace RestfulServer.DataAccess
{
    public class InMemoryCountableResource : ICountableResource
    {
        private readonly IDictionary<string, int> _dictionary;

        public InMemoryCountableResource()
        {
            _dictionary = new Dictionary<string, int>();
        }

        public int GetValue(string id)
        {
            AssertValidId(id);

            _dictionary.TryGetValue(id, out var value);
            return value;
        }

        public bool ChangeValue(string id, int changeSize)
        {
            AssertValidId(id);

            if (!_dictionary.ContainsKey(id))
                _dictionary[id] = 0;

            _dictionary[id] += changeSize;
            return true;
        }
        
        private static void AssertValidId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id), "Id is not valid");
        }
    }
}