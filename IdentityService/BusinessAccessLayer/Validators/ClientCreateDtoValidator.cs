using BAL.Dtos;
using DAL.Repositories;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BAL.Validators
{   
    public class ClientCreateDtoValidator : AbstractValidator<ClientCreateDto>
    {
        private readonly IClientRepository _clientRepository;
        public ClientCreateDtoValidator(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            RuleFor(cl => cl.Phone).Matches(@"^\+375\d+$").WithMessage("{PropertyName} must start with '+375' and contain only numbers.")
                .Length(13).WithMessage("{PropertyName} must be 13 characters long.")
                .MustAsync((phone, cancellationToken) => IsPhoneUniqueAsync(phone, cancellationToken))
                .WithMessage("Accout with this phone number alredy exists...");
            RuleFor(cl => cl.Username).Length(5, 15).WithMessage("{PropertyName} must be between 5 and 15 characters long. Number of characters entered: {TotalLength}.")
                .MustAsync((username, cancellationToken) => IsUsernameUniqueAsync(username, cancellationToken))
                .WithMessage("Accout with this username alredy exists...");
            RuleFor(cl => cl.Password).Length(8, 30).WithMessage("{PropertyName} must be between 8 and 30 characters long. Number of characters entered: {TotalLength}.");
        }

        private async Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken)
        {
            return await _clientRepository.IsUsernameUniqueAsync(username, cancellationToken);
        }

        private async Task<bool> IsPhoneUniqueAsync(string phone, CancellationToken cancellationToken)
        {
            return await _clientRepository.IsPhoneUniqueAsync(phone, cancellationToken);
        }
    }
}
