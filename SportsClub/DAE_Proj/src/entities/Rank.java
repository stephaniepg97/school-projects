package entities;

import javax.persistence.*;
import javax.persistence.Entity;
import java.util.Collection;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

@Entity
@Table(name = "RANKS")
@NamedQueries({
    @NamedQuery(
            name = "rank.all",
            query = "SELECT r FROM Rank r"
    )
})
public class Rank implements entities.Entity {

    @Version
    private int version;

    @Id
    private String name;

    @OneToMany(mappedBy = "rank", fetch = FetchType.LAZY)
    private Set<SportClass> classes;

    public Rank() {
        this.classes = new LinkedHashSet<>();
    }

    public Rank(String name) {
        this.name = name;
        this.classes = new LinkedHashSet<>();
    }

    public void add(SportClass s) {
        this.classes.add(s);
    }

    public void remove(SportClass s) {
        this.classes.remove(s);
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

    public int getVersion() {
        return version;
    }

    public void setVersion(int version) {
        this.version = version;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }
}
