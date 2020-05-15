package ejbs;

import dtos.PurchaseDTO;
import entities.Purchase;

import javax.annotation.PostConstruct;
import javax.ejb.EJB;
import javax.ejb.Stateless;
import java.util.Date;

@Stateless(name = "PurchaseEJB")
public class PurchaseBean extends Bean<Purchase, PurchaseDTO> {

    @EJB
    private AssociateBean associateBean;

    @EJB
    private ReceiptBean receiptBean;

    @Override
    public Object primaryKey(PurchaseDTO dto) {
        return dto.getId();
    }

    @Override
    protected PurchaseDTO map(Purchase entity, PurchaseDTO dto) {
        dto.setId(entity.getId());
        dto.setAssociate(associateBean.toDTO(entity.getAssociate()));
        dto.setDateInMilliseconds(entity.getDate().getTime());
        dto.setReceipt(receiptBean.toDTO(entity.getReceipt()));
        dto.setTotal(entity.getTotal());
        return dto;
    }

    @Override
    protected Purchase map(PurchaseDTO dto, Purchase entity) {
        entity.setId(dto.getId());
        entity.setAssociate(associateBean.toEntity(dto.getAssociate()));
        entity.setDate(new Date(dto.getDateInMilliseconds()));
        entity.setReceipt(receiptBean.toEntity(dto.getReceipt()));
        entity.setTotal(dto.getTotal());
        return entity;
    }

    @Override
    @PostConstruct
    protected void init() {
        initialize(Purchase.class, PurchaseDTO.class);
    }
}
