using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

namespace IMidi;

public class MidiFileData
{
    public static MidiFile MidiFile;
    public static string FileName = "No midi file opened";
    public static TempoMap TempoMap;

    public static void ReleaseMidiFile()
    {
        MidiFile = null;
        FileName = "No midi file opened";
        TempoMap = null;
    }
}
