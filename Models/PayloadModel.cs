namespace kd_backend.Models;

public class PayloadModel
{
    /*
    docker build `
    -f .\kd-backend\Dockerfile `
    -t kd-backend:latest `
    .
    */

    public string ParticipantId { get; set; }
    public string ExperimentType{ get; set; }
    public int ExperimentAttempt { get; set; }
    public string Prompt { get; set; }
    public List<List<int>> Highlights { get; set; }
    public List<List<int>> Lowlights { get; set; }
    public List<KeystrokeDto> Keystrokes { get; set; }
}