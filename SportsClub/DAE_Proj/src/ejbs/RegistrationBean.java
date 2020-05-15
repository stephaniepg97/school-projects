package ejbs;

import dtos.AthleteDTO;
import dtos.ProductDTO;
import dtos.RegistrationDTO;
import entities.Athlete;
import entities.Product;
import entities.Registration;

import javax.annotation.PostConstruct;
import javax.ejb.EJB;
import javax.ejb.Stateless;
import java.util.Set;

@Stateless(name = "RegistrationEJB")
public class RegistrationBean extends Bean<Registration, RegistrationDTO> {

    @EJB
    private AthleteBean athleteBean;

    @EJB
    private ProductBean productBean;

    @Override
    @PostConstruct
    protected void init() {
        initialize(Registration.class, RegistrationDTO.class);
    }

    @Override
    public Object primaryKey(RegistrationDTO dto) {
        return dto.getId();
    }

    @Override
    protected RegistrationDTO map(Registration entity, RegistrationDTO dto) {
        dto.setId(entity.getId());
        dto.setAthlete(athleteBean.toDTO(entity.getAthlete()));
        return dto;
    }

    @Override
    protected Registration map(RegistrationDTO dto, Registration entity) {
        entity.setId(dto.getId());
        entity.setAthlete(athleteBean.toEntity(dto.getAthlete()));
        return entity;
    }
}
