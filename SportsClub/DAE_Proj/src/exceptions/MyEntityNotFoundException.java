package exceptions;

public class MyEntityNotFoundException extends Exception {

    public MyEntityNotFoundException(Class entity, Object primaryKey) {
        super(new StringBuilder(entity.getSimpleName()).append(" with primary key: '").append(primaryKey).append("' does not exists").toString());
    }
}
