package entities;

import javax.persistence.*;
import javax.persistence.Entity;
import java.util.Collection;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

@Entity
@NamedQueries({
    @NamedQuery(
            name = "coach.all",
            query = "SELECT c FROM Coach c"
    ),
    @NamedQuery(
            name = "coach.classes",
            query = "SELECT c.classes FROM Coach c"
    )
})
public class Coach extends User {

    @OneToMany(mappedBy = "coach", fetch = FetchType.LAZY)
    private Set<SportClass> classes;

    public Coach() {
        this.classes = new LinkedHashSet<>();
    }

    public Coach(String username, String password, String name, String email) {
        super(username, password, name, email);
        this.classes = new LinkedHashSet<>();
    }

    public void add(SportClass c) {
        this.classes.add(c);
    }

    public void remove(SportClass c) {
        this.classes.remove(c);
    }

    public void remove(int classId) {
        List<SportClass> result = this.classes.stream().filter(e -> e.getId() == classId).collect(Collectors.toList());
        if (result.size() == 1) remove(result.get(0));
    }

    public Set<SportClass> getClasses() {
        return classes;
    }

    public void setClasses(Collection<SportClass> classes) {
        this.classes = classes.stream().collect(Collectors.toSet());
    }
}
