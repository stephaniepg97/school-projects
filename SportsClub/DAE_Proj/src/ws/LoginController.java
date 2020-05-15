package ws;

import com.nimbusds.jwt.JWT;
import com.nimbusds.jwt.JWTParser;

import dtos.AuthDTO;
import ejbs.JwtBean;

import ejbs.UserBean;
import entities.User;

import jwt.Jwt;

import javax.ejb.EJB;

import javax.ws.rs.*;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;

import java.text.ParseException;

import java.util.logging.Logger;

@Path("/login")
public class LoginController {

    private static final Logger log = Logger.getLogger(LoginController.class.getName());

    @EJB
    JwtBean jwtBean;

    @EJB
    UserBean userBean;

    @POST
    @Path("/token")
    @Produces(MediaType.APPLICATION_JSON)
    @Consumes({MediaType.APPLICATION_JSON})
    public Response authenticateUser(AuthDTO authDTO) throws Exception {
        User user = userBean.authenticate(authDTO);
        log.info("Generating JWT for user " + user.getUsername());
        String token  = jwtBean.createJwt(user.getUsername(), new String[]{user.getClass().getSimpleName()});
        return Response.ok(new Jwt(token)).build();
    }

    @GET
    @Path("/claims")
    public Response demonstrateClaims(@HeaderParam("Authorization") String auth) {
        if (auth != null && auth.startsWith("Bearer ")) {
            try {
                JWT j = JWTParser.parse(auth.substring(7));
                return Response.ok(j.getJWTClaimsSet().getClaims()).build();
            } catch (ParseException e) {
                log.warning(e.toString());
                return Response.status(Response.Status.BAD_REQUEST).build();
            }
        }
        return Response.status(Response.Status.NO_CONTENT).build();
    }
}
