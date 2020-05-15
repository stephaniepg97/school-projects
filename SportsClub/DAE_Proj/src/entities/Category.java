package entities;

import javax.persistence.*;
import javax.persistence.Entity;
import java.util.*;
import java.util.stream.Collectors;

@Entity
@Table(name="CATEGORIES")
@NamedQueries({
    @NamedQuery(
            name = "category.all",
            query = "SELECT c FROM Category c"
    ),
    @NamedQuery(
            name = "category.products",
            query = "SELECT c.products FROM Category c where c.name = :name"
    )
})
public class Category implements entities.Entity {

    @Version
    private int version;

    @Id
    private String name;

    @OneToMany(mappedBy = "category", fetch = FetchType.LAZY)
    private Set<GenericProduct> products;

    public Category() {
        this.products = new LinkedHashSet<>();
    }

    public Category(String name) {
        this.name = name;
        this.products = new LinkedHashSet<>();
    }

    public void add(GenericProduct p) {
        this.products.add(p);
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void remove(GenericProduct p) { this.products.remove(p); }

    public void remove(int productId) {
        List<GenericProduct> result = this.products.stream().filter(e -> e.getId() == productId).collect(Collectors.toList());
        if (result.size() == 1) remove(result.get(0));
    }

    public int getVersion() {
        return version;
    }

    public void setVersion(int version) {
        this.version = version;
    }

    public Set<GenericProduct> getProducts() {
        return products;
    }

    public void setProducts(Set<GenericProduct> products) {
        this.products = products;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Category category = (Category) o;
        return name.equals(category.name);
    }

    @Override
    public int hashCode() {
        return Objects.hash(name);
    }
}
