package ejbs;

import dtos.AthleteDTO;
import entities.Athlete;

import javax.annotation.PostConstruct;
import javax.ejb.EJB;
import javax.ejb.EJBException;
import javax.ejb.Stateless;

@Stateless(name = "AthleteEJB")
public class AthleteBean extends AuthBean<Athlete, AthleteDTO> {

    @Override
    @PostConstruct
    protected void init() {
        initialize(Athlete.class, AthleteDTO.class);
    }
}
