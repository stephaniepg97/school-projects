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
            name = "athlete.all",
            query = "SELECT a FROM Athlete a"
    )
})
public class Athlete extends Associate {

    @OneToMany(mappedBy = "athlete", fetch = FetchType.LAZY)
    private Set<Registration> registrations;

    @OneToMany(mappedBy = "athlete", fetch = FetchType.LAZY)
    private Set<Attendance> attendances;

    public Athlete() {
        this.registrations = new LinkedHashSet<>();
        this.attendances = new LinkedHashSet<>();
    }

    public Athlete(String username, String password, String name, String email) {
        super(username, password, name, email);
        this.registrations = new LinkedHashSet<>();
        this.attendances = new LinkedHashSet<>();
    }

    public void add(Registration r) {
        this.registrations.add(r);
    }

    public void remove(Registration r) {
        this.registrations.remove(r);
    }

    public void removeRegistration(int id) {
        List<Registration> result = this.registrations.stream().filter(e -> e.getId() == id).collect(Collectors.toList());
        if (result.size() == 1) remove(result.get(0));
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

    public Set<Registration> getRegistrations() {
        return registrations;
    }

    public void setRegistrations(Collection<Registration> registrations) {
        this.registrations = registrations.stream().collect(Collectors.toSet());
    }

    public Set<Attendance> getAttendances() {
        return attendances;
    }

    public void setAttendances(Collection<Attendance> attendances) {
        this.attendances = attendances.stream().collect(Collectors.toSet());
    }
}
