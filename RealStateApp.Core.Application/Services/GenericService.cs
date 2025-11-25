using AutoMapper;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class GenericServices<TEntity, TDtoModel> : IGenericService<TDtoModel>
    where TEntity : class
    where TDtoModel : class

{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<TEntity> _repository;

    public GenericServices(IGenericRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        this._mapper = mapper;
    }

    public virtual async Task<List<TDtoModel>> GetAllAsync()
    {
        try
        {
            var dtos = _mapper.Map<List<TDtoModel>>(await _repository.GetAllAsync());
            return dtos;
        }
        catch (Exception ex)
        {
            return [];
        }
    }

    public virtual async Task<TDtoModel?> GetByIdAsync(int id)
    {
        try
        {
            TEntity? entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<TDtoModel>(entity);
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public virtual async Task<TDtoModel?> AddAsync(TDtoModel dtoModel)
    {
        try
        {
            TEntity entity = _mapper.Map<TEntity>(dtoModel);
            TEntity? returnEntity = await _repository.AddAsync(entity);
            if (returnEntity == null)
            {
                return null;
            }

            TDtoModel dto = _mapper.Map<TDtoModel>(returnEntity);
            return dto;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public virtual async Task<List<TDtoModel>> AddRangeAsync(List<TDtoModel> dtomodels)
    {
        try
        {
            List<TEntity> entity = _mapper.Map<List<TEntity>>(dtomodels);
            List<TEntity> returnEntities = await _repository.AddRangeAsync(entity);

            List<TDtoModel> dtos = _mapper.Map<List<TDtoModel>>(returnEntities);
            return dtos;
        }
        catch (Exception e)
        {
            return [];
        }
    }

    public virtual async Task<TDtoModel?> UpdateAsync(int id, TDtoModel dtoModel)
    {
        try
        {
            TEntity entity = _mapper.Map<TEntity>(dtoModel);
            TEntity? returnEntity = await _repository.UpdateAsync(id, entity);
            if (returnEntity == null)
            {
                return null;
            }

            var dto = _mapper.Map<TDtoModel>(returnEntity);
            return dto;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        try
        {
            await _repository.DeleteAsync(id);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}