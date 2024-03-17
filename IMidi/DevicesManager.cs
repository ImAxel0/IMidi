using ImGuiNET;
using System.Numerics;
using Melanchall.DryWetMidi.Multimedia;

namespace IMidi;

public class DevicesManager
{
    static Vector2 _winSize = new(600, 100);
    static bool _show;

    public static OutputDevice? OutputDevice { get; private set; }

    public static void ShowDevicesManager(bool show)
    {
        _show = show;
    }

    public static void Initialize()
    {
        if (OutputDevice.GetAll().Count > 0)
        {
            OutputDevice = OutputDevice.GetByIndex(0);
        }
    }

    public static void RenderDevicesWindow()
    {
        if (!_show) 
            return; 

        ImGui.OpenPopup($"Devices");
        ImGui.SetNextWindowSize(_winSize);
        ImGui.SetNextWindowPos(ImGui.GetIO().DisplaySize / 2 - (_winSize / 2));
        ImGui.BeginPopupModal($"Devices", ref _show, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoMove);

        if (ImGui.BeginCombo("Output device", OutputDevice?.Name, ImGuiComboFlags.HeightLargest))
        {
            foreach (var outDevice in OutputDevice.GetAll())
            {
                if (ImGui.Selectable(outDevice.Name))
                {
                    MidiPlayer.Playback?.Stop();
                    MidiPlayer.Playback?.MoveToStart();
                    MidiPlayer.ClearPlayback();

                    OutputDevice?.Dispose();
                    OutputDevice = null;
                    OutputDevice = outDevice;
                }
            }
            ImGui.EndCombo();
        }

        ImGui.EndPopup();
    }
}
