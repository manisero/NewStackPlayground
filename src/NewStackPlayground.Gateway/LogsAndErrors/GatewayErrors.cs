using Manisero.Logger;

namespace NewStackPlayground.Gateway.LogsAndErrors
{
    [KnownException("AA138B18-6273-4A6A-AEA1-E3437220CFEA")]
    public class SampleKnownError : KnownException
    {
        public string Text { get; }

        public SampleKnownError(
            string text = "Sample known error.")
        {
            Text = text;
        }

        public override object GetData() => new { Text };
    }
}
