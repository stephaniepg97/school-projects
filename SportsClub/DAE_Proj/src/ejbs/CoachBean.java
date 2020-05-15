package ejbs;

import dtos.CoachDTO;
import entities.Coach;

import javax.annotation.PostConstruct;
import javax.ejb.EJB;
import javax.ejb.EJBException;
import javax.ejb.Stateless;

@Stateless(name = "CoachEJB")
public class CoachBean extends AuthBean<Coach, CoachDTO> {

    @Override
    @PostConstruct
    protected void init() {
        initialize(Coach.class, CoachDTO.class);
    }
}
