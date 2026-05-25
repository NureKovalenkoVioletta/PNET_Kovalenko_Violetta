using Microsoft.AspNetCore.Mvc;

namespace DietaryFitnessProject.UI.Localization;

public static class UkMvcValidation
{
    public static void ConfigureModelBindingMessages(MvcOptions options)
    {
        var provider = options.ModelBindingMessageProvider;

        provider.SetValueMustNotBeNullAccessor(
            name => $"Поле «{name}» обов'язкове.");

        provider.SetValueIsInvalidAccessor(
            value => $"Значення «{value}» некоректне.");

        provider.SetAttemptedValueIsInvalidAccessor(
            (value, name) => $"Значення «{value}» некоректне для поля «{name}».");

        provider.SetNonPropertyAttemptedValueIsInvalidAccessor(
            value => $"Значення «{value}» некоректне.");

        provider.SetUnknownValueIsInvalidAccessor(
            name => $"Значення для поля «{name}» некоректне.");

        provider.SetNonPropertyUnknownValueIsInvalidAccessor(
            () => "Надіслано некоректне значення.");

        provider.SetMissingBindRequiredValueAccessor(
            name => $"Поле «{name}» обов'язкове.");

        provider.SetMissingKeyOrValueAccessor(
            () => "Потрібне значення.");

        provider.SetMissingRequestBodyRequiredValueAccessor(
            () => "Потрібне тіло запиту.");
    }
}
