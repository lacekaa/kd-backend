namespace kd_backend.Models;

public class PayloadModel
{
    public string ParticipantId { get; set; }
    public string Prompt { get; set; }
    public string[] Highlights { get; set; }
    public List<KeystrokeDto> Keystrokes { get; set; }
}