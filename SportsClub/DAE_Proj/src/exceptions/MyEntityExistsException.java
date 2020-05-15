package exceptions;

import javax.ejb.EJBException;

public class MyEntityExistsException extends Exception {

    public MyEntityExistsException(Class entity, Object primaryKey) {
        super(new StringBuilder(entity == null ? "entity" : entity.getSimpleName()).append(" with primary key: '").append(primaryKey).append("' already exists").toString());
    }
}
