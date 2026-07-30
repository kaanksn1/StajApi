namespace StajApi.Models.Responses;

/// <summary>
/// JWT içerisinden okunan tek bir claim bilgisini temsil eder.
/// </summary>
public class TokenClaimDto
{
    public string Type { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;
}
