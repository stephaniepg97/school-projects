package dtos;

import entities.Receipt;

import java.util.LinkedHashSet;
import java.util.Objects;
import java.util.Set;

public class PurchaseDTO implements DTO {

    private Integer id;
    private AssociateDTO associate;
    private ReceiptDTO receipt;
    private Set<PaymentDTO> payments;
    private long dateInMilliseconds;
    private double total;

    public PurchaseDTO() {
        this.payments = new LinkedHashSet<>();
    }

    public PurchaseDTO(Integer id, AssociateDTO associate, ReceiptDTO receipt, long dateInMilliseconds, double total) {
        this.id = id;
        this.associate = associate;
        this.receipt = receipt;
        this.dateInMilliseconds = dateInMilliseconds;
        this.total = total;
        this.payments = new LinkedHashSet<>();
    }

    public PurchaseDTO(Integer id, AssociateDTO associate, long dateInMilliseconds, double total) {
        this(id, associate, null, dateInMilliseconds, total);
    }

    public PurchaseDTO(AssociateDTO associate, long dateInMilliseconds, double total) {
        this(null, associate, dateInMilliseconds, total);
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public AssociateDTO getAssociate() {
        return associate;
    }

    public void setAssociate(AssociateDTO associate) {
        this.associate = associate;
    }

    public ReceiptDTO getReceipt() {
        return receipt;
    }

    public void setReceipt(ReceiptDTO receipt) {
        this.receipt = receipt;
    }

    public Set<PaymentDTO> getPayments() {
        return payments;
    }

    public void setPayments(Set<PaymentDTO> payments) {
        this.payments = payments;
    }

    public long getDateInMilliseconds() {
        return dateInMilliseconds;
    }

    public void setDateInMilliseconds(long dateInMilliseconds) {
        this.dateInMilliseconds = dateInMilliseconds;
    }

    public double getTotal() {
        return total;
    }

    public void setTotal(double total) {
        this.total = total;
    }

    @Override
    public void reset() {
        this.payments.clear();
        this.dateInMilliseconds = -1;
        this.id = null;
        this.receipt.reset();
        this.associate.reset();
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        PurchaseDTO that = (PurchaseDTO) o;
        return id == that.id;
    }

    @Override
    public int hashCode() {
        return Objects.hash(id);
    }
}
