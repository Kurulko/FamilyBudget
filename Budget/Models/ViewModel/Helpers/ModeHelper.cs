namespace Budget.Models.ViewModel.Helpers;

public class ModeHelper
{
    public static string GetBootstrapColorByMode(Mode mode)
        => mode switch
        {
            Mode.Get => "info",
            Mode.Add => "primary",
            Mode.Edit => "warning",
            Mode.Delete => "danger",
            _ => "primary",
        };
}
