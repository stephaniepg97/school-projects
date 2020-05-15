package ws;

import dtos.CoachDTO;
import ejbs.CoachBean;
import exceptions.*;

import javax.annotation.security.RolesAllowed;
import javax.ejb.EJB;
import javax.ws.rs.*;
import javax.ws.rs.core.*;
import java.util.Set;
import java.util.stream.Collectors;

@Path("/coaches")
@Produces({MediaType.APPLICATION_JSON})
@Consumes({MediaType.APPLICATION_JSON})
public class CoachController {

    @EJB
    private CoachBean coachBean;

    @Context
    private SecurityContext securityContext;

    @GET
    @RolesAllowed({"Administrator"})
    public Response all() throws MyReflectionException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<Set<CoachDTO>>(coachBean.toDTOs(coachBean.all().stream().collect(Collectors.toSet()))) {}).build();
    }

    @GET
    @Path("{username}")
    public Response get(@PathParam("username") String username) throws MyEntityNotFoundException, MyAuthorizationException, MyReflectionException {
        //if(!securityContext.getUserPrincipal().getName().equals(username) /*|| role(username) != user.getType()*/) throw new MyAuthorizationException(/*user.getType()*/ null, username, "get details", user.getClass(), user.getUsername());
        return Response.status(Response.Status.OK).entity(coachBean.toDTO(coachBean.find(username))).build();
    }

    @POST
    @Path("create")
    @RolesAllowed({"Administrator"})
    public Response create(CoachDTO dto) throws MyEntityExistsException, MyConstraintViolationException {
        return Response.status(Response.Status.CREATED).entity(coachBean.create(dto)).build();
    }
}
