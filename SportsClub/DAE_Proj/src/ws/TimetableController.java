package ws;

import dtos.TimetableDTO;
import ejbs.TimetableBean;
import exceptions.*;

import javax.annotation.security.RolesAllowed;
import javax.ejb.EJB;
import javax.ws.rs.*;
import javax.ws.rs.core.GenericEntity;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import java.util.HashSet;
import java.util.Set;

@Path("/timetables")
@Produces({MediaType.APPLICATION_JSON})
@Consumes({MediaType.APPLICATION_JSON})
public class TimetableController {

    @EJB
    private TimetableBean timetableBean;

    @GET
    @RolesAllowed({"Administrator"})
    public Response all() throws MyReflectionException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<Set<TimetableDTO>>(timetableBean.toDTOs(new HashSet<>(timetableBean.all()))) {}).build();
    }

    @POST
    @Path("/")
    @RolesAllowed({"Administrator"})
    public Response create(TimetableDTO timetableDTO) throws MyEntityExistsException, MyConstraintViolationException {
        return Response.status(Response.Status.CREATED).entity(new GenericEntity<TimetableDTO>(timetableBean.create(timetableDTO)){}).build();
    }

    @DELETE
    @Path("{id}")
    @RolesAllowed({"Administrator"})
    public Response delete(@PathParam("id") Integer id) throws MyEntityNotFoundException, MyReflectionException, MyEntityCannotBeDeletedException {
        timetableBean.delete(id);
        return Response.status(Response.Status.OK).build();
    }

    @PUT
    @Path("{id}")
    @RolesAllowed({"Administrator"})
    public Response update(@PathParam("id") Integer id, TimetableDTO timetableDTO) throws MyConstraintViolationException, MyEntityNotFoundException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<TimetableDTO>(timetableBean.update(timetableDTO)){}).build();
    }
}
