package exceptions;

import javax.ws.rs.core.Response;
import javax.ws.rs.ext.Provider;

@Provider
public class MyAuthenticationExceptionMapper extends ExceptionMapper<MyAuthenticationException> {

    @Override
    public Response toResponse(MyAuthenticationException e) {
        return Response.status(Response.Status.FORBIDDEN).build();
    }
}
