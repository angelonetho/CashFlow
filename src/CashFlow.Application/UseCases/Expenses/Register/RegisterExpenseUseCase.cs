using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase
{
    public ResponseRegisteredExpenseJson Execute(RequestExpenseJson request)
    {
        return new ResponseRegisteredExpenseJson();
    }
}