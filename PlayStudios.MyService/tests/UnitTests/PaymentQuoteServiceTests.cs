using PaymentService;
using Xunit;

namespace UnitTests;

public class PaymentQuoteServiceTests
{
    [Fact]
    public void CreateQuote_ReturnsPendingUsdQuote()
    {
        var quote = new PaymentQuoteService().CreateQuote(125m);

        Assert.Equal(125m, quote.Amount);
        Assert.Equal("USD", quote.Currency);
        Assert.Equal("pending", quote.Status);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CreateQuote_RejectsNonPositiveAmount(int amount)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new PaymentQuoteService().CreateQuote(amount));
    }
}
