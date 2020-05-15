package ws;

import dtos.*;
import ejbs.*;
import exceptions.MyConstraintViolationException;
import exceptions.MyEntityExistsException;
import exceptions.MyEntityNotFoundException;
import exceptions.MyReflectionException;

import javax.annotation.security.RolesAllowed;
import javax.ejb.EJB;
import javax.ws.rs.*;
import javax.ws.rs.core.GenericEntity;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import java.util.HashSet;
import java.util.Set;

@Path("/payments")
@Produces({MediaType.APPLICATION_JSON})
@Consumes({MediaType.APPLICATION_JSON})
public class PaymentController {

    @EJB
    private PaymentBean paymentBean;

    @EJB
    private ProductBean productBean;

    @EJB
    private AssociateBean associateBean;

    @GET
    @Path("/")
    public Response all() throws MyReflectionException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<Set<PaymentDTO>>(paymentBean.toDTOs(new HashSet<>(paymentBean.all()))) {}).build();
    }

    @GET
    @Path("/associates")
    public Response allAssociate() throws MyReflectionException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<Set<AssociateDTO>>(associateBean.toDTOs(new HashSet<>(associateBean.all()))) {}).build();
    }

    @GET
    @Path("/{id}")
    public Response get(@PathParam("id") Integer id) throws MyReflectionException, MyEntityNotFoundException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<PaymentDTO>(paymentBean.toDTO(paymentBean.find(id))){}).build();
    }

    @GET
    @Path("{id}/products")
    public Response products(@PathParam("id") Integer id) throws MyEntityNotFoundException, MyReflectionException {
        paymentBean.find(id);
        return Response.status(Response.Status.OK)
                .entity(new GenericEntity<Set<ProductDTO>>(productBean.toDTOs(new HashSet<>(paymentBean.products(id)))) {})
                .build();
    }

    @POST
    @Path("/")
    @RolesAllowed({"Administrator"})
    public Response create(PaymentDTO paymentDTO) throws MyEntityExistsException, MyConstraintViolationException {
        paymentBean.create(paymentDTO);
        return Response.status(Response.Status.CREATED).build();
    }

    @DELETE
    @Path("{id}")
    @RolesAllowed({"Administrator"})
    public Response delete(@PathParam("id") Integer id) {
        try {
            paymentBean.delete(id);
            return Response.status(Response.Status.OK).build();
        } catch (Exception e) {
            e.getMessage();
            return null;
        }
    }
}
