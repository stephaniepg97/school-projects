package exceptions;

public class MyEntityCannotBeDeletedException extends Exception{

    public MyEntityCannotBeDeletedException(Class entity) {
        super(new StringBuilder(entity.getSimpleName()).append(" cannot be deleted").toString());
    }
}
