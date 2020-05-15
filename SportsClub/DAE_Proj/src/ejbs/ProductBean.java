package ejbs;

import dtos.ProductDTO;
import entities.Product;
import entities.Registration;
import exceptions.MyEntityCannotBeDeletedException;
import exceptions.MyEntityNotFoundException;
import exceptions.MyReflectionException;

import javax.annotation.PostConstruct;
import javax.ejb.EJB;
import javax.ejb.EJBException;
import javax.ejb.Stateless;
import java.util.Collections;
import java.util.List;

@Stateless(name = "ProductEJB")
public class ProductBean extends AbstractProductBean<Product, ProductDTO> {

    @EJB
    private RegistrationBean registrationBean;

    @Override
    @PostConstruct
    protected void init() {
        this.initialize(Product.class, ProductDTO.class);
    }

    public List<Registration> registrations(Integer id) {
        try {
            @SuppressWarnings("unchecked")
            List<Registration> registrations = (List<Registration>) em.createNamedQuery( "product.registrations").setParameter("id", id).getResultList();
            registrations.removeAll(Collections.singletonList(null));
            return registrations;
        } catch (Exception e) {
            throw new EJBException("ERROR_RETRIEVING_REGISTRATIONS", e);
        }
    }

    @Override
    public void delete(Object primaryKey) throws MyEntityNotFoundException, MyReflectionException, MyEntityCannotBeDeletedException {
        for (Registration r : registrations(Integer.valueOf(primaryKey.toString()))) registrationBean.delete(r.getId());
        super.delete(primaryKey);
    }
}
