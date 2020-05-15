package entities;

import javax.persistence.*;
import javax.validation.constraints.NotNull;
import javax.persistence.Entity;
import java.util.Date;
import java.util.Objects;

@Entity
@Table(name="PAYMENTS")
@NamedQueries({
    @NamedQuery(
            name = "payment.all",
            query = "SELECT p FROM Payment p"
    ),
    @NamedQuery(
            name = "payment.state",
            query = "SELECT p FROM Payment p WHERE p.state = :state"
    )
})
public class Payment implements entities.Entity {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private Integer id;

    @NotNull
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Purchase purchase;

    @NotNull
    @ManyToOne(cascade = CascadeType.PERSIST)
    private GenericProduct product;

    @NotNull
    private Date launch;

    @NotNull
    private int quantity;

    @NotNull
    private double payed;

    @Enumerated
    private State state;

    @Version
    private int version;

    public Payment() {
    }

    public Payment(Purchase purchase, GenericProduct product, Date launch, int quantity, double payed, State state) {
        this(null, purchase, product, launch, quantity, payed, state);
    }

    public Payment(Integer id, Purchase purchase, GenericProduct product, Date launch, int quantity, double payed, State state) {
        this.id = id;
        this.purchase = purchase;
        this.product = product;
        this.launch = launch;
        this.quantity = quantity;
        this.payed = payed;
        this.state = state;
    }

    public enum State { PAYED, NOT_PAYED, PARTIAL }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public GenericProduct getProduct() {
        return product;
    }

    public void setProduct(GenericProduct product) {
        this.product = product;
    }

    public int getVersion() {
        return version;
    }

    public void setVersion(int version) {
        this.version = version;
    }

    public Purchase getPurchase() {
        return purchase;
    }

    public void setPurchase(Purchase purchase) {
        this.purchase = purchase;
    }

    public Date getLaunch() {
        return launch;
    }

    public void setLaunch(Date launch) {
        this.launch = launch;
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

    public State getState() { return state; }

    public void setState(State state) {
        this.state = state;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Payment payment = (Payment) o;
        return id == payment.id;
    }

    @Override
    public int hashCode() {
        return Objects.hash(id);
    }
}
