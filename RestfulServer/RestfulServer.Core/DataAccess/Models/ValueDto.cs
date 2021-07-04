namespace RestfulServer.Core.DataAccess.Models
{
    public class ValueDto
    {
        public int Value { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success => string.IsNullOrEmpty(ErrorMessage);
        public bool ExceptionThrown { get; set; }
    }
}