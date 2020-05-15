package entities;

import javax.persistence.*;
import javax.persistence.Entity;
import javax.validation.constraints.NotNull;
import java.util.Collection;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

@Entity
@Table(name="PRODUCTS")
@NamedQueries({
        @NamedQuery(
                name = "product.all",
                query = "SELECT p FROM Product p"
        ),
        @NamedQuery(
                name = "product.registrations",
                query = "SELECT p.registrations FROM Product p WHERE p.id=:id"
        )
})
public class Product extends GenericProduct {

    @NotNull
    @ManyToMany(mappedBy = "products", fetch = FetchType.LAZY)
    private Set<Registration> registrations;

    @NotNull
    @ManyToOne
    private Modality modality;

    public Product() {
        this.registrations = new LinkedHashSet<>();
    }

    public Product(Integer id, Category category, Modality modality, String name, double value, int stock, Integer originalId) {
        super(id, category, name, value, stock, originalId);
        this.modality = modality;
        this.registrations = new LinkedHashSet<>();
    }

    public Product(Category category, Modality modality, String name, double value, int stock, Integer originalId) {
        this(null, category, modality, name, value, stock, originalId);
    }

    public void add(Registration r) {
        this.registrations.add(r);
    }

    public void remove(Registration r) {
        this.registrations.remove(r);
    }

    public void removeRegistration(int id) {
        List<Registration> result = this.registrations.stream().filter(e -> e.getId() == id).collect(Collectors.toList());
        if (result.size() == 1) remove(result.get(0));
    }

    public Set<Registration> getRegistrations() {
        return registrations;
    }

    public void setRegistrations(Collection<Registration> registrations) {
        this.registrations = registrations.stream().collect(Collectors.toSet());
    }

    public Modality getModality() {
        return modality;
    }

    public void setModality(Modality modality) {
        this.modality = modality;
    }
}
