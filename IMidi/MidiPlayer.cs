using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;

namespace IMidi;

public class MidiPlayer
{
    public static Playback Playback;
    public static MetricTimeSpan Time;
    public static float Seconds;

    public static void OnEventReceived(object sender, MidiEventPlayedEventArgs e)
    {
        var eType = e.Event.EventType;
        
        switch (eType)
        {
            case MidiEventType.NoteOn:
                InputManager.OnKeyPress((NoteOnEvent)e.Event);
                break;
            case MidiEventType.NoteOff:
                InputManager.OnKeyRelease((NoteOffEvent)e.Event);
                break;
        }
    }

    public static void OnCurrentTimeChanged(object sender, PlaybackCurrentTimeChangedEventArgs e)
    {
        foreach (var playbackTime in e.Times)
        {
            var s = (MidiTimeSpan)playbackTime.Time;
            Time = TimeConverter.ConvertTo<MetricTimeSpan>((ITimeSpan)s, MidiFileData.TempoMap);
            Seconds = (float)Time.TotalSeconds;
        }
    }

    public static void ClearPlayback()
    {
        if (Playback == null)
            return;

        Playback.Stop();
        Playback.EventPlayed -= OnEventReceived;

        PlaybackCurrentTimeWatcher.Instance.Stop();
        PlaybackCurrentTimeWatcher.Instance.CurrentTimeChanged -= OnCurrentTimeChanged;
        PlaybackCurrentTimeWatcher.Instance.RemovePlayback(Playback);
        Playback = null;

        MidiFileData.ReleaseMidiFile();
    }
}
