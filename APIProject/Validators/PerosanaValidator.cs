using APIProject.Model;
using FluentValidation;

public class PersoanaValidator : AbstractValidator<Persoana>
{
    public PersoanaValidator()
    {
        RuleFor(persoana => persoana.Id).GreaterThanOrEqualTo(0).WithMessage("The Id value can't be negative");
        RuleFor(persoana => persoana.Nume).NotEmpty().WithMessage("You must insert a name for the person");
        RuleFor(persoana => persoana.Prenume).NotEmpty().WithMessage("You must insert a surname for the person");
        RuleFor(persoana => persoana.Adresa).NotEmpty().WithMessage("You must insert an address for the person");
        RuleFor(persoana => persoana.Email).NotEmpty().EmailAddress().WithMessage("You must insert a valid email for the person");
    }
}