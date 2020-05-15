package ws;

import dtos.SeasonDTO;
import ejbs.SeasonBean;
import exceptions.*;

import javax.annotation.security.RolesAllowed;
import javax.ejb.EJB;
import javax.ws.rs.*;
import javax.ws.rs.core.GenericEntity;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import java.util.HashSet;
import java.util.Set;

@Path("/seasons")
@Produces({MediaType.APPLICATION_JSON})
@Consumes({MediaType.APPLICATION_JSON})
public class SeasonController {

    @EJB
    private SeasonBean seasonBean;

    @GET
    @RolesAllowed({"Administrator"})
    public Response all() throws MyReflectionException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<Set<SeasonDTO>>(seasonBean.toDTOs(new HashSet<>(seasonBean.all()))) {}).build();
    }

    @POST
    @Path("/")
    @RolesAllowed({"Administrator"})
    public Response create(SeasonDTO seasonDTO) throws MyEntityExistsException, MyConstraintViolationException {
        return Response.status(Response.Status.CREATED).entity(new GenericEntity<SeasonDTO>(seasonBean.create(seasonDTO)){}).build();
    }

    @DELETE
    @Path("{id}")
    @RolesAllowed({"Administrator"})
    public Response delete(@PathParam("id") Integer id) throws MyEntityNotFoundException, MyReflectionException, MyEntityCannotBeDeletedException {
        seasonBean.delete(id);
        return Response.status(Response.Status.OK).build();
    }

    @PUT
    @Path("{id}")
    @RolesAllowed({"Administrator"})
    public Response update(@PathParam("id") Integer id, SeasonDTO seasonDTO) throws MyConstraintViolationException, MyEntityNotFoundException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<SeasonDTO>(seasonBean.update(seasonDTO)){}).build();
    }
}
