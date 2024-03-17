using IconFonts;
using ImGuiNET;
using System.Numerics;
using Vanara.PInvoke;

namespace IMidi;

public class ScreenCanvas
{
    public static Vector2 CanvasPos { get; private set; }

    static uint _red = ImGui.GetColorU32(new Vector4(1, 0, 0, 1));
    static uint _blacksRed = ImGui.GetColorU32(new Vector4(1.00f, 0.13f, 0.29f, 1));

    private static void RenderGrid()
    {
        for (int key = 0; key < 52; key++)
        {
            if (key % 7 == 2)
            {
                ImGui.GetWindowDrawList().AddLine(CanvasPos + new Vector2(key * PianoRenderer.Width, 0), 
                    new(PianoRenderer.P.X + key * PianoRenderer.Width, PianoRenderer.P.Y), ImGui.GetColorU32(new Vector4(1, 1, 1, 0.06f)), 2);
            }
            else
                ImGui.GetWindowDrawList().AddLine(CanvasPos + new Vector2(key * PianoRenderer.Width, 0), 
                    new(PianoRenderer.P.X + key * PianoRenderer.Width, PianoRenderer.P.Y), ImGui.GetColorU32(new Vector4(1, 1, 1, 0.06f)));
        }
    }

    private static void DrawNotes()
    {
        var speed = 100f * ImGui.GetIO().DeltaTime;
        var drawList = ImGui.GetWindowDrawList();
    
        for (int i = 0; i < PianoRenderer.WhiteRects.Count; i++)
        {            
            PianoRenderer.NoteRect rect = PianoRenderer.WhiteRects[i];
            rect.TopLeft -= speed;
            rect.BottomRight -= speed;
            PianoRenderer.WhiteRects[i] = rect;
            
            if (rect.BottomRight <= 0)
            {
                PianoRenderer.WhiteRects.Remove(rect);
                continue;
            }

            drawList.AddRectFilled(
                new Vector2(PianoRenderer.P.X + rect.KeyNum * PianoRenderer.Width + 1, rect.TopLeft),
                new Vector2(PianoRenderer.P.X + rect.KeyNum * PianoRenderer.Width + PianoRenderer.Width - 1, rect.BottomRight),
                ImGui.GetColorU32(_red), 1, ImDrawFlags.RoundCornersAll);
        }

        for (int i = 0; i < PianoRenderer.BlackRects.Count; i++)
        {
            
            PianoRenderer.NoteRect rect = PianoRenderer.BlackRects[i];          
            rect.TopLeft -= speed;
            rect.BottomRight -= speed;
            PianoRenderer.BlackRects[i] = rect;
            
            if (rect.BottomRight <= 0)
            {
                PianoRenderer.BlackRects.Remove(rect);
                continue;
            }

            drawList.AddRectFilled(
                new Vector2(PianoRenderer.P.X + rect.KeyNum * PianoRenderer.Width + PianoRenderer.Width * 3 / 4, rect.TopLeft),
                new Vector2(PianoRenderer.P.X + rect.KeyNum * PianoRenderer.Width + PianoRenderer.Width * 5 / 4 + 1, rect.BottomRight),
                ImGui.GetColorU32(_blacksRed), 1, ImDrawFlags.RoundCornersAll);
        }
    }   

    public static void RenderScreen()
    {
        CanvasPos = ImGui.GetWindowPos();
        RenderGrid();
        DrawNotes();

        ImGui.SetNextWindowPos(new(ImGui.GetIO().DisplaySize.X / 2 - 100, CanvasPos.Y + 15));
        ImGui.BeginChild("Player controls", new Vector2(200, 50), ImGuiChildFlags.None, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse);

        ImGui.PushFont(FontController.Font16_Icon16);
        if (ImGui.Button($"{FontAwesome6.Play}", new(50, ImGui.GetWindowSize().Y)))
        {
            MidiPlayer.Playback?.Start();
        }
        ImGui.SameLine();
        if (ImGui.Button($"{FontAwesome6.Pause}", new(50, ImGui.GetWindowSize().Y)))
        {
            MidiPlayer.Playback?.Stop();
        }
        ImGui.SameLine();
        if (ImGui.Button($"{FontAwesome6.Stop}", new(50, ImGui.GetWindowSize().Y)))
        {
            MidiPlayer.Playback?.Stop();
            MidiPlayer.Playback?.MoveToStart();
        }

        ImGui.PopFont();
        ImGui.EndChild();
    }
}
