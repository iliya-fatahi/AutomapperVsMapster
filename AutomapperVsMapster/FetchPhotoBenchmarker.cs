using AutoMapper;
using AutomapperVsMapster.Models;
using AutomapperVsMapster.ViewModels;
using BenchmarkDotNet.Attributes;
using Mapster;
using Newtonsoft.Json;

namespace AutomapperVsMapster;

[MemoryDiagnoser]
public class FetchPhotoBenchmarker
{

    private IMapper _mapper;
    public FetchPhotoBenchmarker()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PhotoVm, Photo>();
        });
        _mapper = config.CreateMapper();
    }
    internal List<PhotoVm> Data { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new System.Uri("https://jsonplaceholder.typicode.com/");

        var call = httpClient.GetAsync("/photos").Result;
        var data = ConvertContent<PhotoVm>(call).Result;
        Data = data;
    }

    [Benchmark]
    public void MapWithMapster()
    {
        var mapData = Data.Adapt<List<Photo>>();
    }

    [Benchmark]
    public void MapWithAutomapper()
    {
        var mapData = _mapper.Map<List<Photo>>(Data);
    }

    [Benchmark]
    public void MapWithDefault()
    {
        var mapData = Data.Select(data => new Photo(data.AlbumId,data.Id,data.Title,data.Url,data.ThumbnailUrl)).ToList();
    }

    public async Task<List<T>> ConvertContent<T>(HttpResponseMessage content)
    {
        var deserializeContent = JsonConvert.DeserializeObject<List<T>>(await content.Content.ReadAsStringAsync());
        return deserializeContent;
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        Console.WriteLine("Benchmark is cleaning up .");
    }
}
