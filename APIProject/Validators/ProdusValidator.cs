using APIProject.Model;
using FluentValidation;

public class ProdusValidator : AbstractValidator<Produs>
{
    public ProdusValidator()
    {
        RuleFor(produs => produs.Id).GreaterThanOrEqualTo(0).WithMessage("The Id value can't be negative");
        RuleFor(produs => produs.Denumire).NotEmpty().WithMessage("You must insert a name for the product");
        RuleFor(produs => produs.Stoc).GreaterThanOrEqualTo(0).WithMessage("The Stoc value can't be negative");
        RuleFor(produs => produs.Pret).GreaterThan(0).WithMessage("The Pret value must be higher than 0");
    }
}