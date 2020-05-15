package entities;

import javax.persistence.*;
import javax.persistence.Entity;
import javax.validation.constraints.NotNull;
import java.time.DayOfWeek;
import java.time.LocalTime;
import java.util.*;
import java.util.stream.Collectors;

@Entity
@Table(name="TIMETABLES")
@NamedQueries({
        @NamedQuery(
                name = "timetable.all",
                query = "SELECT t FROM Timetable t"
        )
})
public class Timetable implements entities.Entity {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private Integer id;

    @OneToMany(mappedBy = "timetable", fetch = FetchType.LAZY)
    private Set<SportClass> classes;

    @NotNull
    private DayOfWeek dayOfWeek;

    @NotNull
    private LocalTime duration;

    @NotNull
    private LocalTime init;

    @Version
    private int version;

    public Timetable() {
        this.classes = new LinkedHashSet<>();
    }

    public Timetable(DayOfWeek dayOfWeek, LocalTime duration, LocalTime init) {
        this(null, dayOfWeek, duration, init);
    }

    public Timetable(Integer id, DayOfWeek dayOfWeek, LocalTime duration, LocalTime init) {
        this.id = id;
        this.dayOfWeek = dayOfWeek;
        this.duration = duration;
        this.init = init;
        this.classes = new LinkedHashSet<>();
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public Integer getId() {
        return id;
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

    public DayOfWeek getDayOfWeek() {
        return dayOfWeek;
    }

    public void setDayOfWeek(DayOfWeek dayOfWeek) {
        this.dayOfWeek = dayOfWeek;
    }

    public LocalTime getDuration() {
        return duration;
    }

    public void setDuration(LocalTime duration) {
        this.duration = duration;
    }

    public LocalTime getInit() {
        return init;
    }

    public void setInit(LocalTime init) {
        this.init = init;
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

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Timetable timetable = (Timetable) o;
        return id == timetable.id;
    }

    @Override
    public int hashCode() {
        return Objects.hash(id);
    }
}
