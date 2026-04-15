namespace SimpleMq.Dtos;

internal sealed record ConsumerRegistrationBuildResult(
    IReadOnlyCollection<Type> ConsumerTypes,
    IReadOnlyCollection<ConsumerRegistration> Registrations
);