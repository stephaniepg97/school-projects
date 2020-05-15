package exceptions;

import javax.ws.rs.ext.Provider;

@Provider
public class MyEntityNotFoundExceptionMapper extends ExceptionMapper<MyEntityNotFoundException> {
}
