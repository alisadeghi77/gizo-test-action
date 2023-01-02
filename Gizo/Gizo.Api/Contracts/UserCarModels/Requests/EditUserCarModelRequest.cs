namespace Gizo.Api.Contracts.UserCarModels.Requests;

public class EditUserCarModelRequest
{
    public long Id { get; set; }

    public string? Licence { get; set; }
}
