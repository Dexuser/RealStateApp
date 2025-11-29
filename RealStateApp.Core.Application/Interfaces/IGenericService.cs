namespace RealStateApp.Core.Application.Interfaces;

public interface IGenericService<TDtoModel>
where TDtoModel: class
{
    Task<TDtoModel?> GetByIdAsync(int id);
    Task<List<TDtoModel>> GetAllAsync();
    Task<Result<TDtoModel>> AddAsync(TDtoModel dto);
    Task<Result<List<TDtoModel>>> AddRangeAsync(List<TDtoModel> dto);
    Task<Result<TDtoModel>> UpdateAsync(int id, TDtoModel dto);
    Task<Result> DeleteAsync(int id);
}