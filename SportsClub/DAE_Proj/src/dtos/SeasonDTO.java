package dtos;

import java.util.LinkedHashSet;
import java.util.List;
import java.util.Objects;
import java.util.Set;
import java.util.stream.Collectors;

public class SeasonDTO implements DTO {

    private Integer id;
    private Set<SportClassDTO> classes;
    private long startInMilliseconds;
    private long endInMilliseconds;

    public SeasonDTO() {
        this.classes = new LinkedHashSet<>();
    }

    public SeasonDTO(Integer id, long startInMilliseconds, long endInMilliseconds) {
        this.id = id;
        this.startInMilliseconds = startInMilliseconds;
        this.endInMilliseconds = endInMilliseconds;
        this.classes = new LinkedHashSet<>();
    }

    public SeasonDTO(long startInMilliseconds, long endInMilliseconds) {
        this(null, startInMilliseconds, endInMilliseconds);
    }

    public void add(SportClassDTO c) {
        this.classes.add(c);
    }

    public void remove(SportClassDTO c) {
        this.classes.remove(c);
    }

    public void remove(int classId) {
        List<SportClassDTO> result = this.classes.stream().filter(e -> e.getId() == classId).collect(Collectors.toList());
        if (result.size() == 1) remove(result.get(0));
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

    public void setClasses(Set<SportClassDTO> classes) {
        this.classes = classes;
    }

    public long getStartInMilliseconds() {
        return startInMilliseconds;
    }

    public void setStartInMilliseconds(long startInMilliseconds) {
        this.startInMilliseconds = startInMilliseconds;
    }

    public long getEndInMilliseconds() {
        return endInMilliseconds;
    }

    public void setEndInMilliseconds(long endInMilliseconds) {
        this.endInMilliseconds = endInMilliseconds;
    }

    @Override
    public void reset() {
        this.classes.clear();
        this.startInMilliseconds = this.endInMilliseconds = -1;
        this.id = null;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        SeasonDTO that = (SeasonDTO) o;
        return id == that.id;
    }

    @Override
    public int hashCode() {
        return Objects.hash(id);
    }
}
