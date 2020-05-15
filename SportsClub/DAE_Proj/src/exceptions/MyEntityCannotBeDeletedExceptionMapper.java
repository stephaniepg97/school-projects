package exceptions;
import javax.ws.rs.ext.Provider;

@Provider
public class MyEntityCannotBeDeletedExceptionMapper extends ExceptionMapper<MyEntityCannotBeDeletedException> {
}