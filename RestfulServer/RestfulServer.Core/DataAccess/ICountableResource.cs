namespace RestfulServer.Core.DataAccess
{
    public interface ICountableResource
    {
        int GetValue(string id);
        bool ChangeValue(string id, int changeSize);
    }
}