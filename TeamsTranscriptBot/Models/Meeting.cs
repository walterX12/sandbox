namespace TeamsTranscriptBot.Models;

public class TrackedMeeting
{
    public string MeetingId { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string OrganizerUserId { get; set; } = string.Empty;
    public DateTimeOffset StartTime { get; set; }
    public string OutputFilePath { get; set; } = string.Empty;
    public HashSet<string> ProcessedTranscriptIds { get; set; } = new();
    public bool SummaryGenerated { get; set; } = false;
}
