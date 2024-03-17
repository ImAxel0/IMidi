using Melanchall.DryWetMidi.Core;

namespace IMidi;

public class InputManager
{
    public static List<int> PressedKeys { get; private set; } = new();

    public static void OnKeyPress(NoteOnEvent ev)
    {
        PressedKeys.Add(ev.NoteNumber);
    }

    public static void OnKeyRelease(NoteOffEvent ev)
    {
        PressedKeys.Remove(ev.NoteNumber);
    }
}
