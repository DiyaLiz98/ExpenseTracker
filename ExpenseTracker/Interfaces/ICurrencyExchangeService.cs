namespace ExpenseTracker.Interfaces
{
    public interface ICurrencyExchangeService
    {
        decimal ConvertCurrency(decimal amount, string fromCurrency, string toCurrency);
        Task<decimal?> GetExchangeRateAsync(string currencyCode);
    }

}
