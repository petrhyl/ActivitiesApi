namespace Contracts.Response;

public class ActivityAttenderResponse
{
    public required AppUserResponse Attender { get; set; }

    public required bool IsHost { get; set; }
}
