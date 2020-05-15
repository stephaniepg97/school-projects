package entities;

import javax.persistence.*;
import javax.persistence.Entity;
import javax.validation.constraints.NotNull;
import java.util.*;
import java.util.stream.Collectors;

@Entity
@Table(name = "PURCHASES")
@NamedQueries({
        @NamedQuery(
                name = "purchase.all",
                query = "SELECT p FROM Purchase p"
        ),
        @NamedQuery(
                name = "purchase.payments",
                query = "SELECT p.payments FROM Purchase p where p.id=:id"
        )
})
public class Purchase implements entities.Entity {

    @Version
    private int version;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private Integer id;

    @NotNull
    @ManyToOne
    private Associate associate;

    @OneToOne
    private Receipt receipt;

    @NotNull
    @OneToMany(mappedBy = "purchase", cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    private Set<Payment> payments;

    @NotNull
    private Date date;

    @NotNull
    private double total;

    public Purchase() {
        this.payments = new LinkedHashSet<>();
    }

    public Purchase(Integer id, Associate associate, Receipt receipt, Date date, double total) {
        this.id = id;
        this.associate = associate;
        this.receipt = receipt;
        this.date = date;
        this.total = total;
        this.payments = new LinkedHashSet<>();
    }

    public Purchase(Integer id, Associate associate, Date date, double total) {
        this(id, associate, null, date, total);
    }

    public Purchase(Associate associate, Date date, double total) {
        this(null, associate, null, date, total);
    }

    public void add(Payment p) {
        this.payments.add(p);
    }

    public void remove(Payment p) { this.payments.remove(p); }

    public void removePayment(int id) {
        List<Payment> result = this.payments.stream().filter(e -> e.getId() == id).collect(Collectors.toList());
        if (result.size() == 1) remove(result.get(0));
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public void setPayments(Set<Payment> payments) {
        this.payments = payments;
    }

    public Date getDate() {
        return date;
    }

    public void setDate(Date date) {
        this.date = date;
    }

    public double getTotal() {
        return total;
    }

    public void setTotal(double total) {
        this.total = total;
    }

    public Set<Payment> getPayments() {
        return this.payments;
    }

    public void setPayments(Collection<Payment> payments) {
        this.payments = payments.stream().collect(Collectors.toSet());
    }

    public Associate getAssociate() {
        return associate;
    }

    public void setAssociate(Associate associate) {
        this.associate = associate;
    }

    public Receipt getReceipt() {
        return receipt;
    }

    public void setReceipt(Receipt receipt) {
        this.receipt = receipt;
    }

    public int getVersion() {
        return version;
    }

    public void setVersion(int version) {
        this.version = version;
    }
}
