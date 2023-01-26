using Microsoft.AspNetCore.Http.HttpResults;

public class ValidationTools
{

  public ValidationProblem ValidationProblem(string detail)
  {
    return TypedResults.ValidationProblem(new Dictionary<string, string[]>(), detail);
  }

  public ValidationProblem FieldValidationProblem(string fieldName, string validationFailure)
  {
    return TypedResults.ValidationProblem(new Dictionary<string, string[]>()
        {
            {
              fieldName,
              new[] {  validationFailure }
            }
        });
  }
}
