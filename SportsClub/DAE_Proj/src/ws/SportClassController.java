package ws;

import dtos.SportClassDTO;
import ejbs.SportClassBean;
import exceptions.*;

import javax.annotation.security.RolesAllowed;
import javax.ejb.EJB;
import javax.ws.rs.*;
import javax.ws.rs.core.GenericEntity;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;

@Path("/classes")
@Produces({MediaType.APPLICATION_JSON})
@Consumes({MediaType.APPLICATION_JSON})
public class SportClassController {

    @EJB
    private SportClassBean sportClassBean;

    @GET
    @Path("/{id}")
    public Response get(@PathParam("id") Integer id) throws MyReflectionException, MyEntityNotFoundException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<SportClassDTO>(sportClassBean.toDTO(sportClassBean.find(id))){}).build();
    }

    @POST
    @Path("/")
    @RolesAllowed({"Administrator"})
    public Response create(SportClassDTO sportClassDTO) throws MyEntityExistsException, MyConstraintViolationException {
        return Response.status(Response.Status.CREATED).entity(new GenericEntity<SportClassDTO>(sportClassBean.create(sportClassDTO)){}).build();
    }

    @DELETE
    @Path("{id}")
    @RolesAllowed({"Administrator"})
    public Response delete(@PathParam("id") Integer id) throws MyEntityNotFoundException, MyReflectionException, MyEntityCannotBeDeletedException {
        sportClassBean.delete(id);
        return Response.status(Response.Status.OK).build();
    }

    @PUT
    @Path("{id}")
    @RolesAllowed({"Administrator"})
    public Response update(@PathParam("id") Integer id, SportClassDTO sportClassDTO) throws MyConstraintViolationException, MyEntityNotFoundException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<SportClassDTO>(sportClassBean.update(sportClassDTO)){}).build();
    }
}
