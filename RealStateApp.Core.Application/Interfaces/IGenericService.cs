namespace RealStateApp.Core.Application.Interfaces;

public interface IGenericService<TDtoModel>
where TDtoModel: class
{
    Task<TDtoModel?> GetByIdAsync(int id);
    Task<List<TDtoModel>> GetAllAsync();
    Task<TDtoModel?> AddAsync(TDtoModel dto);
    Task<List<TDtoModel>> AddRangeAsync(List<TDtoModel> dto);
    Task<TDtoModel?> UpdateAsync(int id, TDtoModel dto);
    Task<bool> DeleteAsync(int id);
}