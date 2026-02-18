namespace TeamsTranscriptBot.Models;

public class TranscriptEntry
{
    public string Speaker { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public override string ToString() =>
        $"[{StartTime:hh\\:mm\\:ss}] **{Speaker}**: {Text}";
}
