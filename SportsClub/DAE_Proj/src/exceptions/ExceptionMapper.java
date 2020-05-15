package exceptions;

import javax.ws.rs.core.Response;

public abstract class ExceptionMapper<E extends Exception> implements javax.ws.rs.ext.ExceptionMapper<E> {

    @Override
    public Response toResponse(E e) {
        return Response.status(Response.Status.INTERNAL_SERVER_ERROR).build();
    }
}
