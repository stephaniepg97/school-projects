package ws;

import dtos.RankDTO;
import ejbs.RankBean;
import exceptions.*;

import javax.annotation.security.RolesAllowed;
import javax.ejb.EJB;
import javax.ws.rs.*;
import javax.ws.rs.core.GenericEntity;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import java.util.HashSet;
import java.util.Set;

@Path("/ranks")
@Produces({MediaType.APPLICATION_JSON})
@Consumes({MediaType.APPLICATION_JSON})
public class RankController {

    @EJB
    private RankBean rankBean;

    @GET
    @RolesAllowed({"Administrator"})
    public Response all() throws MyReflectionException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<Set<RankDTO>>(rankBean.toDTOs(new HashSet<>(rankBean.all()))) {}).build();
    }

    @POST
    @Path("/")
    @RolesAllowed({"Administrator"})
    public Response create(RankDTO rankDTO) throws MyEntityExistsException, MyConstraintViolationException {
        return Response.status(Response.Status.CREATED).entity(new GenericEntity<RankDTO>(rankBean.create(rankDTO)){}).build();
    }

    @DELETE
    @Path("{id}")
    @RolesAllowed({"Administrator"})
    public Response delete(@PathParam("id") Integer id) throws MyEntityNotFoundException, MyReflectionException, MyEntityCannotBeDeletedException {
        rankBean.delete(id);
        return Response.status(Response.Status.OK).build();
    }

    @PUT
    @Path("{id}")
    @RolesAllowed({"Administrator"})
    public Response update(@PathParam("id") Integer id, RankDTO rankDTO) throws MyConstraintViolationException, MyEntityNotFoundException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<RankDTO>(rankBean.update(rankDTO)){}).build();
    }
}
