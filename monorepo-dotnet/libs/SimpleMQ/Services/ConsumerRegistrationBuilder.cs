using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using SimpleMq.Attributes;
using SimpleMq.Dtos;
using SimpleMq.Interfaces;

namespace SimpleMq.Services;

internal static class ConsumerRegistrationBuilder
{
    internal static ConsumerRegistrationBuildResult Build(IEnumerable<Assembly> assemblies)
    {
        var consumerTypes = assemblies
            .SelectMany(a =>
            {
                try { return a.GetTypes(); }
                catch (ReflectionTypeLoadException ex) { return ex.Types.OfType<Type>(); }
            })
            .Where(t => typeof(IMessageConsumer).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToArray();

        var registrations = new List<ConsumerRegistration>();

        foreach (var consumerType in consumerTypes)
        {
            var methods = consumerType.GetMethods()
                .Select(method => new
                {
                    Method = method,
                    Attribute = method.GetCustomAttributes(typeof(ConsumerAttribute), inherit: false)
                        .OfType<ConsumerAttribute>()
                        .FirstOrDefault(),
                })
                .Where(x => x.Attribute is not null);

            foreach (var method in methods)
            {
                var methodInfo = method.Method;
                var methodParameters = methodInfo.GetParameters();
                var payloadType = methodParameters.Length == 1 ? methodParameters[0].ParameterType : null;

                if (!TryValidateConsumerMethod(consumerType, methodInfo, out var validationError))
                {
                    throw new InvalidOperationException(validationError);
                }

                registrations.Add(new ConsumerRegistration(
                    HandlerType: consumerType,
                    MethodName: methodInfo.Name,
                    QueueName: method.Attribute!.QueueName,
                    AutoAck: method.Attribute.AutoAck,
                    RoutingKey: method.Attribute.RoutingKey,
                    PayloadType: payloadType,
                    InvokeAsync: BuildAsyncInvoker(consumerType, methodInfo, payloadType)
                ));
            }
        }

        return new ConsumerRegistrationBuildResult(
            ConsumerTypes: new ReadOnlyCollection<Type>(consumerTypes),
            Registrations: new ReadOnlyCollection<ConsumerRegistration>(registrations)
        );
    }

    private static bool TryValidateConsumerMethod(Type consumerType, MethodInfo method, out string error)
    {
        var parameters = method.GetParameters();
        if (parameters.Length > 1)
        {
            error = $"Consumer method '{consumerType.FullName}.{method.Name}' must have zero or one parameter.";
            return false;
        }

        if (!typeof(Task).IsAssignableFrom(method.ReturnType))
        {
            error = $"Consumer method '{consumerType.FullName}.{method.Name}' must return Task.";
            return false;
        }

        error = string.Empty;
        return true;
    }

    private static Func<object, object?, Task> BuildAsyncInvoker(
        Type handlerType,
        MethodInfo method,
        Type? payloadType)
    {
        var instanceParameter = Expression.Parameter(typeof(object), "instance");
        var payloadParameter = Expression.Parameter(typeof(object), "payload");

        var typedInstance = Expression.Convert(instanceParameter, handlerType);

        Expression methodCall = payloadType is null
            ? Expression.Call(typedInstance, method)
            : Expression.Call(
                typedInstance,
                method,
                Expression.Convert(payloadParameter, payloadType)
            );

        var asTask = Expression.Convert(methodCall, typeof(Task));
        var lambda = Expression.Lambda<Func<object, object?, Task>>(asTask, instanceParameter, payloadParameter);
        return lambda.Compile();
    }
}