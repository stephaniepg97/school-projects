package ws;

import dtos.CategoryDTO;
import dtos.GenericProductDTO;
import ejbs.CategoryBean;
import ejbs.GenericProductBean;
import exceptions.MyReflectionException;

import javax.ejb.EJB;
import javax.ws.rs.Consumes;
import javax.ws.rs.GET;
import javax.ws.rs.Path;
import javax.ws.rs.Produces;
import javax.ws.rs.core.*;
import java.util.Set;
import java.util.stream.Collectors;

@Path("/categories")
@Produces({MediaType.APPLICATION_JSON})
@Consumes({MediaType.APPLICATION_JSON})
public class CategoryController {
    @EJB
    private CategoryBean categoryBean;

    @Context
    private SecurityContext securityContext;

    @GET
    @Path("/")
    public Response all() throws MyReflectionException {
        return Response.status(Response.Status.OK).entity(new GenericEntity<Set<CategoryDTO>>(categoryBean.toDTOs(categoryBean.all().stream().collect(Collectors.toSet()))) {}).build();
    }
}
