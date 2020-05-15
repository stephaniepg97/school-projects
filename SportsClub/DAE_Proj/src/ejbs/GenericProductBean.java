package ejbs;

import dtos.GenericProductDTO;
import entities.GenericProduct;
import entities.Payment;
import exceptions.MyEntityNotFoundException;
import exceptions.*;

import javax.annotation.PostConstruct;
import javax.ejb.EJB;
import javax.ejb.EJBException;
import javax.ejb.Stateless;
import java.util.Collections;
import java.util.List;

@Stateless(name = "GenericProductEJB")
public class GenericProductBean extends AbstractGenericProductBean<GenericProduct, GenericProductDTO> {

    @EJB
    private PaymentBean paymentBean;

    @Override
    @PostConstruct
    protected void init() {
        initialize(GenericProduct.class, GenericProductDTO.class);
    }

    public List<Payment> payments(Integer id) {
        try {
            @SuppressWarnings("unchecked")
            List<Payment> payments = (List<Payment>) em.createNamedQuery( "genericProduct.payments").setParameter("id", id).getResultList();
            payments.removeAll(Collections.singletonList(null));
            return payments;
        } catch (Exception e) {
            throw new EJBException("ERROR_RETRIEVING_PAYMENTS", e);
        }
    }

    @Override
    public void delete(Object primaryKey) throws MyEntityNotFoundException, MyEntityCannotBeDeletedException, MyReflectionException {
        GenericProduct product = find(primaryKey);
        if (product.getOriginalId() != null) throw new MyEntityCannotBeDeletedException(GenericProduct.class);
        for (Payment p : payments(Integer.valueOf(primaryKey.toString()))) paymentBean.delete(p.getId());
        super.delete(primaryKey);
    }

    public List<GenericProduct> allOutCopies() {
        try {
            @SuppressWarnings("unchecked")
            List<GenericProduct> products = (List<GenericProduct>) em.createNamedQuery("genericProduct.allOutCopies").getResultList();
            return products;
        } catch (Exception e) {
            throw new EJBException("ERROR_RETRIEVING_PRODUCTS ----> ", e);
        }
    }
}
