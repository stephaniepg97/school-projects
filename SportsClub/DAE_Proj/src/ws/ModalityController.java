package ws;

import dtos.GraduationDTO;
import dtos.ModalityDTO;
import dtos.SportClassDTO;
import ejbs.GraduationBean;
import ejbs.ModalityBean;
import ejbs.SportClassBean;
import exceptions.*;

import javax.annotation.security.RolesAllowed;
import javax.ejb.EJB;
import javax.ws.rs.*;
import javax.ws.rs.core.GenericEntity;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import java.util.HashSet;
import java.util.Set;

@Path("/modalities")
@Produces({MediaType.APPLICATION_JSON})
@Consumes({MediaType.APPLICATION_JSON})
public class ModalityController {

    @EJB
    private ModalityBean modalityBean;

    @EJB
    private GraduationBean graduationBean;

    @EJB
    private SportClassBean sportClassBean;

    @GET
    @Path("/")
    public Response all() throws MyReflectionException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<Set<ModalityDTO>>(modalityBean.toDTOs(new HashSet<>(modalityBean.all()))) {}).build();
    }

    @GET
    @Path("/{id}")
    public Response get(@PathParam("id") Integer id) throws MyReflectionException, MyEntityNotFoundException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<ModalityDTO>(modalityBean.toDTO(modalityBean.find(id))){}).build();
    }

    @GET
    @Path("{id}/graduations")
    public Response graduations(@PathParam("id") Integer id) throws MyEntityNotFoundException, MyReflectionException {
        modalityBean.find(id);
        return Response.status(Response.Status.OK)
                .entity(new GenericEntity<Set<GraduationDTO>>(graduationBean.toDTOs(new HashSet<>(modalityBean.graduations(id)))) {})
                .build();
    }

    @GET
    @Path("{id}/classes")
    public Response classes(@PathParam("id") Integer id) throws MyEntityNotFoundException, MyReflectionException {
        modalityBean.find(id);
        return Response.status(Response.Status.OK)
                .entity(new GenericEntity<Set<SportClassDTO>>(sportClassBean.toDTOs(new HashSet<>(modalityBean.classes(id)))) {})
                .build();
    }

    @POST
    @Path("/")
    @RolesAllowed({"Administrator"})
    public Response create(ModalityDTO modalityDTO) throws MyEntityExistsException, MyConstraintViolationException {
        return Response.status(Response.Status.CREATED).entity(new GenericEntity<ModalityDTO>(modalityBean.create(modalityDTO)){}).build();
    }

    @DELETE
    @Path("{id}")
    @RolesAllowed({"Administrator"})
    public Response delete(@PathParam("id") Integer id) throws MyEntityNotFoundException, MyReflectionException, MyEntityCannotBeDeletedException {
        modalityBean.delete(id);
        return Response.status(Response.Status.OK).build();
    }
}
