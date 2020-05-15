package entities;

import javax.persistence.*;
import javax.persistence.Entity;
import java.util.Date;

@Entity
@Table(name = "ATTENDANCES")
@NamedQueries({
        @NamedQuery(
                name = "attendance.all",
                query = "SELECT a FROM Attendance a"
        )
})
public class Attendance implements entities.Entity {

    @Version
    private int version;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private Integer id;

    @ManyToOne(cascade = CascadeType.PERSIST)
    private Athlete athlete;

    @ManyToOne(cascade = CascadeType.PERSIST)
    private SportClass sportClass;

    private Date date;

    public Attendance() {
    }

    public Attendance(Integer id, Athlete athlete, SportClass sportClass, Date date) {
        this.id = id;
        this.athlete = athlete;
        this.sportClass = sportClass;
        this.date = date;
    }

    public Attendance(Athlete athlete, SportClass sportClass, Date date) {
        this(null, athlete, sportClass, date);
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public Date getDate() {
        return date;
    }

    public void setDate(Date date) {
        this.date = date;
    }

    public Athlete getAthlete() {
        return athlete;
    }

    public void setAthlete(Athlete athlete) {
        this.athlete = athlete;
    }

    public SportClass getSportClass() {
        return sportClass;
    }

    public void setSportClass(SportClass sportClass) {
        this.sportClass = sportClass;
    }

    public int getVersion() {
        return version;
    }

    public void setVersion(int version) {
        this.version = version;
    }
}
