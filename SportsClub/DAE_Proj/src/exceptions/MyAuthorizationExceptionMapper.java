package exceptions;

import javax.ws.rs.core.Response;
import javax.ws.rs.ext.Provider;

@Provider
public class MyAuthorizationExceptionMapper extends ExceptionMapper<MyAuthorizationException> {

    @Override
    public Response toResponse(MyAuthorizationException e) {
        return Response.status(Response.Status.UNAUTHORIZED).build();
    }
}
