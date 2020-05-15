package entities;

import javax.persistence.*;
import javax.persistence.Entity;
import javax.validation.constraints.NotNull;
import java.util.*;
import java.util.stream.Collectors;

@Entity
@Table(name = "MODALITIES")
@NamedQueries({
        @NamedQuery(
                name = "modality.all",
                query = "SELECT m FROM Modality m"
        ),
        @NamedQuery(
                name = "modality.graduations",
                query = "SELECT g FROM Graduation g WHERE g.modality.id = :id"
        ),
        @NamedQuery(
                name = "modality.classes",
                query = "SELECT s FROM SportClass s WHERE s.modality.id = :id"
        )
})
public class Modality implements entities.Entity {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private Integer id;

    @NotNull
    private String name;

    @OneToMany(mappedBy = "modality", cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    private Set<Product> products;

    @Version
    private int version;

    public Modality() {
        this.products = new LinkedHashSet<>();
    }

    public Modality(Integer id, String name) {
        this.id = id;
        this.name = name;
        this.products = new LinkedHashSet<>();
    }

    public Modality(String name) {
        this(null, name);
    }

    public int getVersion() {
        return version;
    }

    public void setVersion(int version) {
        this.version = version;
    }

    public void add(Product p) {
        this.products.add(p);
    }

    public void remove(Product p) {
        this.products.remove(p);
    }

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

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setProducts(Collection<Product> products) {
        this.products = products.stream().collect(Collectors.toSet());
    }

    public Set<Product> getProducts() {
        return products;
    }

    public void setProducts(Set<Product> products) {
        this.products = products;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Modality modality = (Modality) o;
        return id == modality.id;
    }

    @Override
    public int hashCode() {
        return Objects.hash(id);
    }
}
