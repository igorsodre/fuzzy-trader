namespace FuzzyTrader.Contracts.Responses
{
    public class SuccessResponse<T>
    {
        public SuccessResponse() { }

        public SuccessResponse(T response)
        {
            Data = response;
        }

        public T Data { get; set; }
    }
}