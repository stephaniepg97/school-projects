package entities;

import javax.persistence.*;
import javax.persistence.Entity;
import javax.validation.constraints.NotNull;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;


@Entity
@Table(name="REGISTRATIONS")
@NamedQueries({
        @NamedQuery(
                name = "registration.all",
                query = "SELECT r FROM Registration r"
        )
})
public class Registration implements entities.Entity {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private Integer id;

    @NotNull
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Athlete athlete;

    @NotNull
    @ManyToMany(fetch = FetchType.LAZY)
    @JoinTable(name = "PRODUCTS_REGISTRATIONS", joinColumns = @JoinColumn(name = "PRODUCT_ID", referencedColumnName = "ID"), inverseJoinColumns = @JoinColumn(name = "REGISTRATIONS_ID", referencedColumnName = "ID"))
    private Set<Product> products;

    public Registration() {
        this.products = new LinkedHashSet<>();
    }

    public Registration(Integer id, Athlete athlete) {
        this.id = id;
        this.athlete = athlete;
        this.products = new LinkedHashSet<>();
    }

    public Registration(Athlete athlete) {
        this(null, athlete);
    }

    public void add(Product p) {
        this.products.add(p);
    }

    public void remove(Product p) { this.products.remove(p); }

    public void removeProduct(int id) {
        List<Product> result = this.products.stream().filter(e -> e.getId() == id).collect(Collectors.toList());
        if (result.size() == 1) remove(result.get(0));
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public Athlete getAthlete() {
        return athlete;
    }

    public void setAthlete(Athlete athlete) {
        this.athlete = athlete;
    }

    public Set<Product> getProducts() {
        return products;
    }

    public void setProducts(Set<Product> products) {
        this.products = products;
    }
}
