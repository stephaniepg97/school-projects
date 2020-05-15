package dtos;

import java.util.*;
import java.util.stream.Collectors;

public class TimetableDTO implements DTO {

    private Integer id;
    private int dayOfWeek;
    private long secondOfDayDuration;
    private long secondOfDayInit;

    private Set<SportClassDTO> classes;

    public TimetableDTO() {
        this.classes = new LinkedHashSet<>();
    }

    public TimetableDTO(Integer id, int dayOfWeek, long secondOfDayDuration, long secondOfDayInit) {
        this.id = id;
        this.dayOfWeek = dayOfWeek;
        this.secondOfDayDuration = secondOfDayDuration;
        this.secondOfDayInit = secondOfDayInit;
        this.classes = new LinkedHashSet<>();
    }

    public TimetableDTO(int dayOfWeek, long secondOfDayDuration, long secondOfDayInit) {
        this(null, dayOfWeek, secondOfDayDuration, secondOfDayInit);
    }

    public void add(SportClassDTO s) {
        this.classes.add(s);
    }

    public void remove(SportClassDTO s) {
        this.classes.remove(s);
    }

    public void remove(int classId) {
        List<SportClassDTO> result = this.classes.stream().filter(e -> e.getId() == classId).collect(Collectors.toList());
        if (result.size() == 1) remove(result.get(0));
    }

    public int getDayOfWeek() {
        return dayOfWeek;
    }

    public void setDayOfWeek(int dayOfWeek) {
        this.dayOfWeek = dayOfWeek;
    }

    public long getSecondOfDayDuration() {
        return secondOfDayDuration;
    }

    public void setSecondOfDayDuration(long secondOfDayDuration) {
        this.secondOfDayDuration = secondOfDayDuration;
    }

    public long getSecondOfDayInit() {
        return secondOfDayInit;
    }

    public void setSecondOfDayInit(long secondOfDayInit) {
        this.secondOfDayInit = secondOfDayInit;
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public Set<SportClassDTO> getClasses() {
        return classes;
    }

    public void setClasses(Collection<SportClassDTO> sportClasses) {
        this.classes.addAll(sportClasses);
    }

    @Override
    public void reset() {
        this.classes.clear();
        this.id = null;
        this.dayOfWeek = -1;
        this.secondOfDayInit = this.secondOfDayDuration = -1;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        TimetableDTO that = (TimetableDTO) o;
        return id == that.id;
    }

    @Override
    public int hashCode() {
        return Objects.hash(id);
    }
}
