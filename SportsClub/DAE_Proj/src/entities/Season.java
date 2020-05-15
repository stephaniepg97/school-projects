package entities;

import javax.persistence.*;
import javax.persistence.Entity;
import javax.validation.constraints.NotNull;
import java.util.*;
import java.util.stream.Collectors;

@Entity
@Table(name = "SEASONS")
@NamedQueries({
        @NamedQuery(
                name = "season.all",
                query = "SELECT s FROM Season s"
        )
})
public class Season implements entities.Entity {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private Integer id;

    @NotNull
    private Date start;

    @NotNull
    private Date end;

    @OneToMany(mappedBy = "season", fetch = FetchType.LAZY)
    private Set<SportClass> classes;

    @Version
    private int version;

    public Season() {
        this.classes = new LinkedHashSet<>();
    }

    public Season(Integer id, Date start, Date end) {
        this.id = id;
        this.start = start;
        this.end = end;
        this.classes = new LinkedHashSet<>();
    }

    public Season(Date start, Date end) {
        this(null, start, end);
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
    public void setId(Integer id) {
        this.id = id;
    }

    public Integer getId() {
        return id;
    }

    public Date getStart() {
        return start;
    }

    public void setStart(Date start) {
        this.start = start;
    }

    public Date getEnd() {
        return end;
    }

    public void setEnd(Date end) {
        this.end = end;
    }

    public int getVersion() {
        return version;
    }

    public void setVersion(int version) {
        this.version = version;
    }
}
