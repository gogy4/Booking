using Booking.Models;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Interfaces;

namespace Domain.CustomerValidator;

public class CustomerEditValidator : AbstractValidator<CustomerEditViewModel>
{
    private readonly ICustomerRepository customerRepository;

    public CustomerEditValidator(ICustomerRepository customerRepository)
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
            .MustAsync((email, token) => ValidateUniqueField(email, customerRepository.GetByEmail))
            .WithMessage("Данная почта уже зарегистрирована");

        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Пароль обязателен для заполнения")
            .Length(6, 100).WithMessage("Пароль должен содержать от 6 до 100 символов")
            .Matches(@"[!@#$%^&*(){}[\]_+-]").WithMessage("Пароль должен содержать хотя бы один специальный символ: !@#$%^&*(){}[]_-")
            .Matches(@"[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву")
            .Matches(@"[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву")
            .MustAsync((model, password, token) => ValidateOldPassword(model.Email, model.OldPassword, token))
            .WithMessage("Вы ввели неверный старый пароль");
        
        RuleFor(x => x.NewPassword)
            .Length(6, 100).WithMessage("Новый пароль должен содержать от 6 до 100 символов")
            .Matches(@"[!@#$%^&*(){}[\]_+-]").WithMessage("Новый пароль должен содержать хотя бы один специальный символ: !@#$%^&*(){}[]_-")
            .Matches(@"[A-Z]").WithMessage("Новый пароль должен содержать хотя бы одну заглавную букву")
            .Matches(@"[a-z]").WithMessage("Новый пароль должен содержать хотя бы одну строчную букву");

        RuleFor(x => x.ConfirmPassword)
            .Length(6, 100).WithMessage("Новый пароль должен содержать от 6 до 100 символов")
            .Matches(@"[!@#$%^&*(){}[\]_+-]")
            .WithMessage("Новый пароль должен содержать хотя бы один специальный символ: !@#$%^&*(){}[]_-")
            .Matches(@"[A-Z]").WithMessage("Новый пароль должен содержать хотя бы одну заглавную букву")
            .Matches(@"[a-z]").WithMessage("Новый пароль должен содержать хотя бы одну строчную букву")
            .MustAsync((model, password, token) =>
                ValidateNewPassword(model.NewPassword, model.ConfirmPassword, token))
            .WithMessage("Пароли не совпадают");
    }
    
    private async Task<bool> ValidateUniqueField(string value, Func<string, Task<Customer?>> getCustomerByField)
    {
        var customer = await getCustomerByField(value);
        return customer is null;
    }
    
    private async Task<bool> ValidateOldPassword(string email, string oldPassword, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByEmail(email);

        if (customer == null)
            return false;

        return customer.Password == oldPassword;
    }
    
    private async Task<bool> ValidateNewPassword(string newPassword, string confirmPassword, CancellationToken cancellationToken)
    {
        return newPassword == confirmPassword;
    }
}
