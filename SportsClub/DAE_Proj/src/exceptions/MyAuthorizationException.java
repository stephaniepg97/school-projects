package exceptions;

public class MyAuthorizationException extends Exception {

    public MyAuthorizationException(String role, String username, String operation, Class entity, Object primaryKey) {
        super(new StringBuilder(role)
                .append(" with username: '")
                .append(username)
                .append("' is not allowed to ")
                .append(operation.toUpperCase())
                .append(":\t")
                .append(entity.getSimpleName())
                .append("(")
                .append(primaryKey)
                .append(")")
                .toString());
    }
}
