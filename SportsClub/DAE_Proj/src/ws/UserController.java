package ws;

import dtos.*;
import ejbs.*;
import entities.Payment;
import entities.User;
import exceptions.*;

import javax.annotation.security.RolesAllowed;
import javax.ejb.EJB;
import javax.mail.MessagingException;
import javax.ws.rs.*;
import javax.ws.rs.core.*;
import java.util.*;
import java.util.stream.Collectors;

import static entities.User.hashPassword;

@Path("/users")
@Produces({MediaType.APPLICATION_JSON})
@Consumes({MediaType.APPLICATION_JSON})
public class UserController {

    @EJB
    private UserBean userBean;

    @EJB
    private AdministratorBean administratorBean;

    @EJB
    private CoachBean coachBean;

    @EJB
    private AssociateBean associateBean;

    @EJB
    private PurchaseBean purchaseBean;

    @EJB
    private GenericProductBean genericProductBean;

    @EJB
    private PaymentBean paymentBean;

    @EJB
    private ReceiptBean receiptBean;

    @EJB
    private EmailBean emailBean;

    @Context
    private SecurityContext securityContext;

    @GET
    @RolesAllowed({"Administrator"})
    public Response all() throws MyReflectionException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<Set<UserDTO>>(userBean.toDTOs(new HashSet<>(userBean.all()))) {}).build();
    }

    @GET
    @RolesAllowed({"Administrator"})
    @Path("filter/{type}")
    public Response all(@PathParam("type") String type) {
        return Response.status(Response.Status.OK).entity(new GenericEntity<Set<UserDTO>>(userBean.toDTOs(new HashSet<>(userBean.filter(type)))) {}).build();
    }

    @GET
    @Path("{username}")
    public Response get(@PathParam("username") String username) throws MyEntityNotFoundException, MyAuthorizationException, MyReflectionException {
        User user = userBean.find(username);
        /*if(securityContext.isUserInRole("Administrator")){
            return Response.status(Response.Status.OK).entity(userBean.toDTO(user)).build();
        }
        if(!securityContext.getUserPrincipal().getName().equals(username)){
            throw new MyAuthorizationException(null, username, "get details", user.getClass(), user.getUsername());
        }*/
        return Response.status(Response.Status.OK).entity(userBean.toDTO(user)).build();
    }

    @GET
    @Path("{username}/emails")
    public Response emails(@PathParam("username") String username) {
        return Response.status(Response.Status.OK).entity(new GenericEntity<Set<EmailDTO>>(emailBean.toDTOs(new HashSet<>(emailBean.emails(username)))) {}).build();
    }

    @POST
    @Path("/")
    @RolesAllowed({"Administrator"})
    public Response create(UserDTO dto) throws MyEntityExistsException, MyConstraintViolationException {
        dto.setPassword(hashPassword(dto.getPassword()));
        return Response.status(Response.Status.CREATED).entity(userBean.create(dto)).build();
    }

    @POST
    @Path("/create/Administrator")
    @RolesAllowed({"Administrator"})
    public Response create(AdministratorDTO dto) throws MyEntityExistsException, MyConstraintViolationException {
        dto.setPassword(hashPassword(dto.getPassword()));
        return Response.status(Response.Status.CREATED).entity(administratorBean.create(dto)).build();
    }

    @POST
    @Path("/create/Coach")
    @RolesAllowed({"Administrator"})
    public Response create(CoachDTO dto) throws MyEntityExistsException, MyConstraintViolationException {
        dto.setPassword(hashPassword(dto.getPassword()));
        return Response.status(Response.Status.CREATED).entity(coachBean.create(dto)).build();
    }

    @POST
    @Path("/create/Associate")
    public Response create(AssociateDTO dto) throws MyEntityExistsException, MyConstraintViolationException {
        dto.setPassword(hashPassword(dto.getPassword()));
        return Response.status(Response.Status.CREATED).entity(associateBean.create(dto)).build();
    }

    @POST
    @Path("{username}/email/send")
    public Response sendEmail(@PathParam("username") String username, EmailDTO emailDTO) throws MessagingException {
        emailBean.send(emailDTO);
        return Response.status(Response.Status.OK).build();
    }

    @DELETE
    @Path("{username}")
    @RolesAllowed({"Administrator"})
    public Response delete(@PathParam("username") String username) throws MyReflectionException, MyEntityNotFoundException, MyEntityCannotBeDeletedException {
        userBean.delete(username);
        return Response.status(Response.Status.OK).build();
    }

    @POST
    @Path("{username}/purchase")
    public Response purchase(@PathParam("username") String username, PurchaseDTO purchaseDTO) throws MyEntityExistsException, MyEntityNotFoundException, MyConstraintViolationException, MyReflectionException {
        Set<PaymentDTO> payments = purchaseDTO.getPayments();
        purchaseDTO = purchaseBean.create(purchaseDTO);
        boolean paid = true;
        for (PaymentDTO p: payments) {
            p.setPurchase(purchaseDTO);
            if (Payment.State.values()[paymentBean.create(p).getState()] == Payment.State.NOT_PAYED) paid = false;
        }
        if (paid) receiptBean.create(new ReceiptDTO(purchaseDTO));
        /*for (GenericProductDTO productDTO: productDTOS) {
            productDTO.setOriginalId(productDTO.getId());
            productDTO.setId(null);
            genericProductBean.create(productDTO);
            total = total + productDTO.getValue();
        }

        AssociateDTO associateDTO = associateBean.toDTO(associateBean.find(username));
        Date date = new Date();
        long timeMilli = date.getTime();
        PurchaseDTO purchaseDTO = purchaseBean.create(new PurchaseDTO(associateDTO, timeMilli, total));

        for (GenericProductDTO productDTO: productDTOS) {
            int count = 0;
            for (GenericProductDTO otherProductDTO: productDTOS) {
                if(productDTO.equals(otherProductDTO)){
                    count++;
                    if(count>1){
                        productDTOS.remove(otherProductDTO);
                    }
                }
            }
            paymentBean.create(new PaymentDTO(purchaseDTO, productDTO, timeMilli, count, 0, 0));
        }*/

        return Response.status(Response.Status.CREATED).entity(purchaseDTO).build();
    }

    @PUT
    @Path("{username}")
    @RolesAllowed({"Administrator"})
    public Response update(@PathParam("username") String username, UserDTO userDTO) throws MyConstraintViolationException, MyEntityNotFoundException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<UserDTO>(userBean.update(userDTO)){}).build();
    }

    private String role(String username) throws MyRoleNotFoundException {
        List<String> result = Arrays.stream((new String[]{"Administrator", "Coach", "Associate", "Athlete"})).filter(role -> securityContext.isUserInRole(role)).collect(Collectors.toList());
        if (result.size() != 1) throw new MyRoleNotFoundException(username);
        return result.get(0);
    }
}
