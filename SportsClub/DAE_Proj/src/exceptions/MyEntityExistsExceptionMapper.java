package exceptions;
import javax.ws.rs.ext.Provider;

@Provider
public class MyEntityExistsExceptionMapper extends ExceptionMapper<MyEntityExistsException> {
}