package dtos;

import java.util.Collection;
import java.util.LinkedHashSet;
import java.util.Set;
import java.util.stream.Collectors;

public class RankDTO implements DTO {

    private String name;
    private Set<SportClassDTO> classes;

    public RankDTO() {
        this.classes = new LinkedHashSet<>();
    }

    public RankDTO(String name) {
        this.name = name;
        this.classes = new LinkedHashSet<>();
    }

    public void add(SportClassDTO c) {
        this.classes.add(c);
    }

    public void remove(SportClassDTO c) {
        this.classes.remove(c);
    }

    public Set<SportClassDTO> getClasses() {
        return classes;
    }

    public void setClasses(Collection<SportClassDTO> classes) {
        this.classes = classes.stream().collect(Collectors.toSet());
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    @Override
    public void reset() {
        this.name = null;
        this.classes.clear();
    }
}
