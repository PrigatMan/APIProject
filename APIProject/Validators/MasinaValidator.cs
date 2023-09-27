using FluentValidation;
using APIProject.Model;
using System.Data.SqlClient;
using System.Data;
using Dapper;
public class MasinaValidator : AbstractValidator<Masina>
{
    public MasinaValidator() 
    {
        RuleFor(masina => masina.Id).GreaterThanOrEqualTo(0).WithMessage("The Id value can't be negative");
        RuleFor(masina => masina.Marca).NotEmpty().WithMessage("You must insert a car manufacturer");
        RuleFor(masina => masina.Model).NotEmpty().WithMessage("You must insert the car model");
        RuleFor(masina => masina.An).GreaterThanOrEqualTo(1889).WithMessage("The year value must be higher than 1889");
        RuleFor(masina => masina.Motor).GreaterThan(0).WithMessage("The engine size value must be higher than 0");
    }
}