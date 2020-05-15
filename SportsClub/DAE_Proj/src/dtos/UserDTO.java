package dtos;

import java.util.Collection;
import java.util.LinkedHashSet;
import java.util.Set;
import java.util.stream.Collectors;

public class UserDTO extends AuthDTO {

    private String name;
    private String email;
    private Set<EmailDTO> emails;

    public UserDTO() {
        this.emails = new LinkedHashSet<>();
    }

    public UserDTO(String username, String password, String name, String email) {
        super(username, password);
        this.name = name;
        this.email = email;
        this.emails = new LinkedHashSet<>();
    }

    @Override
    public void reset() {
        super.reset();
        this.email = this.name = null;
        this.emails.clear();
    }

    public void add(EmailDTO e) {
        this.emails.add(e);
    }

    public void remove(EmailDTO e) { this.emails.remove(e); }

    public Set<EmailDTO> getEmails() {
        return emails;
    }

    public void setEmails(Collection<EmailDTO> emails) {
        this.emails = emails.stream().collect(Collectors.toSet());
    }

    public String getName() { return name; }

    public void setName(String name) { this.name = name; }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }
}
