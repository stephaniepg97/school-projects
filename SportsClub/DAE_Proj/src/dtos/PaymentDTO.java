package dtos;

import java.util.Objects;

public class PaymentDTO implements DTO {

    private Integer id;
    private PurchaseDTO purchase;
    private GenericProductDTO product;
    private long launchInMilliseconds;
    private int quantity;
    private double payed;

    private int state;

    public PaymentDTO() {
    }

    public PaymentDTO(Integer id, PurchaseDTO purchase, GenericProductDTO product, long launchInMilliseconds, int quantity, double payed, int state) {
        this.id = id;
        this.purchase = purchase;
        this.product = product;
        this.launchInMilliseconds = launchInMilliseconds;
        this.quantity = quantity;
        this.payed = payed;
        this.state = state;
    }

    public PaymentDTO(PurchaseDTO purchase, GenericProductDTO product, long launchInMilliseconds, int quantity, double payed, int state) {
        this(null, purchase, product, launchInMilliseconds, quantity, payed, state);
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public GenericProductDTO getProduct() {
        return product;
    }

    public void setProduct(GenericProductDTO product) {
        this.product = product;
    }

    public long getLaunchInMilliseconds() {
        return launchInMilliseconds;
    }

    public void setLaunchInMilliseconds(long launchInMilliseconds) {
        this.launchInMilliseconds = launchInMilliseconds;
    }

    public int getQuantity() {
        return quantity;
    }

    public void setQuantity(int quantity) {
        this.quantity = quantity;
    }

    public double getPayed() {
        return payed;
    }

    public void setPayed(double payed) {
        this.payed = payed;
    }

    public int getState() {
        return state;
    }

    public void setState(int state) {
        this.state = state;
    }

    public PurchaseDTO getPurchase() {
        return purchase;
    }

    public void setPurchase(PurchaseDTO purchase) {
        this.purchase = purchase;
    }

    @Override
    public void reset() {
        this.purchase.reset();
        this.launchInMilliseconds = -1;
        this.payed = -1;
        this.state = -1;
        this.product.reset();
        this.quantity = -1;
        this.id = null;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        PaymentDTO that = (PaymentDTO) o;
        return id == that.id;
    }

    @Override
    public int hashCode() {
        return Objects.hash(id);
    }
}
