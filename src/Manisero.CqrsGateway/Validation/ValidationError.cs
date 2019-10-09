namespace Manisero.CqrsGateway.Validation
{
    public class ValidationError
    {
        public string ErrorCode { get; set; }

        public string Property { get; set; }

        public object Data { get; set; }
    }
}
