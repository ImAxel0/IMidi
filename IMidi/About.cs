using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace IMidi;

public class About
{
    static readonly string _info = "Powered by DryWetMidi";
    static Vector2 _winSize = new(600, 300);
    static bool _show;

    public static void ShowAboutWindow(bool show)
    {
        _show = show;
    }

    public static void RenderWindow()
    {
        if (!_show)
            return;

        ImGui.OpenPopup($"About");
        ImGui.SetNextWindowSize(_winSize);
        ImGui.SetNextWindowPos(ImGui.GetIO().DisplaySize / 2 - (_winSize / 2));
        ImGui.BeginPopupModal($"About", ref _show, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoMove);

        ImGui.SetCursorPos(ImGui.GetWindowSize() / 2 - new Vector2(75, 75));
        ImGui.Image(ProgramData.LogoImage, new(150, 150));

        ImGui.SetCursorPosX(ImGui.GetWindowSize().X / 2 - (ImGui.CalcTextSize(_info).X / 2));
        if (ImGui.IsItemHovered())
        {
            ImGui.SetMouseCursor(ImGuiMouseCursor.Hand);
            if (ImGui.IsMouseClicked(ImGuiMouseButton.Left))
            {
                Process.Start(new ProcessStartInfo("https://github.com/ImAxel0/IMidi") { UseShellExecute = true });
            }
        }

        ImGui.Text(_info);
        if (ImGui.IsItemHovered())
        {
            ImGui.SetMouseCursor(ImGuiMouseCursor.Hand);
            if (ImGui.IsMouseClicked(ImGuiMouseButton.Left))
            {
                Process.Start(new ProcessStartInfo("https://github.com/melanchall/drywetmidi") { UseShellExecute = true });
            }
        }

        ImGui.EndPopup();
    }
}
