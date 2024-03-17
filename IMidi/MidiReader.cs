using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;

namespace IMidi;

public class MidiReader
{
    private static MidiFile ReadMidiFile(string filePath)
    {
        return MidiFile.Read(filePath);
    }

    public static void OpenMidiDialog()
    {
        OpenFileDialog dialog = new()
        {
            Title = "Select a midi file",
            Filter = "midi files (*.mid)|*.mid"
        };
        dialog.ShowOpenFileDialog();

        if (dialog.Success)
        {
            var file = new FileInfo(dialog.Files.First());
            MidiFileData.FileName = file.Name;           
            var midiFile = ReadMidiFile(file.FullName);
            MidiFileData.MidiFile = midiFile;
            MidiFileData.TempoMap = midiFile.GetTempoMap();

            if (MidiPlayer.Playback != null)
            {
                MidiPlayer.Playback.Stop();
                MidiPlayer.Playback.EventPlayed -= MidiPlayer.OnEventReceived;

                PlaybackCurrentTimeWatcher.Instance.Stop();
                PlaybackCurrentTimeWatcher.Instance.CurrentTimeChanged -= MidiPlayer.OnCurrentTimeChanged;
                PlaybackCurrentTimeWatcher.Instance.RemovePlayback(MidiPlayer.Playback);
            }        

            MidiPlayer.Playback = midiFile.GetPlayback(DevicesManager.OutputDevice);
            MidiPlayer.Playback.EventPlayed += MidiPlayer.OnEventReceived;
           
            PlaybackCurrentTimeWatcher.Instance.AddPlayback(MidiPlayer.Playback, TimeSpanType.Midi);
            PlaybackCurrentTimeWatcher.Instance.CurrentTimeChanged += MidiPlayer.OnCurrentTimeChanged;
            PlaybackCurrentTimeWatcher.Instance.Start();
        }
    }
}
