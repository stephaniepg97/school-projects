package entities;

import javax.persistence.*;
import javax.persistence.Entity;

@Entity
@NamedQueries({
    @NamedQuery(
            name = "administrator.all",
            query = "SELECT treat (u as Administrator) FROM User u"
    )
})
public class Administrator extends User {

    public Administrator() {
    }

    public Administrator(String username, String password, String name, String email) {
        super(username, password, name, email);
    }
}
