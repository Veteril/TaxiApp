using BAL.Dtos;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Validators
{
    public class DriverCreateDtoValidator : AbstractValidator<DriverCreateDto>
    {
        private readonly IDriverRepository _driverRepository;
        public DriverCreateDtoValidator(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
            
            RuleFor(dr => dr.Phone).Matches(@"^\+375\d+$").WithMessage("{PropertyName} must start with '+375' and contain only numbers.")
                .Length(13).WithMessage("{PropertyName} must be 13 characters long.")
                .MustAsync((phone, cancellationToken) => IsPhoneUniqueAsync(phone, cancellationToken))
                .WithMessage("Accout with this phone number alredy exists...");
            
            RuleFor(dr => dr.Username).Length(5, 15).WithMessage("{PropertyName} must be between 5 and 15 characters long. Number of characters entered: {TotalLength}.")
                .MustAsync((username, cancellationToken) => IsUsernameUniqueAsync(username, cancellationToken))
                .WithMessage("Accout with this username alredy exists...");
            
            RuleFor(dr => dr.Password).Length(8, 30)
                .WithMessage("{PropertyName} must be between 8 and 30 characters long. Number of characters entered: {TotalLength}.");

            RuleFor(dr => dr.Name).Length(2, 15)
                .WithMessage("{PropertyName} must be between 2 and 15 characters long. Number of characters entered: {TotalLength}.");

            RuleFor(dr => dr.Email).NotEmpty().WithMessage("{PropertyName} can`t be empty...")
                .EmailAddress().WithMessage("Inccorect format of {PropertyName}");
        }

        private async Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken)
        {
            return await _driverRepository.IsUsernameUniqueAsync(username, cancellationToken);
        }

        private async Task<bool> IsPhoneUniqueAsync(string phone, CancellationToken cancellationToken)
        {
            return await _driverRepository.IsPhoneUniqueAsync(phone, cancellationToken);
        }
    }
}
