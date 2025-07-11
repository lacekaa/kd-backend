﻿using kd_backend.Models;
using System.Globalization;

namespace kd_backend.Services;

public class DataProcessingService
{
    public string ProcessPayloadAndSaveCsv(PayloadModel payload)
    {
        var csvLines = new List<string>();
        // CSV-Header
        csvLines.Add(
            "PARTICIPANT_ID,EXPERIMENTTYPE,EXPERIMENTATTEMPT,TOTALATTEMPTCOUNT,LATINSQUARETYPE,PROMPT,HIGHLIGHTS,UNIMPORTANT,KEYSTROKE_ID,PRESS_TIME,RELEASE_TIME,LETTER,KEYCODE,FREQUENCY");

        foreach (var keystroke in payload.Keystrokes)
        {
            var highlights = string.Join(";", payload.Highlights.Select(h => $"[{h[0]},{h[1]}]"));
            var lowlights = string.Join(";", payload.Lowlights.Select(h => $"[{h[0]},{h[1]}]"));
            var line =
                $"{payload.ParticipantId},{payload.ExperimentType},{payload.ExperimentAttempt},{payload.totalAttempt},{keystroke.LatinSquareType},{Escape(payload.Prompt)},\"{highlights}\",\"{lowlights}\",{keystroke.KeystrokeId},{keystroke.PressTime},{keystroke.ReleaseTime},{Escape(keystroke.Letter)},{keystroke.Keycode}, {keystroke.Frequency}";
            csvLines.Add(line);
        }

        var csvContent = string.Join(Environment.NewLine, csvLines);

        // Erzeugen eines eindeutigen Dateinamens
        var fileName =
            $"experiment_{payload.ParticipantId}_{DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture)}.csv";
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