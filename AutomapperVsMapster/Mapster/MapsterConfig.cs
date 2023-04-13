using AutomapperVsMapster.Models;
using AutomapperVsMapster.ViewModels;
using Mapster;

namespace AutomapperVsMapster.Mapster;

internal class MapsterConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Photo, PhotoVm>()
            .Map(dest => dest.AlbumId, src => src.AlbumId)
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Url, src => src.Url)
            .Map(dest => dest.ThumbnailUrl, src => src.ThumbnailUrl);
    }
}
