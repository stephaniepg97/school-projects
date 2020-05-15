package ejbs;

import dtos.PaymentDTO;
import entities.Graduation;
import entities.Payment;
import entities.Product;

import javax.annotation.PostConstruct;
import javax.ejb.EJB;
import javax.ejb.EJBException;
import javax.ejb.Stateless;
import java.util.Date;
import java.util.List;

@Stateless(name = "PaymentEJB")
public class PaymentBean extends Bean<Payment, PaymentDTO> {

    @EJB
    private GenericProductBean genericProductBean;

    @EJB
    private PurchaseBean purchaseBean;

    @Override
    public Object primaryKey(PaymentDTO dto) {
        return dto.getId();
    }

    @Override
    protected PaymentDTO map(Payment entity, PaymentDTO dto) {
        dto.setId(entity.getId());
        dto.setLaunchInMilliseconds( entity.getLaunch().getTime());
        dto.setPayed(entity.getPayed());
        dto.setProduct(genericProductBean.toDTO(entity.getProduct()));
        dto.setPurchase(purchaseBean.toDTO(entity.getPurchase()));
        dto.setQuantity(entity.getQuantity());
        dto.setState(entity.getState().ordinal());
        return dto;
    }

    @Override
    protected Payment map(PaymentDTO dto, Payment entity) {
        entity.setId(dto.getId());
        entity.setLaunch(new Date(dto.getLaunchInMilliseconds()));
        entity.setPayed(dto.getPayed());
        entity.setProduct(genericProductBean.toEntity(dto.getProduct()));
        entity.setPurchase(purchaseBean.toEntity(dto.getPurchase()));
        entity.setQuantity(dto.getQuantity());
        entity.setState(Payment.State.values()[dto.getState()]);
        return entity;
    }

    @Override
    @PostConstruct
    protected void init() {
        initialize(Payment.class, PaymentDTO.class);
    }

    public List<Product> products(Integer id) {
        try {
            @SuppressWarnings("unchecked")
            List<Product> graduations = (List<Product>) em.createNamedQuery( "payment.products").setParameter("id", id).getResultList();
            return graduations;
        } catch (Exception e) {
            throw new EJBException("ERROR_RETRIEVING_GRADUATIONS", e);
        }
    }
}
