package exceptions;

public class MyAuthenticationException extends Exception{

    public MyAuthenticationException(String username) {
        super("Failed logging in with username '" + username + "': unknown username or wrong password");
    }
}
