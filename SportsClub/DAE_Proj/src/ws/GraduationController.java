package ws;

import dtos.GraduationDTO;
import ejbs.GraduationBean;
import ejbs.ProductBean;
import exceptions.*;

import javax.annotation.security.RolesAllowed;
import javax.ejb.EJB;
import javax.ws.rs.*;
import javax.ws.rs.core.GenericEntity;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;

@Path("/graduations")
@Produces({MediaType.APPLICATION_JSON})
@Consumes({MediaType.APPLICATION_JSON})
public class GraduationController {

    @EJB
    private ProductBean productBean;

    @EJB
    private GraduationBean graduationBean;

    @GET
    @Path("/{id}")
    public Response get(@PathParam("id") Integer id) throws MyReflectionException, MyEntityNotFoundException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<GraduationDTO>(graduationBean.toDTO(graduationBean.find(id))){}).build();
    }

    @POST
    @Path("/")
    @RolesAllowed({"Administrator"})
    public Response create(GraduationDTO graduationDTO) throws MyEntityExistsException, MyConstraintViolationException {
        return Response.status(Response.Status.CREATED).entity(new GenericEntity<GraduationDTO>(graduationBean.create(graduationDTO)){}).build();
    }

    @DELETE
    @Path("{id}")
    @RolesAllowed({"Administrator"})
    public Response delete(@PathParam("id") Integer id) throws MyEntityNotFoundException, MyReflectionException, MyEntityCannotBeDeletedException {
        productBean.delete(id);
        return Response.status(Response.Status.OK).build();
    }

    @PUT
    @Path("{id}")
    @RolesAllowed({"Administrator"})
    public Response update(@PathParam("id") Integer id, GraduationDTO graduationDTO) throws MyConstraintViolationException, MyEntityNotFoundException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<GraduationDTO>(graduationBean.update(graduationDTO)){}).build();
    }
}
