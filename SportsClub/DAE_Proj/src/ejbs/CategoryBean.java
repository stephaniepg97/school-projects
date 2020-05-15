package ejbs;

import dtos.CategoryDTO;
import dtos.GenericProductDTO;
import entities.Category;
import entities.GenericProduct;
import entities.Product;
import exceptions.MyReflectionException;

import javax.annotation.PostConstruct;
import javax.ejb.EJB;
import javax.ejb.EJBException;
import javax.ejb.Stateless;
import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import java.util.Set;

@Stateless(name = "CategoryEJB")
public class CategoryBean extends Bean<Category, CategoryDTO> {

    @EJB
    private GenericProductBean genericProductBean;

    @Override
    public Object primaryKey(CategoryDTO dto) {
        return dto.getName();
    }

    @Override
    protected CategoryDTO map(Category entity, CategoryDTO dto) {
        dto.setName(entity.getName());
        return dto;
    }

    @Override
    protected Category map(CategoryDTO dto, Category entity) {
        entity.setName(dto.getName());
        return entity;
    }

    @Override
    @PostConstruct
    protected void init() {
        initialize(Category.class, CategoryDTO.class);
    }
}
