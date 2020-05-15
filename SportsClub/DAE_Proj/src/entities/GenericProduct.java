package entities;

import javax.persistence.*;
import javax.persistence.Entity;
import javax.validation.constraints.NotNull;
import java.util.*;
import java.util.stream.Collectors;

@Entity
@Table(name="GENERIC_PRODUCTS")
@Inheritance(strategy = InheritanceType.TABLE_PER_CLASS)
@NamedQueries({
        @NamedQuery(
                name = "genericProduct.all",
                query = "SELECT p FROM GenericProduct p"
        ),
        @NamedQuery(
                name = "genericProduct.allOutCopies",
                query = "SELECT p FROM GenericProduct p WHERE p.originalId IS NULL"
        ),
        @NamedQuery(
                name = "genericProduct.payments",
                query = "SELECT p.payments FROM GenericProduct p WHERE p.id=:id"
        )
})
public class GenericProduct implements entities.Entity {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    protected Integer id;

    @NotNull
    @ManyToOne(cascade = CascadeType.PERSIST)
    protected Category category;

    @NotNull
    @OneToMany(mappedBy = "product", fetch = FetchType.LAZY)
    protected Set<Payment> payments;

    @NotNull
    protected String name;

    @NotNull
    protected double value;

    @NotNull
    protected int stock;

    protected Integer originalId;

    @Version
    private int version;

    public GenericProduct() {
        this.payments = new LinkedHashSet<>();
    }

    public GenericProduct(Integer id, Category category, String name, double value, int stock, Integer originalId) {
        this.id = id;
        this.category = category;
        this.name = name;
        this.stock = stock;
        this.value = value;
        this.originalId = originalId;
        this.payments = new LinkedHashSet<>();
    }

    public GenericProduct(Category category, String name, double value, int stock, Integer originalId) {
        this(null, category, name, value, stock, originalId);
    }

    public int getVersion() {
        return version;
    }

    public void setVersion(int version) {
        this.version = version;
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

    public Category getCategory() {
        return category;
    }

    public void setCategory(Category category) {
        this.category = category;
    }

    public String getName() {
        return name;
    }

    public void setName(String description) {
        this.name = description;
    }

    public double getValue() {
        return value;
    }

    public void setValue(double value) {
        this.value = value;
    }

    public int getStock() {
        return stock;
    }

    public void setStock(int stock) {
        this.stock = stock;
    }

    public Integer getOriginalId() {
        return originalId;
    }

    public void setOriginalId(Integer originalId) {
        this.originalId = originalId;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        GenericProduct product = (GenericProduct) o;
        return id == product.id;
    }

    @Override
    public int hashCode() {
        return Objects.hash(id);
    }
}
