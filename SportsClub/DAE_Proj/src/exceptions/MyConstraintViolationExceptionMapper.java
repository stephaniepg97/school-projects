package exceptions;

import javax.ws.rs.ext.Provider;

@Provider
public class MyConstraintViolationExceptionMapper extends exceptions.ExceptionMapper<MyConstraintViolationException> {
}
