using ImGuiNET;
using System.Numerics;

namespace IMidi;

public class ImGuiTheme
{
    public static ImGuiStylePtr Style;

    public static void PushTheme()
    {
        Style = ImGui.GetStyle();
        Style.FrameRounding = 4;
        Style.FramePadding = new Vector2(5, 7);
        Style.Colors[(int)ImGuiCol.Button] = new Vector4(0.29f, 0.29f, 0.29f, .9f);
        Style.Colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.29f, 0.29f, 0.29f, .9f) * 1.2f;
        Style.Colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.29f, 0.29f, 0.29f, .9f) * 1.5f;

        Style.Colors[(int)ImGuiCol.TitleBgActive] = new Vector4(1, 0, 0, 1);

        Style.Colors[(int)ImGuiCol.FrameBg] = new Vector4(0.29f, 0.29f, 0.29f, .9f);
        Style.Colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.29f, 0.29f, 0.29f, .9f) * 1.2f;
        Style.Colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.29f, 0.29f, 0.29f, .9f) * 1.5f;

        Style.Colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.29f, 0.29f, 0.29f, .9f);
    }
}
