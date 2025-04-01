using kd_backend.Models;
using System.Globalization;

namespace kd_backend.Services;

public class DataProcessingService
{
    public string ProcessPayloadAndSaveCsv(PayloadModel payload)
    {
        var csvLines = new List<string>();
        // CSV-Header
        csvLines.Add("PARTICIPANT_ID,PROMPT,KEYSTROKE_ID,PRESS_TIME,RELEASE_TIME,LETTER,KEYCODE");

        foreach (var keystroke in payload.Keystrokes)
        {
            var line = $"{payload.ParticipantId},{Escape(payload.Prompt)},{keystroke.KeystrokeId},{keystroke.PressTime},{keystroke.ReleaseTime},{Escape(keystroke.Letter)},{keystroke.Keycode}";
            csvLines.Add(line);
        }

        var csvContent = string.Join(Environment.NewLine, csvLines);

        // Erzeugen eines eindeutigen Dateinamens
        var fileName = $"experiment_{payload.ParticipantId}_{DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture)}.csv";
        var folder = "DataFiles";
        Directory.CreateDirectory(folder);
        var filePath = Path.Combine(folder, fileName);

        File.WriteAllText(filePath, csvContent);
        return filePath;
    }

    private string Escape(string input)
    {
        if (input.Contains(',') || input.Contains('\"'))
        {
            return "\"" + input.Replace("\"", "\"\"") + "\"";
        }
        return input;
    }
}