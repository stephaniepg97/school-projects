package dtos;

import java.util.Collection;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

public class CoachDTO extends UserDTO {

    private Set<SportClassDTO> classes;

    public CoachDTO() {
        this.classes = new LinkedHashSet<>();
    }

    public CoachDTO(String username, String password, String name, String email) {
        super(username, password, name, email);
        this.classes = new LinkedHashSet<>();
    }

    @Override
    public void reset() {
        super.reset();
        this.classes.clear();
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
}
