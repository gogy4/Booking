using FluentValidation;
using Infrastructure.Interfaces;

namespace Domain.CustomerValidator
{
    public class CustomerLoginValidator : AbstractValidator<(string Email, string Password)>
    {
        private readonly ICustomerRepository customerRepository; 

        public CustomerLoginValidator(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email обязателен для заполнения")
                .EmailAddress().WithMessage("Email не является действительным");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен для заполнения")
                .Length(6, 100).WithMessage("Пароль должен содержать от 6 до 100 символов");

            RuleFor(x => x)
                .MustAsync(ValidateCredentials).WithMessage("Неверный логин или пароль");
        }

        private async Task<bool> ValidateCredentials((string Email, string Password) credentials, CancellationToken cancellationToken)
        {
            var customer = await customerRepository.GetByEmail(credentials.Email);

            if (customer == null)
                return false; 

            return customer.Password == credentials.Password;
        }
    }
}