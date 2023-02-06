namespace lab15.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
