using Domain.Entities;
using FluentValidation;
using Infrastructure.Interfaces;

namespace Domain.CustomerValidator;

public class CustomerRegisterValidator : AbstractValidator<Customer>
{
    private readonly ICustomerRepository customerRepository;

    public CustomerRegisterValidator(ICustomerRepository customerRepository)
    {
        this.customerRepository = customerRepository;

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Имя обязательно для заполнения")
            .Length(2, 50).WithMessage("Имя должно содержать от 2 до 50 символов");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Фамилия обязательна для заполнения")
            .Length(2, 50).WithMessage("Фамилия должна содержать от 2 до 50 символов");
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Номер телефона обязателен для заполнения")
            .Matches(@"^\+7\d{10}$").WithMessage("Номер телефона должен начинаться с +7 и содержать 10 цифр после него")
            .MustAsync((phone, token) => ValidateUniqueField(phone, customerRepository.GetByPhoneNumber))
            .WithMessage("Данный номер телефона уже зарегистрирован");

        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обязателен для заполнения")
            .EmailAddress().WithMessage("Email не является действительным")
            .MustAsync((email, token) => ValidateUniqueField(email, this.customerRepository.GetByEmail))
            .WithMessage("Данная почта уже зарегистрирована");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен для заполнения")
            .Length(6, 100).WithMessage("Пароль должен содержать от 6 до 100 символов")
            .Matches(@"[!@#$%^&*(){}[\]_+-]").WithMessage("Пароль должен содержать хотя бы один специальный символ: !@#$%^&*(){}[]_-")
            .Matches(@"[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву")
            .Matches(@"[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву");
    }
    
    private async Task<bool> ValidateUniqueField(string value, Func<string, Task<Customer?>> getCustomerByField)
    {
        var customer = await getCustomerByField(value);
        return customer is null;
    }
}
