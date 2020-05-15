package ejbs;

import dtos.UserDTO;
import entities.User;
import exceptions.*;

import javax.annotation.PostConstruct;
import javax.ejb.EJBException;
import javax.ejb.Stateless;
import java.util.List;

@Stateless(name = "UserEJB")
public class UserBean extends AuthBean<User, UserDTO> {

    public void removeUser(String username) throws MyEntityNotFoundException {
        try{
            User user =  em.find(User.class, username);
            if(user == null){
                throw new MyEntityNotFoundException(User.class, username );
            }
            em.remove(user);
        } catch (MyEntityNotFoundException e) {
            throw e;
        } catch (Exception e) {
            throw new EJBException("ERROR_REMOVING_USER ----> " + e.getMessage());
        }
    }

    public List<User> filter(String type) {
        return null;
    }

    @Override
    @PostConstruct
    protected void init() {
        this.initialize(User.class, UserDTO.class);
    }
}
