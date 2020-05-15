package ejbs;

import dtos.ReceiptDTO;
import entities.Receipt;

import javax.annotation.PostConstruct;
import javax.ejb.EJB;
import javax.ejb.Stateless;

@Stateless(name = "ReceiptEJB")
public class ReceiptBean extends Bean<Receipt, ReceiptDTO> {

    @EJB
    private PurchaseBean purchaseBean;

    @Override
    public Object primaryKey(ReceiptDTO dto) {
        return dto.getId();
    }

    @Override
    protected ReceiptDTO map(Receipt entity, ReceiptDTO dto) {
        dto.setId(entity.getId());
        dto.setPurchase(purchaseBean.toDTO(entity.getPurchase()));
        return dto;
    }

    @Override
    protected Receipt map(ReceiptDTO dto, Receipt entity) {
        entity.setId(dto.getId());
        entity.setPurchase(purchaseBean.toEntity(dto.getPurchase()));
        return entity;
    }

    @Override
    @PostConstruct
    protected void init() {
        initialize(Receipt.class, ReceiptDTO.class);
    }
}
