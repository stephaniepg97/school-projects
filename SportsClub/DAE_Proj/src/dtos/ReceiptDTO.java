package dtos;

import java.util.Objects;

public class ReceiptDTO implements DTO {

    private Integer id;
    private PurchaseDTO purchase;

    public ReceiptDTO(){
    }

    public ReceiptDTO(Integer id, PurchaseDTO purchase) {
        this.id = id;
        this.purchase = purchase;
    }

    public ReceiptDTO(PurchaseDTO purchase) {
        this(null, purchase);
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public PurchaseDTO getPurchase() {
        return purchase;
    }

    public void setPurchase(PurchaseDTO purchase) {
        this.purchase = purchase;
    }

    @Override
    public void reset() {
        this.id = null;
        this.purchase.reset();
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        ReceiptDTO that = (ReceiptDTO) o;
        return id == that.id;
    }

    @Override
    public int hashCode() {
        return Objects.hash(id);
    }
}
