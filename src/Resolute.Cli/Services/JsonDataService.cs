using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using ConsoleApp.Models;

namespace ConsoleApp.Services;

public class JsonDataService
{
    private readonly string _dataFilePath;
    private readonly JsonSerializerOptions _jsonOptions;

    public JsonDataService(string dataFilePath = "resolutions.json")
    {
        _dataFilePath = dataFilePath;
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
    }

    public async Task<ResolutionData> LoadDataAsync()
    {
        try
        {
            if (!File.Exists(_dataFilePath))
            {
                return new ResolutionData();
            }

            var json = await File.ReadAllTextAsync(_dataFilePath);
            return JsonSerializer.Deserialize<ResolutionData>(json, _jsonOptions)
                   ?? new ResolutionData();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data: {ex.Message}");
            return new ResolutionData();
        }
    }

    public async Task SaveDataAsync(ResolutionData data)
    {
        try
        {
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            await File.WriteAllTextAsync(_dataFilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving data: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> BackupDataAsync()
    {
        try
        {
            if (!File.Exists(_dataFilePath))
            {
                return false;
            }

            var backupPath = $"{_dataFilePath}.backup_{DateTime.Now:yyyyMMdd_HHmmss}";
            File.Copy(_dataFilePath, backupPath);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating backup: {ex.Message}");
            return false;
        }
    }
}
