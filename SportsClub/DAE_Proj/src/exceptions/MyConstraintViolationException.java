package exceptions;

public class MyConstraintViolationException extends Exception {

    public MyConstraintViolationException(String message) {
        super(message);
    }
}
