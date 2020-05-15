package exceptions;

import javax.ws.rs.ext.Provider;

@Provider
public class MyIllegalArgumentExceptionMapper extends ExceptionMapper<MyIllegalArgumentException> {
}
