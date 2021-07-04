using System.Threading.Tasks;
using RestfulServer.Core.DataAccess.Models;

namespace RestfulServer.Core.DataAccess
{
    public interface ICountableResource
    {
        Task<ValueDto> GetValue(string id);
        Task<ValueDto> ChangeValueBy(string id, int value);
    }
}