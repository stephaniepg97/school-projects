package exceptions;

import javax.ws.rs.ext.Provider;

@Provider
public class MyRoleNotFoundExceptionMapper extends exceptions.ExceptionMapper<MyRoleNotFoundException> {
}

