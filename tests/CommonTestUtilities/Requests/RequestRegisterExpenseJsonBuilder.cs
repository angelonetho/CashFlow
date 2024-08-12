using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterExpenseJsonBuilder
{
    public static RequestExpenseJson Build()
    {
        var faker = new Faker();

        var request = new RequestExpenseJson
        {
            Title = faker.Commerce.ProductName(),
            Description = faker.Commerce.ProductDescription(),
            PaymentType = faker.PickRandom<PaymentType>(),
            Amount = faker.Random.Decimal( min: 10, max: 1000),
            Date = faker.Date.Past()
        };

        return request;
    }
}