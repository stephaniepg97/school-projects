package entities;

import javax.persistence.*;
import javax.persistence.Entity;
import javax.validation.constraints.NotNull;
import java.util.*;
import java.util.stream.Collectors;

@Entity
@Table(name="RECEIPTS")
@NamedQueries({
    @NamedQuery(
            name = "receipt.all",
            query = "SELECT r FROM Receipt r"
    )
})
public class Receipt implements entities.Entity {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private Integer id;

    @NotNull
    @OneToOne(mappedBy = "receipt", cascade = CascadeType.PERSIST)
    private Purchase purchase;

    @Version
    private int version;

    public Receipt() {
    }

    public Receipt(Integer id, Purchase purchase) {
        this.id = id;
        this.purchase = purchase;
    }
    public Receipt(Purchase purchase) {
        this(null, purchase);
    }

    public Purchase getPurchase() {
        return purchase;
    }

    public void setPurchase(Purchase purchase) {
        this.purchase = purchase;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public int getVersion() {
        return version;
    }

    public void setVersion(int version) {
        this.version = version;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Receipt receipt = (Receipt) o;
        return id == receipt.id;
    }

    @Override
    public int hashCode() {
        return Objects.hash(id);
    }
}
