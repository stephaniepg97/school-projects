package dtos;

public class AttendanceDTO implements DTO {

    private Integer id;
    private AthleteDTO athlete;
    private SportClassDTO sportClass;
    private long dateInMilliseconds;

    public AttendanceDTO() {
    }

    public AttendanceDTO(Integer id, AthleteDTO athlete, SportClassDTO sportClass, long dateInMilliseconds) {
        this.id = id;
        this.athlete = athlete;
        this.sportClass = sportClass;
        this.dateInMilliseconds = dateInMilliseconds;
    }

    public AttendanceDTO(AthleteDTO athlete, SportClassDTO sportClass, long dateInMilliseconds) {
        this(null, athlete, sportClass, dateInMilliseconds);
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public long getDateInMilliseconds() {
        return dateInMilliseconds;
    }

    public void setDateInMilliseconds(long dateInMilliseconds) {
        this.dateInMilliseconds = dateInMilliseconds;
    }

    public AthleteDTO getAthlete() {
        return athlete;
    }

    public void setAthlete(AthleteDTO athlete) {
        this.athlete = athlete;
    }

    public SportClassDTO getSportClass() {
        return sportClass;
    }

    public void setSportClass(SportClassDTO sportClass) {
        this.sportClass = sportClass;
    }

    @Override
    public void reset() {
        this.id = null;
        this.dateInMilliseconds = -1;
        this.athlete = null;
        this.sportClass = null;
    }
}
