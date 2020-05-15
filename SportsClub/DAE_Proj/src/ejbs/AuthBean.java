package ejbs;

import dtos.AuthDTO;
import dtos.UserDTO;
import entities.User;
import exceptions.*;

import javax.ejb.EJBException;
import javax.persistence.MappedSuperclass;

@MappedSuperclass
public abstract class AuthBean<E extends User, D extends UserDTO> extends Bean<E, D> {

    public E authenticate(AuthDTO dto) throws MyEntityNotFoundException, MyAuthorizationException {
        try {
            if (dto == null) return null;
            E user = find(dto.getUsername());
            if (!user.getPassword().equals(User.hashPassword(dto.getPassword()))) throw new MyAuthorizationException("User", dto.getUsername(), "authenticate", user.getClass(), user.getUsername());
            return user;
        }
        catch (MyEntityNotFoundException | MyAuthorizationException e) {
            throw e;
        }
        catch (Exception e) {
            throw new EJBException(e.getMessage());
        }
    }

    @Override
    public Object primaryKey(D dto) {
        return dto.getUsername();
    }

    @Override
    protected E map(D dto, E entity)  {
        entity.setUsername(dto.getUsername());
        entity.setPassword(dto.getPassword());
        entity.setName(dto.getName());
        entity.setEmail(dto.getEmail());
        return entity;
    }

    @Override
    protected D map(E entity, D dto) {
        dto.setUsername(entity.getUsername());
        dto.setPassword(entity.getPassword());
        dto.setName(entity.getName());
        dto.setEmail(entity.getEmail());
        return dto;
    }
}
