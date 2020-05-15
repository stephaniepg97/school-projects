package ejbs;

import dtos.AdministratorDTO;
import entities.Administrator;
import javax.annotation.PostConstruct;
import javax.ejb.Stateless;

@Stateless(name = "AdministratorEJB")
public class AdministratorBean extends AuthBean<Administrator, AdministratorDTO> {

    @Override
    @PostConstruct
    protected void init() {
        initialize(Administrator.class, AdministratorDTO.class);
    }
}
