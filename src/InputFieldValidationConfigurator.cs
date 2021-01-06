using System.Collections.Generic;

namespace AppAny.HotChocolate.FluentValidation
{
	/// <summary>
	/// Configures input field validation options
	/// </summary>
	public interface IInputFieldValidationConfigurator
	{
		/// <summary>
		/// Overrides global validation options. Conditionally skips validation
		/// </summary>
		IInputFieldValidationConfigurator SkipValidation(SkipValidation skipValidation);

		/// <summary>
		/// Overrides global validation options. Adds new <see cref="InputValidatorFactory"/> for input field
		/// </summary>
		IInputFieldValidationConfigurator UseInputValidatorFactories(params InputValidatorFactory[] validatorFactories);

		/// <summary>
		/// Overrides global validation options. Adds new <see cref="ErrorMapper"/> for input field
		/// </summary>
		IInputFieldValidationConfigurator UseErrorMappers(params ErrorMapper[] errorMappers);
	}

	internal sealed class InputFieldValidationConfigurator : IInputFieldValidationConfigurator
	{
		private readonly InputFieldValidationOptions options;

		public InputFieldValidationConfigurator(InputFieldValidationOptions options)
		{
			this.options = options;
		}

		public IInputFieldValidationConfigurator SkipValidation(SkipValidation skipValidation)
		{
			options.SkipValidation = skipValidation;

			return this;
		}

		public IInputFieldValidationConfigurator UseInputValidatorFactories(params InputValidatorFactory[] validatorFactories)
		{
			options.ValidatorFactories ??= new List<InputValidatorFactory>();

			foreach (var validatorFactory in validatorFactories)
			{
				options.ValidatorFactories.Add(validatorFactory);
			}

			return this;
		}

		public IInputFieldValidationConfigurator UseErrorMappers(params ErrorMapper[] errorMappers)
		{
			options.ErrorMappers ??= new List<ErrorMapper>();

			foreach (var errorMapper in errorMappers)
			{
				options.ErrorMappers.Add(errorMapper);
			}

			return this;
		}
	}
}