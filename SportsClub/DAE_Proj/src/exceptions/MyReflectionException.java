package exceptions;

public class MyReflectionException extends Exception {

    public MyReflectionException(Class generic, String operation) {
        super(new StringBuilder("REFLECTION_ERROR_")
                .append(operation.toUpperCase())
                .append(": ").append(generic != null ? generic.getSimpleName().toUpperCase() : "(GENERIC)").toString());
    }
}
