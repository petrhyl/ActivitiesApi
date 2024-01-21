namespace Contracts.Response;

public class ActivityAttenderResponse
{
    public required ProfileResponse Attender { get; set; }

    public required bool IsHost { get; set; }
}
