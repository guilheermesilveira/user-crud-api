namespace UserCrud.API.Responses;

public class BadRequestResponse
{
    public List<string> Errors { get; set; } = new();
}