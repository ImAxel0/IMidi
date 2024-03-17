using ImGuiNET;
using Melanchall.DryWetMidi.Interaction;
using IconFonts;

namespace IMidi;

public class MenuBar
{
    public static void RenderMenubar()
    {
        ImGui.BeginMenuBar();

        if (ImGui.BeginMenu($"{FontAwesome6.Folder} File"))
        {
            if (ImGui.MenuItem("Load midi file"))
            {
                MidiReader.OpenMidiDialog();
            }

            ImGui.EndMenu();
        }

        if (ImGui.BeginMenu($"{FontAwesome6.Computer} Devices"))
        {
            if (ImGui.Selectable("Devices manager"))
            {
                DevicesManager.ShowDevicesManager(true);
            }
            ImGui.EndMenu();
        }

        if (ImGui.BeginMenu($"{FontAwesome6.User} About"))
        {
            About.ShowAboutWindow(true);
            ImGui.EndMenu();
        }

        ImGui.SetCursorScreenPos(new(ImGui.GetIO().DisplaySize.X / 2 - (ImGui.CalcTextSize($"{MidiFileData.FileName}").X / 2), ImGui.GetCursorPos().Y));
        ImGui.TextDisabled($"{MidiFileData.FileName}");

        if (MidiPlayer.Playback != null)
        {
            string timeLabel = $"{(int)MidiPlayer.Seconds}s / {(int)MidiFileData.MidiFile.GetDuration<MetricTimeSpan>().TotalSeconds} sec";

            ImGui.SetCursorScreenPos(new(ImGui.GetIO().DisplaySize.X - (ImGui.CalcTextSize(timeLabel).X + 20), ImGui.GetCursorPos().Y));
            ImGui.Text(timeLabel);
        }

        ImGui.EndMenuBar();
    }
}
