package entities;

import javax.persistence.*;
import javax.validation.constraints.NotNull;
import javax.persistence.Entity;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

@Entity
@Table(name="SPORT_CLASSES")
@NamedQueries({
        @NamedQuery(
                name = "sportClass.all",
                query = "SELECT s FROM SportClass s"
        ),
        @NamedQuery(
                name = "sportClass.attendances",
                query = "SELECT s.attendances FROM SportClass s WHERE s.id=:id"
        )
})
public class SportClass extends Product {

    @NotNull
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Season season;

    @NotNull
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Rank rank;

    @NotNull
    @ManyToOne(cascade = CascadeType.ALL)
    private Timetable timetable;

    @NotNull
    @ManyToOne(cascade = CascadeType.PERSIST)
    private Coach coach;

    @NotNull
    @OneToMany(mappedBy = "sportClass", fetch = FetchType.LAZY)
    private Set<Attendance> attendances;

    public SportClass(Integer id, String name, double value, int stock, Integer originalId, Season season, Modality modality, Rank rank, Timetable timetable, Coach coach) {
        super(id, new Category("SportClass"), modality, name, value, stock, originalId);
        this.id = id;
        this.season = season;
        this.rank = rank;
        this.timetable = timetable;
        this.coach = coach;
        this.attendances = new LinkedHashSet<>();
    }

    public SportClass(String name, double value, int stock, Integer originalId, Season season, Modality modality, Rank rank, Timetable timetable, Coach coach) {
        this(null, name, value, stock, originalId, season, modality, rank, timetable, coach);
    }

    public SportClass(String name, double value, int stock, Season season, Modality modality, Rank rank, Timetable timetable, Coach coach) {
        this(null, name, value, stock, null, season, modality, rank, timetable, coach);
    }

    public SportClass() {
        this.attendances = new LinkedHashSet<>();
    }

    public void add(Attendance r) {
        this.attendances.add(r);
    }

    public void remove(Attendance r) {
        this.attendances.remove(r);
    }

    public void removeAttendance(int id) {
        List<Attendance> result = this.attendances.stream().filter(e -> e.getId() == id).collect(Collectors.toList());
        if (result.size() == 1) remove(result.get(0));
    }

    public Rank getRank() {
        return rank;
    }

    public void setRank(Rank rank) {
        this.rank = rank;
    }

    public Timetable getTimetable() {
        return timetable;
    }

    public void setTimetable(Timetable timetable) {
        this.timetable = timetable;
    }

    public Coach getCoach() {
        return coach;
    }

    public void setCoach(Coach coach) {
        this.coach = coach;
    }

    public Season getSeason() {
        return season;
    }

    public void setSeason(Season season) {
        this.season = season;
    }
}
