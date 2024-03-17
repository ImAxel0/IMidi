namespace IMidi;

public class ProgramData
{
    public static IntPtr LogoImage;

    public static void Initialize()
    {
        ImGuiTheme.PushTheme();
        DevicesManager.Initialize();
    }
}
