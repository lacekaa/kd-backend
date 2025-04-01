namespace kd_backend.Models;

public class KeystrokeDto
{
    public int KeystrokeId { get; set; }
    public long PressTime { get; set; }
    public long ReleaseTime { get; set; }
    public string Letter { get; set; }
    public int Keycode { get; set; }
}