package ejbs;

import dtos.AssociateDTO;
import entities.Associate;

import javax.annotation.PostConstruct;
import javax.ejb.EJB;
import javax.ejb.EJBException;
import javax.ejb.Stateless;

@Stateless(name = "AssociateEJB")
public class AssociateBean extends AuthBean<Associate, AssociateDTO> {

    @Override
    @PostConstruct
    protected void init() {
        initialize(Associate.class, AssociateDTO.class);
    }
}
