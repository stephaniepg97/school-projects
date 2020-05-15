package entities;

import javax.persistence.*;
import javax.persistence.Entity;
import javax.validation.constraints.NotNull;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

@Entity
@NamedQueries({
    @NamedQuery(
            name = "associate.all",
            query = "SELECT a FROM Associate a"
    )
})
public class Associate extends User {

    @NotNull
    @OneToMany(mappedBy = "associate", fetch = FetchType.LAZY)
    private Set<Purchase> purchases;

    public Associate() {
        this.purchases = new LinkedHashSet<>();
    }

    public Associate(String username, String password, String name, String email) {
        super(username, password, name, email);
        this.purchases = new LinkedHashSet<>();
    }

    public void add(Purchase p) {
        this.purchases.add(p);
    }

    public void remove(Purchase p) { this.purchases.remove(p); }

    public void removePurchase(int id) {
        List<Purchase> result = this.purchases.stream().filter(e -> e.getId() == id).collect(Collectors.toList());
        if (result.size() == 1) remove(result.get(0));
    }

    public Set<Purchase> getPurchases() {
        return purchases;
    }

    public void setPurchases(Set<Purchase> purchases) {
        this.purchases = purchases;
    }
}
