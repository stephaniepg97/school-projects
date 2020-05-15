package entities;

import javax.persistence.*;
import javax.persistence.Entity;

@Entity
@Table(name="GRADUATIONS")
@NamedQueries({
    @NamedQuery(
            name = "graduation.all",
            query = "SELECT g FROM Graduation g"
    )
})
public class Graduation extends Product {

    public Graduation() {
    }

    public Graduation(Integer id, Modality modality, String name, double value, int stock, Integer originalId) {
        super(id, new Category("Graduation"), modality, name, value, stock, originalId);
    }

    public Graduation(Modality modality, String name, double value, int stock, Integer originalId) {
        this(null, modality, name, value, stock, originalId);
    }
}


