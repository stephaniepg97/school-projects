package dtos;

import java.util.Collection;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

public class AthleteDTO extends AssociateDTO {

    private Set<RegistrationDTO> registrations;
    private Set<AttendanceDTO> attendances;

    public AthleteDTO(){
        this.registrations = new LinkedHashSet<>();
    }

    public AthleteDTO(String username, String password, String name, String email){
        super(username, password, name, email);
        this.registrations = new LinkedHashSet<>();
        this.attendances = new LinkedHashSet<>();
    }

    @Override
    public void reset() {
        super.reset();
        this.registrations.clear();
        this.attendances.clear();
    }

    public void add(RegistrationDTO r) {
        this.registrations.add(r);
    }

    public void remove(RegistrationDTO r) {
        this.registrations.remove(r);
    }

    public void removeRegistration(int id) {
        List<RegistrationDTO> result = this.registrations.stream().filter(e -> e.getId() == id).collect(Collectors.toList());
        if (result.size() == 1) remove(result.get(0));
    }

    public void add(AttendanceDTO r) {
        this.attendances.add(r);
    }

    public void remove(AttendanceDTO r) {
        this.attendances.remove(r);
    }

    public void removeAttendance(int id) {
        List<AttendanceDTO> result = this.attendances.stream().filter(e -> e.getId() == id).collect(Collectors.toList());
        if (result.size() == 1) remove(result.get(0));
    }

    public void setRegistrations(Set<RegistrationDTO> registrations) {
        this.registrations = registrations;
    }

    public Set<AttendanceDTO> getAttendances() {
        return attendances;
    }

    public void setAttendances(Set<AttendanceDTO> attendances) {
        this.attendances = attendances;
    }

    public Set<RegistrationDTO> getRegistrations() {
        return registrations;
    }

    public void setRegistrations(Collection<RegistrationDTO> registrations) {
        this.registrations = registrations.stream().collect(Collectors.toSet());
    }
}
