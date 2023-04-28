using Budget.Models.ViewModel.Helpers;

namespace Budget.Models.ViewModel;

public struct TagAModel
{
    public TagAModel(string hrefBeforeUserId, string hrefAfterUserId, string? userId, Mode mode)
        => (HrefBeforeUserId, HrefAfterUserId, UserId, Mode) = (hrefBeforeUserId, hrefAfterUserId, userId, mode);

    public Mode Mode { get; set; }

    public string HrefBeforeUserId { get; set; }
    public string HrefAfterUserId { get; set; }
    public string? UserId { get; set; }


    public string GetHref()
    {
        string href = $"/{HrefBeforeUserId}";

        if (UserId is not null)
            href += $"/{UserId}";

        href += $"/{HrefAfterUserId}";

        return href;
    }
}