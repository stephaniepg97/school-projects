package ws;

import dtos.GenericProductDTO;
import ejbs.GenericProductBean;
import exceptions.MyEntityCannotBeDeletedException;
import exceptions.MyEntityNotFoundException;
import exceptions.MyReflectionException;

import javax.ejb.EJB;
import javax.ws.rs.*;
import javax.ws.rs.core.*;
import java.util.HashSet;
import java.util.Set;

@Path("/products")
@Produces({MediaType.APPLICATION_JSON})
@Consumes({MediaType.APPLICATION_JSON})
public class GenericProductController {

    @EJB
    private GenericProductBean genericProductBean;

    @Context
    private SecurityContext securityContext;

    @GET
    @Path("/")
    public Response all() {
        return Response.status(Response.Status.OK).entity(new GenericEntity<Set<GenericProductDTO>>(genericProductBean.toDTOs(new HashSet<>(genericProductBean.allOutCopies()))) {}).build();
    }

    @GET
    @Path("/all")
    public Response allWithCopies() throws MyReflectionException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<Set<GenericProductDTO>>(genericProductBean.toDTOs(new HashSet<>(genericProductBean.all()))) {}).build();
    }

    @GET
    @Path("{id}")
    public Response get(@PathParam("id") Integer id) throws MyEntityNotFoundException, MyReflectionException {
        return Response.status(Response.Status.OK).entity(genericProductBean.find(id)).build();
    }

    @DELETE
    @Path("{id}")
    public Response delete(@PathParam("id") int id) throws MyEntityNotFoundException, MyReflectionException, MyEntityCannotBeDeletedException {
        genericProductBean.delete(id);
        return Response.status(Response.Status.OK).build();
    }

}
