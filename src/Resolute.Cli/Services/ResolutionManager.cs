using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.Models;

namespace ConsoleApp.Services;

public class ResolutionManager
{
    private readonly JsonDataService _dataService;
    private ResolutionData _data;

    public ResolutionManager(JsonDataService dataService)
    {
        _dataService = dataService;
        _data = new ResolutionData();
    }

    public async Task InitializeAsync()
    {
        _data = await _dataService.LoadDataAsync();
    }

    public async Task<Resolution> CreateResolutionAsync(Resolution resolution)
    {
        resolution.Id = Guid.NewGuid();
        resolution.CreatedDate = DateTime.Now;
        _data.Resolutions.Add(resolution);
        await _dataService.SaveDataAsync(_data);
        return resolution;
    }

    public IEnumerable<Resolution> GetAllResolutions()
    {
        return _data.Resolutions;
    }

    public Resolution? GetResolutionById(Guid id)
    {
        return _data.Resolutions.FirstOrDefault(r => r.Id == id);
    }

    public IEnumerable<Resolution> GetActiveResolutions()
    {
        return _data.Resolutions.Where(r => !r.IsCompleted);
    }

    public IEnumerable<Resolution> GetCompletedResolutions()
    {
        return _data.Resolutions.Where(r => r.IsCompleted);
    }

    public IEnumerable<Resolution> GetResolutionsByCategory(string category)
    {
        return _data.Resolutions.Where(r =>
            r.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<bool> UpdateResolutionAsync(Resolution resolution)
    {
        var existing = _data.Resolutions.FirstOrDefault(r => r.Id == resolution.Id);
        if (existing == null)
        {
            return false;
        }

        var index = _data.Resolutions.IndexOf(existing);
        _data.Resolutions[index] = resolution;
        await _dataService.SaveDataAsync(_data);
        return true;
    }

    public async Task<bool> DeleteResolutionAsync(Guid id)
    {
        var resolution = _data.Resolutions.FirstOrDefault(r => r.Id == id);
        if (resolution == null)
        {
            return false;
        }

        _data.Resolutions.Remove(resolution);
        await _dataService.SaveDataAsync(_data);
        return true;
    }

    public async Task<bool> CompleteResolutionAsync(Guid id)
    {
        var resolution = _data.Resolutions.FirstOrDefault(r => r.Id == id);
        if (resolution == null)
        {
            return false;
        }

        resolution.CompletedDate = DateTime.Now;
        await _dataService.SaveDataAsync(_data);
        return true;
    }

    public async Task<bool> AddCheckInAsync(Guid resolutionId, CheckIn checkIn)
    {
        var resolution = _data.Resolutions.FirstOrDefault(r => r.Id == resolutionId);
        if (resolution == null)
        {
            return false;
        }

        checkIn.Date = DateTime.Now;
        resolution.CheckIns.Add(checkIn);

        if (checkIn.Status == CheckInStatus.Completed)
        {
            resolution.CompletedDate = DateTime.Now;
        }

        await _dataService.SaveDataAsync(_data);
        return true;
    }

    public IEnumerable<CheckIn> GetCheckInsForResolution(Guid resolutionId)
    {
        var resolution = _data.Resolutions.FirstOrDefault(r => r.Id == resolutionId);
        return resolution?.CheckIns ?? Enumerable.Empty<CheckIn>();
    }
}
