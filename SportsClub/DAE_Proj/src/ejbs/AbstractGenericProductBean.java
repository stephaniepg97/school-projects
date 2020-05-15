package ejbs;

import dtos.CategoryDTO;
import dtos.GenericProductDTO;
import entities.Category;
import entities.GenericProduct;

import javax.ejb.EJB;
import javax.persistence.MappedSuperclass;

@MappedSuperclass
public abstract class AbstractGenericProductBean<E extends GenericProduct, D extends GenericProductDTO> extends Bean<E, D> {

    @EJB
    protected CategoryBean categoryBean;

    @Override
    protected D map(E entity, D dto) {
        dto.setId(entity.getId());
        Category category = entity.getCategory();
        dto.setCategory(categoryBean.toDTO(category));
        dto.setName(entity.getName());
        dto.setOriginalId(entity.getOriginalId());
        dto.setValue(entity.getValue());
        return dto;
    }

    @Override
    protected E map(D dto, E entity) {
        entity.setId(dto.getId());
        CategoryDTO category = dto.getCategory();
        entity.setCategory(categoryBean.toEntity(category));
        entity.setName(dto.getName());
        entity.setOriginalId(dto.getOriginalId());
        entity.setValue(dto.getValue());
        entity.setStock(dto.getStock());
        return entity;
    }

    protected D clone(Class<D> dtoClass, D dto) throws IllegalAccessException, InstantiationException {
        if (dtoClass == null || dto == null) return null;
        D clone = dtoClass.newInstance();
        clone.setCategory(dto.getCategory());
        clone.setName(dto.getName());
        clone.setValue(dto.getValue());
        return clone;
    }

    @Override
    public Object primaryKey(D dto) {
        return dto.getId();
    }
}
