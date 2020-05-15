package dtos;

import java.util.Collection;
import java.util.LinkedHashSet;
import java.util.Set;
import java.util.stream.Collectors;

public class AssociateDTO extends UserDTO {

    private Set<PurchaseDTO> purchases;


    public AssociateDTO() {
        this.purchases = new LinkedHashSet<>();
    }

    public AssociateDTO(String username, String password, String name, String email) {
        super(username, password, name, email);
        this.purchases = new LinkedHashSet<>();
    }

    @Override
    public void reset() {
        super.reset();
        this.purchases.clear();
    }

    public void add(PurchaseDTO p) {
        this.purchases.add(p);
    }

    public void remove(PurchaseDTO p) { this.purchases.remove(p); }

    public Set<PurchaseDTO> getPurchases() {
        return purchases;
    }

    public void setPurchases(Collection<PurchaseDTO> purchases) {
        this.purchases = purchases.stream().collect(Collectors.toSet());
    }
}
