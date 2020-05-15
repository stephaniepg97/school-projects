package ejbs;

import dtos.CategoryDTO;
import dtos.GraduationDTO;
import entities.Graduation;
import exceptions.MyEntityNotFoundException;

import javax.annotation.PostConstruct;
import javax.ejb.EJB;
import javax.ejb.EJBException;
import javax.ejb.Stateless;

@Stateless(name = "GraduationEJB")
public class GraduationBean extends AbstractProductBean<Graduation, GraduationDTO> {

    @Override
    @PostConstruct
    protected void init() {
        initialize(Graduation.class, GraduationDTO.class);
    }
}
