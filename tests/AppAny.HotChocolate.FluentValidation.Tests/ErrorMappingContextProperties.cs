using System.Threading.Tasks;
using FluentValidation;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppAny.HotChocolate.FluentValidation.Tests
{
  public class ErrorMappingContextProperties
  {
    [Fact]
    public async Task AddFluentValidation()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation(opt =>
            {
              opt.UseDefaultErrorMapper(
                (_, context) =>
                {
                  Assert.Equal("input", context.Argument.Name);
                  Assert.Single(context.ValidationResult.Errors);
                  Assert.Equal(nameof(TestPersonInput.Name), context.ValidationFailure.PropertyName);
                });
            })
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation());
            }));
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

      result.AssertDefaultErrorMapper(
        "NotEmptyValidator",
        NotEmptyNameValidator.Message);
    }

    [Fact]
    public async Task UseFluentValidation()
    {
      var executor = await TestSetup.CreateRequestExecutor(builder =>
        {
          builder.AddFluentValidation()
            .AddMutationType(new TestMutation(field =>
            {
              field.Argument("input", arg => arg.Type<NonNullType<TestPersonInputType>>().UseFluentValidation(opt =>
              {
                opt.UseDefaultErrorMapper(
                  (_, context) =>
                  {
                    Assert.Equal("input", context.Argument.Name);
                    Assert.Single(context.ValidationResult.Errors);
                    Assert.Equal(nameof(TestPersonInput.Name), context.ValidationFailure.PropertyName);
                  });
              }));
            }));
        },
        services =>
        {
          services.AddTransient<IValidator<TestPersonInput>, NotEmptyNameValidator>();
        });

      var result = Assert.IsType<OperationResult>(
        await executor.ExecuteAsync(TestSetup.Mutations.WithEmptyName));

      result.AssertNullResult();

      result.AssertDefaultErrorMapper(
        "NotEmptyValidator",
        NotEmptyNameValidator.Message);
    }
  }
}
