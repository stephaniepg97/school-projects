package ejbs;

import dtos.ModalityDTO;
import dtos.ProductDTO;
import dtos.RegistrationDTO;
import entities.Modality;
import entities.Product;
import entities.Registration;

import javax.ejb.EJB;
import javax.persistence.MappedSuperclass;
import java.util.Set;

@MappedSuperclass
public abstract class AbstractProductBean<E extends Product, D extends ProductDTO> extends AbstractGenericProductBean<E, D> {

    @EJB
    protected RegistrationBean registrationBean;

    @EJB
    private ModalityBean modalityBean;

    @Override
    protected D map(E entity, D dto) {
        dto = super.map(entity, dto);
        dto.setModality(modalityBean.toDTO(entity.getModality()));
        return dto;
    }

    @Override
    protected E map(D dto, E entity) {
        entity = super.map(dto, entity);
        entity.setModality(modalityBean.toEntity(dto.getModality()));
        return entity;
    }

    @Override
    protected D clone(Class<D> dtoClass, D dto) throws InstantiationException, IllegalAccessException {
        D clone = super.clone(dtoClass, dto);
        if (clone != null) dto.setRegistrations(dto.getRegistrations());
        return clone;
    }
}
