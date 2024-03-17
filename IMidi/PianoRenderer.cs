using ImGuiNET;
using System.Numerics;

namespace IMidi;

public class PianoRenderer
{
    static uint _black = ImGui.GetColorU32(new Vector4(0, 0, 0, 1));
    static uint _white = ImGui.GetColorU32(new Vector4(1));
    static uint _red = ImGui.GetColorU32(new Vector4(1, 0, 0, 1));
    static uint _blacksRed = ImGui.GetColorU32(new Vector4(1.00f, 0.13f, 0.29f, 1));

    public static float Width;
    public static float Height;
    public static Vector2 P;

    public static List<NoteRect> WhiteRects = new();
    public static List<NoteRect> BlackRects = new();

    public struct NoteRect
    {
        public int KeyNum;
        public float TopLeft;
        public float BottomRight;
    }

    public static void RenderKeyboard()
    {
        ImDrawListPtr draw_list = ImGui.GetWindowDrawList();
        P = ImGui.GetCursorScreenPos();

        Width = ImGui.GetIO().DisplaySize.X * 1.9f / 100;
        Height = ImGui.GetContentRegionAvail().Y;

        int cur_key = 21;
        int cCount = 1;
        for (int key = 0; key < 52; key++)
        {
            uint col = _white;
            if (InputManager.PressedKeys.Contains(cur_key))
            {
                col = _red;
                var n = new NoteRect();
                n.KeyNum = key;
                n.TopLeft = P.Y;
                n.BottomRight = P.Y;
                WhiteRects.Add(n);
            }

            draw_list.AddRectFilled(
                    new(P.X + key * Width, P.Y),
                    new(P.X + key * Width + Width, P.Y + Height),
                    col, 0);
            draw_list.AddRect(
                    new(P.X + key * Width, P.Y),
                    new(P.X + key * Width + Width, P.Y + Height),
                    _black, 0);

            if (key % 7 == 1)
            {
                ImGui.GetForegroundDrawList().AddText(new(P.X + key * Width + Width + 9, P.Y + Height / 1.1f), _black, $"C{cCount}");
                cCount++;
            }

            cur_key++;
            if (KeysUtils.HasBlack(key))
            {
                cur_key++;
            }
        }
        
        cur_key = 22;
        for (int key = 0; key < 52; key++)
        {
            if (KeysUtils.HasBlack(key))
            {
                uint col = _black;
                if (InputManager.PressedKeys.Contains(cur_key))
                {
                    col = _blacksRed;
                    var n = new NoteRect();
                    n.KeyNum = key;
                    n.TopLeft = P.Y;
                    n.BottomRight = P.Y;
                    BlackRects.Add(n);
                }
                draw_list.AddRectFilled(
                        new(P.X + key * Width + Width * 3 / 4, P.Y),
                        new(P.X + key * Width + Width * 5 / 4 + 1, P.Y + Height / 1.5f),
                        col, 0);
                draw_list.AddRect(
                        new(P.X + key * Width + Width * 3 / 4, P.Y),
                        new(P.X + key * Width + Width * 5 / 4 + 1, P.Y + Height / 1.5f),
                        _black, 0);
                cur_key += 2;
            }
            else
            {
                cur_key++;
            }
        }
    }
}
