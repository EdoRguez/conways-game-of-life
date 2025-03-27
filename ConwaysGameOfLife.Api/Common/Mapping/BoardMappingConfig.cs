using ConwaysGameOfLife.Contracts;
using ConwaysGameOfLife.Core.Entities;
using Mapster;

namespace ConwaysGameOfLife.Api.Common.Mapping;

public class BoardMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CellRequest, Cell>();
        config.NewConfig<CreateBoardRequest, Board>();
    }
}