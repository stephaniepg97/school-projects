package exceptions;

public class MyRoleNotFoundException extends Exception {

    public MyRoleNotFoundException(String username) {
        super(new StringBuilder("User with username: '").append(username).append("' does not have any roles").toString());
    }
}
